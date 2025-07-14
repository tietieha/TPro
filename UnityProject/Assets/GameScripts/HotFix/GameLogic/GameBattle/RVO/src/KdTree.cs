/*
 * KdTree.cs
 * RVO2 Library C#
 *
 * Copyright 2008 University of North Carolina at Chapel Hill
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 * Please send all bug reports to <geom@cs.unc.edu>.
 *
 * The authors may be contacted via:
 *
 * Jur van den Berg, Stephen J. Guy, Jamie Snape, Ming C. Lin, Dinesh Manocha
 * Dept. of Computer Science
 * 201 S. Columbia St.
 * Frederick P. Brooks, Jr. Computer Science Bldg.
 * Chapel Hill, N.C. 27599-3175
 * United States of America
 *
 * <http://gamma.cs.unc.edu/RVO2/>
 */

using System.Collections.Generic;
using System;
using System.Collections;
using M.Battle;
using UnityEngine;

namespace RVO
{
    /**
     * <summary>Defines k-D trees for agents and static obstacles in the
     * simulation.</summary>
     */
    internal class KdTree
    {
        /**
         * <summary>Defines a node of an agent k-D tree.</summary>
         */
        private struct AgentTreeNode
        {
            internal int Begin;
            internal int End;
            internal int Left;
            internal int Right;
            internal float MaxX;
            internal float MaxY;
            internal float MinX;
            internal float MinY;
        }

        /**
         * <summary>Defines a pair of scalar values.</summary>
         */
        private struct FloatPair
        {
            private float _a;
            private float _b;

            /// <summary>
            /// Constructs and initializes a pair of scalar values.
            /// </summary>
            /// <param name="a">The first scalar value.</param>
            /// <param name="b">The second scalar value.</param>
            internal FloatPair(float a, float b)
            {
                _a = a;
                _b = b;
            }

            /**
             * <summary>Returns true if the first pair of scalar values is less
             * than the second pair of scalar values.</summary>
             *
             * <returns>True if the first pair of scalar values is less than the
             * second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator <(FloatPair pair1, FloatPair pair2)
            {
                return pair1._a < pair2._a ||
                       !(pair2._a < pair1._a) && pair1._b < pair2._b;
            }

            /**
             * <summary>Returns true if the first pair of scalar values is less
             * than or equal to the second pair of scalar values.</summary>
             *
             * <returns>True if the first pair of scalar values is less than or
             * equal to the second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator <=(FloatPair pair1, FloatPair pair2)
            {
                return (pair1._a == pair2._a && pair1._b == pair2._b) || pair1 < pair2;
            }

            /**
             * <summary>Returns true if the first pair of scalar values is
             * greater than the second pair of scalar values.</summary>
             *
             * <returns>True if the first pair of scalar values is greater than
             * the second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator >(FloatPair pair1, FloatPair pair2)
            {
                return !(pair1 <= pair2);
            }

            /**
             * <summary>Returns true if the first pair of scalar values is
             * greater than or equal to the second pair of scalar values.
             * </summary>
             *
             * <returns>True if the first pair of scalar values is greater than
             * or equal to the second pair of scalar values.</returns>
             *
             * <param name="pair1">The first pair of scalar values.</param>
             * <param name="pair2">The second pair of scalar values.</param>
             */
            public static bool operator >=(FloatPair pair1, FloatPair pair2)
            {
                return !(pair1 < pair2);
            }
        }

        /**
         * <summary>Defines a node of an obstacle k-D tree.</summary>
         */
        private class ObstacleTreeNode
        {
            internal Obstacle obstacle_;
            internal ObstacleTreeNode left_;
            internal ObstacleTreeNode right_;
        };

        /**
         * <summary>The maximum size of an agent k-D tree leaf.</summary>
         */
        private const int MAX_LEAF_SIZE = 10;

        private Agent[] agents_;
        private AgentTreeNode[] agentTree_;
        private ObstacleTreeNode obstacleTree_;

        /**
         * <summary>Builds an agent k-D tree.</summary>
         */
        internal void buildAgentTree(IList<Agent> simulatorAgents)
        {
            if (agents_ == null || agents_.Length != simulatorAgents.Count)
            {
                agents_ = new Agent[simulatorAgents.Count];

                for (int i = 0; i < agents_.Length; ++i)
                {
                    agents_[i] = simulatorAgents[i];
                }

                agentTree_ = new AgentTreeNode[2 * agents_.Length];

                for (int i = 0; i < agentTree_.Length; ++i)
                {
                    agentTree_[i] = new AgentTreeNode();
                }
            }

            if (agents_.Length != 0)
            {
                buildAgentTreeRecursive(0, agents_.Length, 0);
            }
        }

        /**
         * <summary>Builds an obstacle k-D tree.</summary>
         */
        internal void buildObstacleTree(ref IList<Obstacle> simulatorObstacles)
        {
            obstacleTree_ = new ObstacleTreeNode();

            IList<Obstacle> obstacles = new List<Obstacle>(simulatorObstacles.Count);

            for (int i = 0; i < simulatorObstacles.Count; ++i)
            {
                obstacles.Add(simulatorObstacles[i]);
            }

            obstacleTree_ = buildObstacleTreeRecursive(obstacles, ref simulatorObstacles);
        }

        /**
         * <summary>Computes the agent neighbors of the specified agent.
         * </summary>
         *
         * <param name="agent">The agent for which agent neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         */
        internal void computeAgentNeighbors(Agent agent, ref float rangeSq, AgentType agentType)
        {
            queryAgentTreeRecursive(agent, ref rangeSq, 0, agentType);
        }

        /**
         * <summary>Computes the obstacle neighbors of the specified agent.
         * </summary>
         *
         * <param name="agent">The agent for which obstacle neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         */
        internal void computeObstacleNeighbors(Agent agent, float rangeSq)
        {
            queryObstacleTreeRecursive(agent, rangeSq, obstacleTree_);
        }

        /**
         * <summary>Queries the visibility between two points within a specified
         * radius.</summary>
         *
         * <returns>True if q1 and q2 are mutually visible within the radius;
         * false otherwise.</returns>
         *
         * <param name="q1">The first point between which visibility is to be
         * tested.</param>
         * <param name="q2">The second point between which visibility is to be
         * tested.</param>
         * <param name="radius">The radius within which visibility is to be
         * tested.</param>
         */
        internal bool queryVisibility(Vector2 q1, Vector2 q2, float radius)
        {
            return queryVisibilityRecursive(q1, q2, radius, obstacleTree_);
        }

        internal int queryNearAgent(Vector2 point, float radius)
        {
            float rangeSq = float.MaxValue;
            int agentNo = -1;
            queryAgentTreeRecursive(point, ref rangeSq, ref agentNo, 0);
            if (rangeSq < radius * radius)
                return agentNo;
            return -1;
        }

        /**
         * <summary>Recursive method for building an agent k-D tree.</summary>
         *
         * <param name="begin">The beginning agent k-D tree node node index.
         * </param>
         * <param name="end">The ending agent k-D tree node index.</param>
         * <param name="node">The current agent k-D tree node index.</param>
         */
        private void buildAgentTreeRecursive(int begin, int end, int node)
        {
            agentTree_[node].Begin = begin;
            agentTree_[node].End = end;
            agentTree_[node].MinX = agentTree_[node].MaxX = agents_[begin].Position.x_;
            agentTree_[node].MinY = agentTree_[node].MaxY = agents_[begin].Position.y_;

            for (int i = begin + 1; i < end; ++i)
            {
                agentTree_[node].MaxX =
                    Math.Max(agentTree_[node].MaxX, agents_[i].Position.x_);
                agentTree_[node].MinX =
                    Math.Min(agentTree_[node].MinX, agents_[i].Position.x_);
                agentTree_[node].MaxY =
                    Math.Max(agentTree_[node].MaxY, agents_[i].Position.y_);
                agentTree_[node].MinY =
                    Math.Min(agentTree_[node].MinY, agents_[i].Position.y_);
            }

            if (end - begin > MAX_LEAF_SIZE)
            {
                /* No leaf node. */
                bool isVertical = agentTree_[node].MaxX - agentTree_[node].MinX >
                                  agentTree_[node].MaxY - agentTree_[node].MinY;
                float splitValue = 0.5f * (isVertical
                    ? agentTree_[node].MaxX + agentTree_[node].MinX
                    : agentTree_[node].MaxY + agentTree_[node].MinY);

                int left = begin;
                int right = end;

                while (left < right)
                {
                    while (left < right &&
                           (isVertical
                               ? agents_[left].Position.x_
                               : agents_[left].Position.y_) < splitValue)
                    {
                        ++left;
                    }

                    while (right > left &&
                           (isVertical
                               ? agents_[right - 1].Position.x_
                               : agents_[right - 1].Position.y_) >= splitValue)
                    {
                        --right;
                    }

                    if (left < right)
                    {
                        Agent tempAgent = agents_[left];
                        agents_[left] = agents_[right - 1];
                        agents_[right - 1] = tempAgent;
                        ++left;
                        --right;
                    }
                }

                int leftSize = left - begin;

                if (leftSize == 0)
                {
                    ++leftSize;
                    ++left;
                    ++right;
                }

                agentTree_[node].Left = node + 1;
                agentTree_[node].Right = node + 2 * leftSize;

                buildAgentTreeRecursive(begin, left, agentTree_[node].Left);
                buildAgentTreeRecursive(left, end, agentTree_[node].Right);
            }
        }

        /**
         * <summary>Recursive method for building an obstacle k-D tree.
         * </summary>
         *
         * <returns>An obstacle k-D tree node.</returns>
         *
         * <param name="obstacles">A list of obstacles.</param>
         */
        private ObstacleTreeNode buildObstacleTreeRecursive(IList<Obstacle> obstacles,
            ref IList<Obstacle> simulatorObstacles)
        {
            if (obstacles.Count == 0)
            {
                return null;
            }

            ObstacleTreeNode node = new ObstacleTreeNode();

            int optimalSplit = 0;
            int minLeft = obstacles.Count;
            int minRight = obstacles.Count;

            for (int i = 0; i < obstacles.Count; ++i)
            {
                int leftSize = 0;
                int rightSize = 0;

                Obstacle obstacleI1 = obstacles[i];
                Obstacle obstacleI2 = obstacleI1.next_;

                /* Compute optimal split node. */
                for (int j = 0; j < obstacles.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Obstacle obstacleJ1 = obstacles[j];
                    Obstacle obstacleJ2 = obstacleJ1.next_;

                    float j1LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_,
                        obstacleJ1.point_);
                    float j2LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_,
                        obstacleJ2.point_);

                    if (j1LeftOfI >= -RVOMath.RVO_EPSILON &&
                        j2LeftOfI >= -RVOMath.RVO_EPSILON)
                    {
                        ++leftSize;
                    }
                    else if (j1LeftOfI <= RVOMath.RVO_EPSILON &&
                             j2LeftOfI <= RVOMath.RVO_EPSILON)
                    {
                        ++rightSize;
                    }
                    else
                    {
                        ++leftSize;
                        ++rightSize;
                    }

                    if (new FloatPair(Math.Max(leftSize, rightSize),
                            Math.Min(leftSize, rightSize)) >=
                        new FloatPair(Math.Max(minLeft, minRight),
                            Math.Min(minLeft, minRight)))
                    {
                        break;
                    }
                }

                if (new FloatPair(Math.Max(leftSize, rightSize),
                        Math.Min(leftSize, rightSize)) < new FloatPair(
                        Math.Max(minLeft, minRight), Math.Min(minLeft, minRight)))
                {
                    minLeft = leftSize;
                    minRight = rightSize;
                    optimalSplit = i;
                }
            }

            {
                /* Build split node. */
                IList<Obstacle> leftObstacles = new List<Obstacle>(minLeft);

                for (int n = 0; n < minLeft; ++n)
                {
                    leftObstacles.Add(null);
                }

                IList<Obstacle> rightObstacles = new List<Obstacle>(minRight);

                for (int n = 0; n < minRight; ++n)
                {
                    rightObstacles.Add(null);
                }

                int leftCounter = 0;
                int rightCounter = 0;
                int i = optimalSplit;

                Obstacle obstacleI1 = obstacles[i];
                Obstacle obstacleI2 = obstacleI1.next_;

                for (int j = 0; j < obstacles.Count; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    Obstacle obstacleJ1 = obstacles[j];
                    Obstacle obstacleJ2 = obstacleJ1.next_;

                    float j1LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_,
                        obstacleJ1.point_);
                    float j2LeftOfI = RVOMath.leftOf(obstacleI1.point_, obstacleI2.point_,
                        obstacleJ2.point_);

                    if (j1LeftOfI >= -RVOMath.RVO_EPSILON &&
                        j2LeftOfI >= -RVOMath.RVO_EPSILON)
                    {
                        leftObstacles[leftCounter++] = obstacles[j];
                    }
                    else if (j1LeftOfI <= RVOMath.RVO_EPSILON &&
                             j2LeftOfI <= RVOMath.RVO_EPSILON)
                    {
                        rightObstacles[rightCounter++] = obstacles[j];
                    }
                    else
                    {
                        /* Split obstacle j. */
                        float t =
                            RVOMath.det(obstacleI2.point_ - obstacleI1.point_,
                                obstacleJ1.point_ - obstacleI1.point_) /
                            RVOMath.det(obstacleI2.point_ - obstacleI1.point_,
                                obstacleJ1.point_ - obstacleJ2.point_);

                        Vector2 splitPoint = obstacleJ1.point_ +
                                             t * (obstacleJ2.point_ - obstacleJ1.point_);

                        Obstacle newObstacle = new Obstacle();
                        newObstacle.point_ = splitPoint;
                        newObstacle.previous_ = obstacleJ1;
                        newObstacle.next_ = obstacleJ2;
                        newObstacle.convex_ = true;
                        newObstacle.direction_ = obstacleJ1.direction_;

                        newObstacle.id_ = simulatorObstacles.Count;

                        simulatorObstacles.Add(newObstacle);

                        obstacleJ1.next_ = newObstacle;
                        obstacleJ2.previous_ = newObstacle;

                        if (j1LeftOfI > 0.0f)
                        {
                            leftObstacles[leftCounter++] = obstacleJ1;
                            rightObstacles[rightCounter++] = newObstacle;
                        }
                        else
                        {
                            rightObstacles[rightCounter++] = obstacleJ1;
                            leftObstacles[leftCounter++] = newObstacle;
                        }
                    }
                }

                node.obstacle_ = obstacleI1;
                node.left_ =
                    buildObstacleTreeRecursive(leftObstacles, ref simulatorObstacles);
                node.right_ =
                    buildObstacleTreeRecursive(rightObstacles, ref simulatorObstacles);

                return node;
            }
        }

        private void queryAgentTreeRecursive(Vector2 position, ref float rangeSq,
            ref int agentNo, int node)
        {
            if (agentTree_[node].End - agentTree_[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].Begin; i < agentTree_[node].End; ++i)
                {
                    float distSq = RVOMath.absSq(position - agents_[i].Position);
                    if (distSq < rangeSq)
                    {
                        rangeSq = distSq;
                        agentNo = agents_[i].Id;
                    }
                }
            }
            else
            {
                float distSqLeft =
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Left].MinX - position.x_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        position.x_ - agentTree_[agentTree_[node].Left].MaxX)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Left].MinY - position.y_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        position.y_ - agentTree_[agentTree_[node].Left].MaxY));
                float distSqRight =
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Right].MinX - position.x_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        position.x_ - agentTree_[agentTree_[node].Right].MaxX)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Right].MinY - position.y_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        position.y_ - agentTree_[agentTree_[node].Right].MaxY));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < rangeSq)
                    {
                        queryAgentTreeRecursive(position, ref rangeSq, ref agentNo,
                            agentTree_[node].Left);

                        if (distSqRight < rangeSq)
                        {
                            queryAgentTreeRecursive(position, ref rangeSq, ref agentNo,
                                agentTree_[node].Right);
                        }
                    }
                }
                else
                {
                    if (distSqRight < rangeSq)
                    {
                        queryAgentTreeRecursive(position, ref rangeSq, ref agentNo,
                            agentTree_[node].Right);

                        if (distSqLeft < rangeSq)
                        {
                            queryAgentTreeRecursive(position, ref rangeSq, ref agentNo,
                                agentTree_[node].Left);
                        }
                    }
                }
            }
        }

        /**
         * <summary>Recursive method for computing the agent neighbors of the
         * specified agent.</summary>
         *
         * <param name="agent">The agent for which agent neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         * <param name="node">The current agent k-D tree node index.</param>
         * <param name="agentType">agent类型</param>
         */
        private void queryAgentTreeRecursive(Agent agent, ref float rangeSq, int node,
            AgentType agentType = AgentType.All)
        {
            if (agentTree_[node].End - agentTree_[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].Begin; i < agentTree_[node].End; ++i)
                {
                    if (agentType == AgentType.All || agents_[i].Type == agentType)
                        agent.InsertAgentNeighbor(agents_[i], ref rangeSq);
                }
            }
            else
            {
                float distSqLeft =
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Left].MinX - agent.Position.x_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agent.Position.x_ - agentTree_[agentTree_[node].Left].MaxX)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Left].MinY - agent.Position.y_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agent.Position.y_ - agentTree_[agentTree_[node].Left].MaxY));
                float distSqRight =
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Right].MinX - agent.Position.x_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agent.Position.x_ - agentTree_[agentTree_[node].Right].MaxX)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agentTree_[agentTree_[node].Right].MinY - agent.Position.y_)) +
                    RVOMath.sqr(Math.Max(0.0f,
                        agent.Position.y_ - agentTree_[agentTree_[node].Right].MaxY));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < rangeSq)
                    {
                        queryAgentTreeRecursive(agent, ref rangeSq,
                            agentTree_[node].Left, agentType);

                        if (distSqRight < rangeSq)
                        {
                            queryAgentTreeRecursive(agent, ref rangeSq,
                                agentTree_[node].Right, agentType);
                        }
                    }
                }
                else
                {
                    if (distSqRight < rangeSq)
                    {
                        queryAgentTreeRecursive(agent, ref rangeSq,
                            agentTree_[node].Right, agentType);

                        if (distSqLeft < rangeSq)
                        {
                            queryAgentTreeRecursive(agent, ref rangeSq,
                                agentTree_[node].Left, agentType);
                        }
                    }
                }
            }
        }

        /**
         * <summary>Recursive method for computing the obstacle neighbors of the
         * specified agent.</summary>
         *
         * <param name="agent">The agent for which obstacle neighbors are to be
         * computed.</param>
         * <param name="rangeSq">The squared range around the agent.</param>
         * <param name="node">The current obstacle k-D node.</param>
         */
        private void queryObstacleTreeRecursive(Agent agent, float rangeSq,
            ObstacleTreeNode node)
        {
            if (node != null)
            {
                Obstacle obstacle1 = node.obstacle_;
                Obstacle obstacle2 = obstacle1.next_;

                float agentLeftOfLine = RVOMath.leftOf(obstacle1.point_, obstacle2.point_,
                    agent.Position);

                queryObstacleTreeRecursive(agent, rangeSq,
                    agentLeftOfLine >= 0.0f ? node.left_ : node.right_);

                float distSqLine = RVOMath.sqr(agentLeftOfLine) /
                                   RVOMath.absSq(obstacle2.point_ - obstacle1.point_);

                if (distSqLine < rangeSq)
                {
                    if (agentLeftOfLine < 0.0f)
                    {
                        /*
                         * Try obstacle at this node only if agent is on right side of
                         * obstacle (and can see obstacle).
                         */
                        agent.InsertObstacleNeighbor(node.obstacle_, rangeSq);
                    }

                    /* Try other side of line. */
                    queryObstacleTreeRecursive(agent, rangeSq,
                        agentLeftOfLine >= 0.0f ? node.right_ : node.left_);
                }
            }
        }

        /**
         * <summary>Recursive method for querying the visibility between two
         * points within a specified radius.</summary>
         *
         * <returns>True if q1 and q2 are mutually visible within the radius;
         * false otherwise.</returns>
         *
         * <param name="q1">The first point between which visibility is to be
         * tested.</param>
         * <param name="q2">The second point between which visibility is to be
         * tested.</param>
         * <param name="radius">The radius within which visibility is to be
         * tested.</param>
         * <param name="node">The current obstacle k-D node.</param>
         */
        private bool queryVisibilityRecursive(Vector2 q1, Vector2 q2, float radius,
            ObstacleTreeNode node)
        {
            if (node == null)
            {
                return true;
            }

            Obstacle obstacle1 = node.obstacle_;
            Obstacle obstacle2 = obstacle1.next_;

            float q1LeftOfI = RVOMath.leftOf(obstacle1.point_, obstacle2.point_, q1);
            float q2LeftOfI = RVOMath.leftOf(obstacle1.point_, obstacle2.point_, q2);
            float invLengthI = 1.0f / RVOMath.absSq(obstacle2.point_ - obstacle1.point_);

            if (q1LeftOfI >= 0.0f && q2LeftOfI >= 0.0f)
            {
                return queryVisibilityRecursive(q1, q2, radius, node.left_) &&
                       ((RVOMath.sqr(q1LeftOfI) * invLengthI >= RVOMath.sqr(radius) &&
                         RVOMath.sqr(q2LeftOfI) * invLengthI >= RVOMath.sqr(radius)) ||
                        queryVisibilityRecursive(q1, q2, radius, node.right_));
            }

            if (q1LeftOfI <= 0.0f && q2LeftOfI <= 0.0f)
            {
                return queryVisibilityRecursive(q1, q2, radius, node.right_) &&
                       ((RVOMath.sqr(q1LeftOfI) * invLengthI >= RVOMath.sqr(radius) &&
                         RVOMath.sqr(q2LeftOfI) * invLengthI >= RVOMath.sqr(radius)) ||
                        queryVisibilityRecursive(q1, q2, radius, node.left_));
            }

            if (q1LeftOfI >= 0.0f && q2LeftOfI <= 0.0f)
            {
                /* One can see through obstacle from left to right. */
                return queryVisibilityRecursive(q1, q2, radius, node.left_) &&
                       queryVisibilityRecursive(q1, q2, radius, node.right_);
            }

            float point1LeftOfQ = RVOMath.leftOf(q1, q2, obstacle1.point_);
            float point2LeftOfQ = RVOMath.leftOf(q1, q2, obstacle2.point_);
            float invLengthQ = 1.0f / RVOMath.absSq(q2 - q1);

            return point1LeftOfQ * point2LeftOfQ >= 0.0f &&
                   RVOMath.sqr(point1LeftOfQ) * invLengthQ > RVOMath.sqr(radius) &&
                   RVOMath.sqr(point2LeftOfQ) * invLengthQ > RVOMath.sqr(radius) &&
                   queryVisibilityRecursive(q1, q2, radius, node.left_) &&
                   queryVisibilityRecursive(q1, q2, radius, node.right_);
        }

        internal List<int> GetAgentsInRange(Vector2 position, float range, int side = -1, bool isVirtual = false)
        {
            List<int> agentsInRange = new List<int>();
            if (agents_ == null || agents_.Length == 0) return agentsInRange;
            float rangeSq = range * range;
            GetAgentsInRangeRecursive(position, rangeSq, side, 0, agentsInRange, isVirtual);
            return agentsInRange;
        }

        private void GetAgentsInRangeRecursive(Vector2 position, float rangeSq, int side, int node,
            List<int> agentsInRange, bool isVirtual = false)
        {
            if (agentTree_[node].End - agentTree_[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].Begin; i < agentTree_[node].End; ++i)
                {
                    if (isVirtual == agents_[i].IsVirtual && (side == 0 || agents_[i].Side == side))
                    {
                        float distSq = RVOMath.absSq(position - agents_[i].Position);
                        if (distSq < rangeSq)
                        {
                            agentsInRange.Add(agents_[i].Id);
                        }
                    }
                }
            }
            else
            {
                float distSqLeft = RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Left].MinX - position.x_)) +
                                   RVOMath.sqr(Math.Max(0.0f, position.x_ - agentTree_[agentTree_[node].Left].MaxX)) +
                                   RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Left].MinY - position.y_)) +
                                   RVOMath.sqr(Math.Max(0.0f, position.y_ - agentTree_[agentTree_[node].Left].MaxY));
                float distSqRight = RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Right].MinX - position.x_)) +
                                    RVOMath.sqr(Math.Max(0.0f, position.x_ - agentTree_[agentTree_[node].Right].MaxX)) +
                                    RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Right].MinY - position.y_)) +
                                    RVOMath.sqr(Math.Max(0.0f, position.y_ - agentTree_[agentTree_[node].Right].MaxY));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < rangeSq)
                    {
                        GetAgentsInRangeRecursive(position, rangeSq, side, agentTree_[node].Left, agentsInRange,
                            isVirtual);

                        if (distSqRight < rangeSq)
                        {
                            GetAgentsInRangeRecursive(position, rangeSq, side, agentTree_[node].Right, agentsInRange,
                                isVirtual);
                        }
                    }
                }
                else
                {
                    if (distSqRight < rangeSq)
                    {
                        GetAgentsInRangeRecursive(position, rangeSq, side, agentTree_[node].Right, agentsInRange,
                            isVirtual);

                        if (distSqLeft < rangeSq)
                        {
                            GetAgentsInRangeRecursive(position, rangeSq, side, agentTree_[node].Left, agentsInRange,
                                isVirtual);
                        }
                    }
                }
            }
        }

        internal List<int> GetAgentsInSectorRange(Vector2 position, float range, float angle, Vector2 direction,
            int side)
        {
            List<int> agentsInRange = new List<int>();
            float rangeSq = range * range;
            float halfAngle = angle / 2;
            GetAgentsInSectorRangeRecursive(position, rangeSq, halfAngle, direction, side, 0, agentsInRange);
            return agentsInRange;
        }

        private void GetAgentsInSectorRangeRecursive(Vector2 position, float rangeSq, float halfAngle,
            Vector2 direction, int side, int node, List<int> agentsInRange, bool isVirtual = false)
        {
            if (agentTree_[node].End - agentTree_[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].Begin; i < agentTree_[node].End; ++i)
                {
                    if ((side == 0 || agents_[i].Side == side) && agents_[i].IsVirtual == isVirtual)
                    {
                        Vector2 toAgent = agents_[i].Position - position;
                        float distSq = RVOMath.absSq(toAgent);
                        if (distSq < rangeSq)
                        {
                            float angleToAgent = Vector2.Angle(direction, toAgent);
                            if (angleToAgent <= halfAngle)
                            {
                                agentsInRange.Add(agents_[i].Id);
                            }
                        }
                    }
                }
            }
            else
            {
                float distSqLeft = RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Left].MinX - position.x_)) +
                                   RVOMath.sqr(Math.Max(0.0f, position.x_ - agentTree_[agentTree_[node].Left].MaxX)) +
                                   RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Left].MinY - position.y_)) +
                                   RVOMath.sqr(Math.Max(0.0f, position.y_ - agentTree_[agentTree_[node].Left].MaxY));
                float distSqRight = RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Right].MinX - position.x_)) +
                                    RVOMath.sqr(Math.Max(0.0f, position.x_ - agentTree_[agentTree_[node].Right].MaxX)) +
                                    RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Right].MinY - position.y_)) +
                                    RVOMath.sqr(Math.Max(0.0f, position.y_ - agentTree_[agentTree_[node].Right].MaxY));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < rangeSq)
                    {
                        GetAgentsInSectorRangeRecursive(position, rangeSq, halfAngle, direction, side,
                            agentTree_[node].Left, agentsInRange, isVirtual);

                        if (distSqRight < rangeSq)
                        {
                            GetAgentsInSectorRangeRecursive(position, rangeSq, halfAngle, direction, side,
                                agentTree_[node].Right, agentsInRange, isVirtual);
                        }
                    }
                }
                else
                {
                    if (distSqRight < rangeSq)
                    {
                        GetAgentsInSectorRangeRecursive(position, rangeSq, halfAngle, direction, side,
                            agentTree_[node].Right, agentsInRange, isVirtual);

                        if (distSqLeft < rangeSq)
                        {
                            GetAgentsInSectorRangeRecursive(position, rangeSq, halfAngle, direction, side,
                                agentTree_[node].Left, agentsInRange, isVirtual);
                        }
                    }
                }
            }
        }

        internal List<int> GetAgentsInRectangularRange(Vector2 position, float width, float height, int side)
        {
            List<int> agentsInRange = new List<int>();
            GetAgentsInRectangularRangeRecursive(position, width, height, side, 0, agentsInRange);
            return agentsInRange;
        }

        private void GetAgentsInRectangularRangeRecursive(Vector2 position, float width, float height, int side,
            int node, List<int> agentsInRange)
        {
            if (agentTree_[node].End - agentTree_[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].Begin; i < agentTree_[node].End; ++i)
                {
                    if (side == -1 || agents_[i].Side == side)
                    {
                        Vector2 toAgent = agents_[i].Position - position;
                        if (Math.Abs(toAgent.x_) <= width / 2 && Math.Abs(toAgent.y_) <= height / 2)
                        {
                            agentsInRange.Add(agents_[i].Id);
                        }
                    }
                }
            }
            else
            {
                float distSqLeft = RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Left].MinX - position.x_)) +
                                   RVOMath.sqr(Math.Max(0.0f, position.x_ - agentTree_[agentTree_[node].Left].MaxX)) +
                                   RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Left].MinY - position.y_)) +
                                   RVOMath.sqr(Math.Max(0.0f, position.y_ - agentTree_[agentTree_[node].Left].MaxY));
                float distSqRight = RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Right].MinX - position.x_)) +
                                    RVOMath.sqr(Math.Max(0.0f, position.x_ - agentTree_[agentTree_[node].Right].MaxX)) +
                                    RVOMath.sqr(Math.Max(0.0f, agentTree_[agentTree_[node].Right].MinY - position.y_)) +
                                    RVOMath.sqr(Math.Max(0.0f, position.y_ - agentTree_[agentTree_[node].Right].MaxY));

                if (distSqLeft < distSqRight)
                {
                    if (distSqLeft < width * width / 4 + height * height / 4)
                    {
                        GetAgentsInRectangularRangeRecursive(position, width, height, side, agentTree_[node].Left,
                            agentsInRange);

                        if (distSqRight < width * width / 4 + height * height / 4)
                        {
                            GetAgentsInRectangularRangeRecursive(position, width, height, side, agentTree_[node].Right,
                                agentsInRange);
                        }
                    }
                }
                else
                {
                    if (distSqRight < width * width / 4 + height * height / 4)
                    {
                        GetAgentsInRectangularRangeRecursive(position, width, height, side, agentTree_[node].Right,
                            agentsInRange);

                        if (distSqLeft < width * width / 4 + height * height / 4)
                        {
                            GetAgentsInRectangularRangeRecursive(position, width, height, side, agentTree_[node].Left,
                                agentsInRange);
                        }
                    }
                }
            }
        }

        internal List<int> GetAgentsInRange(float x, float y, float range, int side, bool isVirtual = false)
        {
            Vector2 position = new Vector2(x, y);
            return GetAgentsInRange(position, range, side, isVirtual);
        }

        internal List<int> GetAgentsInRotatedSectorRange(float x, float y, float range, float angle, float angleRad,
            int side = -1, bool isVirtual = false)
        {
            Vector2 center = new Vector2(x, y);
            // 扇形中心线方向：将默认方向 (1,0) 旋转 angleRad
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);
            Vector2 direction = new Vector2(cos, sin); // 由 (1, 0) 旋转得来
            return GetAgentsInSectorRange(x, y, range, angle, direction, side, isVirtual);
        }

        internal List<int> GetAgentsInSectorRange(float x, float y, float range, float angle, Vector2 direction,
            int side = -1, bool isVirtual = false)
        {
            List<int> agentsInRange = new List<int>();
            float rangeSq = range * range;
            float halfAngle = angle / 2;
            GetAgentsInSectorRangeRecursive(new Vector2(x, y), rangeSq, halfAngle, direction, side, 0, agentsInRange,
                isVirtual);
            return agentsInRange;
        }

        internal List<int> GetAgentsInRectangularRange(float x, float y, float width, float height, int side)
        {
            Vector2 position = new Vector2(x, y);
            return GetAgentsInRectangularRange(position, width, height, side);
        }

        internal List<int> GetRotatedRectFromLeftCenter(float centerX, float centerY, float width, float height,
            float angleRad,
            int side = -1, bool isVirtual = false)
        {
            float halfH = height / 2f;
            float cos = (float)Math.Cos(angleRad);
            float sin = (float)Math.Sin(angleRad);

            // 以左侧中点为锚点，定义原始4个角点（局部坐标系）
            Vector2[] localCorners = new Vector2[4];
            localCorners[0] = new Vector2(0, -halfH); // 左下
            localCorners[1] = new Vector2(width, -halfH); // 右下
            localCorners[2] = new Vector2(width, halfH); // 右上
            localCorners[3] = new Vector2(0, halfH); // 左上

            List<Vector2> corners = new List<Vector2>();

            for (int i = 0; i < 4; i++)
            {
                float x = localCorners[i].x_;
                float y = localCorners[i].y_;

                // 旋转（绕Z轴逆时针）
                float rx = x * cos - y * sin;
                float ry = x * sin + y * cos;

                // 加上旋转中心坐标
                corners.Add(new Vector2(centerX + rx, centerY + ry));
            }

            RvoSimulatorDebug._showTime = 10;
            RvoSimulatorDebug.drawRectPoints.Clear();
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(localCorners[0].x() + centerX, 0,
                localCorners[0].y() + centerY));
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(localCorners[1].x() + centerX, 0,
                localCorners[1].y() + centerY));
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(localCorners[2].x() + centerX, 0,
                localCorners[2].y() + centerY));
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(localCorners[3].x() + centerX, 0,
                localCorners[3].y() + centerY));
            
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(corners[0].x(), 0, corners[0].y()));
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(corners[1].x(), 0, corners[1].y()));
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(corners[2].x(), 0, corners[2].y()));
            RvoSimulatorDebug.drawRectPoints.Add(new Vector3(corners[3].x(), 0, corners[3].y()));
            return GetAgentsInPolygonRange(corners, side, isVirtual);
        }

        private List<int> GetAgentsInPolygonRange(List<Vector2> polygonPoints, int side, bool isVirtual)
        {
            List<int> agentsInPolygon = new List<int>();
            if (polygonPoints == null || polygonPoints.Count < 3) return agentsInPolygon;
            GetAgentsInPolygonRangeRecursive(polygonPoints, side, isVirtual, 0, agentsInPolygon);
            return agentsInPolygon;
        }

        private void GetAgentsInPolygonRangeRecursive(List<Vector2> polygonPoints, int side, bool isVirtual, int node,
            List<int> agentsInPolygon)
        {
            if (agentTree_[node].End - agentTree_[node].Begin <= MAX_LEAF_SIZE)
            {
                for (int i = agentTree_[node].Begin; i < agentTree_[node].End; ++i)
                {
                    if ((side == 0 || agents_[i].Side == side) && agents_[i].IsVirtual == isVirtual)
                    {
                        if (IsCircleIntersectPolygon(agents_[i].Position, agents_[i].radius_, polygonPoints))
                        {
                            agentsInPolygon.Add(agents_[i].Id);
                        }
                    }
                }
            }
            else
            {
                if (IsRectIntersectPolygon(agentTree_[node], polygonPoints))
                {
                    GetAgentsInPolygonRangeRecursive(polygonPoints, side, isVirtual, agentTree_[node].Left,
                        agentsInPolygon);
                    GetAgentsInPolygonRangeRecursive(polygonPoints, side, isVirtual, agentTree_[node].Right,
                        agentsInPolygon);
                }
            }
        }

        private bool IsPointInPolygon(Vector2 point, List<Vector2> polygon)
        {
            int n = polygon.Count;
            bool inside = false;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                if (((polygon[i].y_ > point.y_) != (polygon[j].y_ > point.y_)) &&
                    (point.x_ < (polygon[j].x_ - polygon[i].x_) * (point.y_ - polygon[i].y_) /
                        (polygon[j].y_ - polygon[i].y_) + polygon[i].x_))
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        private bool IsCircleIntersectPolygon(Vector2 center, float radius, List<Vector2> polygon)
        {
            // 1. 圆心在多边形内
            if (IsPointInPolygon(center, polygon))
                return true;

            // 2. 多边形任意一条边与圆相交
            for (int i = 0; i < polygon.Count; i++)
            {
                Vector2 a = polygon[i];
                Vector2 b = polygon[(i + 1) % polygon.Count];

                float dist = DistancePointToSegment(center, a, b);
                if (dist <= radius)
                    return true;
            }

            // 3. 多边形有顶点在圆内
            foreach (var vertex in polygon)
            {
                if (RVOMath.abs(center - vertex) <= radius)
                    return true;
            }

            return false; // 没交集
        }
        
        private float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
        {
            Vector2 ab = b - a;
            Vector2 ap = p - a;

            float abLenSq = RVOMath.absSq(ab);
            if (abLenSq == 0f) return RVOMath.abs(p - a); // a == b，退化为点

            float t = Math.Max(0, Math.Min(1, RVOMath.Dot(ap, ab) / abLenSq));
            Vector2 closest = a + t * ab;
            return RVOMath.abs(p - closest);
        }


        private bool IsRectIntersectPolygon(AgentTreeNode node, List<Vector2> polygon)
        {
            Vector2[] rectCorners = new Vector2[]
            {
                new Vector2(node.MinX, node.MinY),
                new Vector2(node.MinX, node.MaxY),
                new Vector2(node.MaxX, node.MinY),
                new Vector2(node.MaxX, node.MaxY),
            };

            foreach (var corner in rectCorners)
            {
                if (IsPointInPolygon(corner, polygon)) return true;
            }

            foreach (var p in polygon)
            {
                if (p.x_ >= node.MinX && p.x_ <= node.MaxX && p.y_ >= node.MinY && p.y_ <= node.MaxY)
                    return true;
            }
            
            for (int i = 0; i < 4; i++)
            {
                Vector2 r1 = rectCorners[i];
                Vector2 r2 = rectCorners[(i + 1) % 4];

                for (int j = 0; j < polygon.Count; j++)
                {
                    Vector2 p1 = polygon[j];
                    Vector2 p2 = polygon[(j + 1) % polygon.Count];

                    if (DoLineSegmentsIntersect(r1, r2, p1, p2))
                        return true;
                }
            }

            return false;
        }
        
        private bool DoLineSegmentsIntersect(Vector2 a1, Vector2 a2, Vector2 b1, Vector2 b2)
        {
            // 基于跨立实验
            float d1 = RVOMath.leftOf(a1, a2, b1);
            float d2 = RVOMath.leftOf(a1, a2, b2);
            float d3 = RVOMath.leftOf(b1, b2, a1);
            float d4 = RVOMath.leftOf(b1, b2, a2);

            return (d1 * d2 < 0) && (d3 * d4 < 0);
        }
    }
}