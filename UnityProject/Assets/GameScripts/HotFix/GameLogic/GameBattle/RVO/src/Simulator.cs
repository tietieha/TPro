/*
 * Simulator.cs
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
using System.Threading;
using FixPoint;
using UnityEngine;
using XLua;

namespace RVO
{
    public enum AgentType
    {
        All,
        VirtualHero,
        SoldierAgent
    }

    /**
     * <summary>Defines the simulation.</summary>
     */
    [LuaCallCSharp]
    public class Simulator
    {
        /**
         * <summary>Defines a worker.</summary>
         */
        private class Worker
        {
            private ManualResetEvent _doneEvent;
            private int _end;
            private int _start;
            private Simulator _simulator;
            private CancellationTokenSource _cancellationTokenSource; // 用于控制取消
            private CancellationToken _cancellationToken;

            /**
             * <summary>Constructs and initializes a worker.</summary>
             *
             * <param name="start">Start.</param>
             * <param name="end">End.</param>
             * <param name="doneEvent">Done event.</param>
             * <param name="simulator">Simulator Instance</param>
             */
            internal Worker(int start, int end, ManualResetEvent doneEvent, Simulator simulator)
            {
                _start = start;
                _end = end;
                _doneEvent = doneEvent;
                _simulator = simulator;
            }

            internal void Config(int start, int end)
            {
                _start = start;
                _end = end;
            }

            /**
             * <summary>Performs a simulation step.</summary>
             *
             * <param name="obj">Unused.</param>
             */
            internal void Step(object obj)
            {
                for (int index = _start; index < _end; ++index)
                {
                    if (_cancellationToken.IsCancellationRequested) // 如果请求了取消
                    {
                        return; // 提前退出任务
                    }

                    _simulator.Agents[index].ComputeNeighbors(_simulator.KdTree);
                    _simulator.Agents[index].ComputeNewVelocity(_simulator.TimeStep);
                }

                _doneEvent.Set();
            }

            /**
             * <summary>updates the two-dimensional position and
             * two-dimensional velocity of each agent.</summary>
             *
             * <param name="obj">Unused.</param>
             */
            internal void Update(object obj)
            {
                for (int index = _start; index < _end; ++index)
                {
                    if (_cancellationToken.IsCancellationRequested) // 如果请求了取消
                    {
                        return; // 提前退出任务
                    }

                    _simulator.Agents[index].Update(_simulator.TimeStep);
                }

                _doneEvent.Set();
            }

            internal void Cancel()
            {
                _cancellationTokenSource.Cancel();
            }

            // 释放资源
            internal void Dispose()
            {
                _cancellationTokenSource.Dispose();
            }
        }

        internal IDictionary<int, int> AgentNo2IndexDict;
        internal IDictionary<int, int> Index2AgentNoDict;
        internal IList<Agent> Agents;
        internal IList<Obstacle> Obstacles;
        internal IList<Agent> VirtualHeroAgents;
        internal KdTree KdTree;
        internal KdTree DeadKdTree;
        internal KdTree VirtualHeroKdTree;
        internal float TimeStep;

        internal IList<Agent> DeadAgents;
        internal IDictionary<int, int> DeadAgentNo2IndexDict;
        internal IDictionary<int, int> DeadIndex2AgentNoDict;

        // private static Simulator instance_ = new Simulator();

        private Agent _defaultAgent;
        private ManualResetEvent[] _doneEvents;
        private Worker[] _workers;
        private int _numWorkers;
        private int _workerAgentCount;
        private float _globalTime;

        // public static Simulator Instance
        // {
        //     get
        //     {
        //         return instance_;
        //     }
        // }
        /// <summary>
        /// 代理复活
        /// </summary>
        /// <param name="agentNo"></param>
        public void AgentRevive(int agentNo)
        {
            var agent = (SoldierAgent)DeadAgents[DeadAgentNo2IndexDict[agentNo]];
            if (AgentNo2IndexDict.ContainsKey(agentNo))
            {
                agent.NeedDelete = false;
                return;
            }

            agent.NeedDelete = false;
            Agents.Add(agent);
            OnAddAgent();
        }

        public void DelAgent(int agentNo)
        {
            var agent = Agents[AgentNo2IndexDict[agentNo]];
            agent.NeedDelete = true;
            if (DeadAgentNo2IndexDict.ContainsKey(agentNo))
            {
                return;
            }

            DeadAgents.Add(agent);
            OnAddDeadAgent();
            DeadKdTree.buildAgentTree(DeadAgents);
        }

        void UpdateDeleteAgent()
        {
            HashSet<int> removeIds = null;
            bool isDelete = false;
            for (int i = Agents.Count - 1; i >= 0; i--)
            {
                if (Agents[i].NeedDelete)
                {
                    if (removeIds == null)
                    {
                        removeIds = new HashSet<int>();
                    }

                    removeIds.Add(Agents[i].Id);
                    Agents.RemoveAt(i);
                    isDelete = true;
                }
            }

            if (isDelete)
            {
                OnDelAgent();
                // 清理已经失效的TargetId
                foreach (var agent in Agents)
                {
                    if (removeIds.Contains(agent.TargetId))
                    {
                        agent.SetTargetId(0, false);
                    }
                }
            }
        }

        static int _sTotalID = 0;

        /**
         * <summary>Adds a new agent with default properties to the simulation.
         * </summary>
         *
         * <returns>The number of the agent, or -1 when the agent defaults have
         * not been set.</returns>
         *
         * <param name="position">The two-dimensional starting position of this
         * agent.</param>
         */
        public int AddAgent(Vector2 position)
        {
            if (_defaultAgent == null)
            {
                return -1;
            }

            Agent agent = new Agent();
            agent.Id = _sTotalID;
            _sTotalID++;
            agent.maxNeighbors_ = _defaultAgent.maxNeighbors_;
            agent.maxSpeed_ = _defaultAgent.maxSpeed_;
            agent.neighborDist_ = _defaultAgent.neighborDist_;
            agent.Position = position;
            agent.radius_ = _defaultAgent.radius_;
            agent.timeHorizon_ = _defaultAgent.timeHorizon_;
            agent.timeHorizonObst_ = _defaultAgent.timeHorizonObst_;
            agent.velocity_ = _defaultAgent.velocity_;
            Agents.Add(agent);
            OnAddAgent();
            return agent.Id;
        }

        public int addAgent_Radius_Lua(float x, float y, float radius, float avoidanceRatio)
        {
            if (_defaultAgent == null)
            {
                return -1;
            }

            Agent agent = new Agent();
            agent.Id = _sTotalID;
            _sTotalID++;
            agent.maxNeighbors_ = _defaultAgent.maxNeighbors_;
            agent.maxSpeed_ = _defaultAgent.maxSpeed_;
            agent.neighborDist_ = _defaultAgent.neighborDist_;
            agent.Position = new Vector2(x, y);
            agent.radius_ = radius;
            agent.timeHorizon_ = _defaultAgent.timeHorizon_;
            agent.timeHorizonObst_ = _defaultAgent.timeHorizonObst_;
            agent.velocity_ = _defaultAgent.velocity_;
            agent.AvoidanceRatio = avoidanceRatio;
            Agents.Add(agent);
            OnAddAgent();
            return agent.Id;
        }

        void OnDelAgent()
        {
            AgentNo2IndexDict.Clear();
            Index2AgentNoDict.Clear();

            for (int i = 0; i < Agents.Count; i++)
            {
                int agentNo = Agents[i].Id;
                AgentNo2IndexDict.Add(agentNo, i);
                Index2AgentNoDict.Add(i, agentNo);
            }
        }

        public void SetAgentPosition(int agentNo, float x, float y)
        {
            SoldierAgent agent = GetAgent(agentNo);
            if (agent == null) return;
            agent.Position = new Vector2(x, y);
            agent.SyncLuaPos();
        }

        public void OnDeadAgentRevive()
        {
            DeadAgentNo2IndexDict.Clear();
            DeadIndex2AgentNoDict.Clear();
            List<int> reviveIds = new List<int>();
            for (int i = DeadAgents.Count - 1; i >= 0; i--)
            {
                if (!DeadAgents[i].NeedDelete)
                {
                    reviveIds.Add(i);
                }
            }

            for (int i = 0; i < reviveIds.Count; i++)
            {
                DeadAgents.RemoveAt(reviveIds[i]);
            }

            for (int i = 0; i < DeadAgents.Count; i++)
            {
                int agentNo = DeadAgents[i].Id;
                DeadAgentNo2IndexDict.Add(agentNo, i);
                DeadIndex2AgentNoDict.Add(i, agentNo);
            }
        }

        void OnAddAgent()
        {
            if (Agents.Count == 0)
                return;

            int index = Agents.Count - 1;
            int agentNo = Agents[index].Id;
            AgentNo2IndexDict.Add(agentNo, index);
            Index2AgentNoDict.Add(index, agentNo);
        }

        void OnAddDeadAgent()
        {
            if (DeadAgents.Count == 0)
                return;

            int index = DeadAgents.Count - 1;
            int agentNo = DeadAgents[index].Id;
            DeadAgentNo2IndexDict.Add(agentNo, index);
            DeadIndex2AgentNoDict.Add(index, agentNo);
        }

        /**
         * <summary>Adds a new agent to the simulation.</summary>
         *
         * <returns>The number of the agent.</returns>
         *
         * <param name="position">The two-dimensional starting position of this
         * agent.</param>
         * <param name="neighborDist">The maximum distance (center point to
         * center point) to other agents this agent takes into account in the
         * navigation. The larger this number, the longer the running time of
         * the simulation. If the number is too low, the simulation will not be
         * safe. Must be non-negative.</param>
         * <param name="maxNeighbors">The maximum number of other agents this
         * agent takes into account in the navigation. The larger this number,
         * the longer the running time of the simulation. If the number is too
         * low, the simulation will not be safe.</param>
         * <param name="timeHorizon">The minimal amount of time for which this
         * agent's velocities that are computed by the simulation are safe with
         * respect to other agents. The larger this number, the sooner this
         * agent will respond to the presence of other agents, but the less
         * freedom this agent has in choosing its velocities. Must be positive.
         * </param>
         * <param name="timeHorizonObst">The minimal amount of time for which
         * this agent's velocities that are computed by the simulation are safe
         * with respect to obstacles. The larger this number, the sooner this
         * agent will respond to the presence of obstacles, but the less freedom
         * this agent has in choosing its velocities. Must be positive.</param>
         * <param name="radius">The radius of this agent. Must be non-negative.
         * </param>
         * <param name="maxSpeed">The maximum speed of this agent. Must be
         * non-negative.</param>
         * <param name="velocity">The initial two-dimensional linear velocity of
         * this agent.</param>
         */
        public int addAgent(Vector2 position, float neighborDist, int maxNeighbors, float timeHorizon,
            float timeHorizonObst, float radius, float maxSpeed, Vector2 velocity)
        {
            Agent agent = new Agent();
            agent.Id = _sTotalID;
            _sTotalID++;
            agent.maxNeighbors_ = maxNeighbors;
            agent.maxSpeed_ = maxSpeed;
            agent.neighborDist_ = neighborDist;
            agent.Position = position;
            agent.radius_ = radius;
            agent.timeHorizon_ = timeHorizon;
            agent.timeHorizonObst_ = timeHorizonObst;
            agent.velocity_ = velocity;
            Agents.Add(agent);
            OnAddAgent();
            return agent.Id;
        }

        public void AddVirtualHeroAgent(int id, float x, float y, int teamId,
            float radius, int side, float maxSpeed, float attackRange,
            bool isVirtual, bool isMelee, float stopDis,
            float avoidanceRatio, LuaArrAccess luaArray, AgentType agentType,
            float neighborDist, int maxNeighbors, float timeHorizon)
        {
            var agent = new SoldierAgent(this, id, teamId, attackRange,
                side, isVirtual, isMelee, stopDis, luaArray);
            agent.Position = new Vector2(x, y);
            agent.radius_ = radius;
            agent.AvoidanceRatio = avoidanceRatio;
            agent.maxSpeed_ = maxSpeed;
            agent.Type = agentType;
            agent.maxNeighbors_ = maxNeighbors;
            agent.neighborDist_ = neighborDist;
            agent.timeHorizon_ = timeHorizon;
            agent.timeHorizonObst_ = _defaultAgent.timeHorizonObst_;
            agent.velocity_ = agent.Position;

            Agents.Add(agent);
            OnAddAgent();
            VirtualHeroAgents.Add(agent);
        }

        public void AddSoliderAgent(int id, float x, float y, int teamId,
            float radius, int side, float maxSpeed, float attackRange,
            bool isVirtual, bool isMelee, float stopDis,
            float avoidanceRatio, int groupId, LuaArrAccess luaArray, AgentType agentType)
        {
            var agent = new SoldierAgent(this, id, teamId, attackRange,
                side, isVirtual, isMelee, stopDis, luaArray);
            agent.Position = new Vector2(x, y);
            agent.radius_ = radius;
            agent.AvoidanceRatio = avoidanceRatio;
            agent.maxSpeed_ = maxSpeed;
            agent.Type = agentType;
            agent.maxNeighbors_ = _defaultAgent.maxNeighbors_;
            // agent.maxSpeed_ = _defaultAgent.maxSpeed_;
            agent.neighborDist_ = _defaultAgent.neighborDist_;
            // agent.Position = position;
            // agent.radius_ = _defaultAgent.radius_;
            agent.timeHorizon_ = _defaultAgent.timeHorizon_;
            agent.timeHorizonObst_ = _defaultAgent.timeHorizonObst_;
            agent.velocity_ = Vector2.Zero;
            agent.GroupId = groupId;

            Agents.Add(agent);
            OnAddAgent();

            SoldierAgent groupAgent = GetAgent(groupId);
            groupAgent.AddSoldier(id);
        }

        public void BuildAgentTree()
        {
            KdTree.buildAgentTree(Agents);
            VirtualHeroKdTree.buildAgentTree(VirtualHeroAgents);
        }

        public void BuildDeadAgentTree()
        {
            DeadKdTree.buildAgentTree(DeadAgents);
        }

        public void SetAgentSpeed(int id, float speed)
        {
            var agent = Agents[AgentNo2IndexDict[id]];
            agent.maxSpeed_ = speed;
        }

        public void SetAgentAvoidRatio(int id, float avoidRatio)
        {
            var agent = Agents[AgentNo2IndexDict[id]];
            agent.AvoidanceRatio = avoidRatio;
        }

        /**
         * <summary>Adds a new obstacle to the simulation.</summary>
         *
         * <returns>The number of the first vertex of the obstacle, or -1 when
         * the number of vertices is less than two.</returns>
         *
         * <param name="vertices">List of the vertices of the polygonal obstacle
         * in counterclockwise order.</param>
         *
         * <remarks>To add a "negative" obstacle, e.g. a bounding polygon around
         * the environment, the vertices should be listed in clockwise order.
         * </remarks>
         */
        public int AddObstacle(IList<Vector2> vertices)
        {
            if (vertices.Count < 2)
            {
                return -1;
            }

            int obstacleNo = Obstacles.Count;

            for (int i = 0; i < vertices.Count; ++i)
            {
                Obstacle obstacle = new Obstacle();
                obstacle.point_ = vertices[i];

                if (i != 0)
                {
                    obstacle.previous_ = Obstacles[Obstacles.Count - 1];
                    obstacle.previous_.next_ = obstacle;
                }

                if (i == vertices.Count - 1)
                {
                    obstacle.next_ = Obstacles[obstacleNo];
                    obstacle.next_.previous_ = obstacle;
                }

                obstacle.direction_ = RVOMath.normalize(vertices[(i == vertices.Count - 1 ? 0 : i + 1)] - vertices[i]);

                if (vertices.Count == 2)
                {
                    obstacle.convex_ = true;
                }
                else
                {
                    obstacle.convex_ = (RVOMath.leftOf(vertices[(i == 0 ? vertices.Count - 1 : i - 1)], vertices[i],
                        vertices[(i == vertices.Count - 1 ? 0 : i + 1)]) >= 0.0f);
                }

                obstacle.id_ = Obstacles.Count;
                Obstacles.Add(obstacle);
            }

            return obstacleNo;
        }

        /**
         * <summary>Clears the simulation.</summary>
         */
        public void Clear()
        {
            Agents = new List<Agent>();
            DeadAgents = new List<Agent>();
            VirtualHeroAgents = new List<Agent>();
            AgentNo2IndexDict = new Dictionary<int, int>();
            Index2AgentNoDict = new Dictionary<int, int>();
            DeadAgentNo2IndexDict = new Dictionary<int, int>();
            DeadIndex2AgentNoDict = new Dictionary<int, int>();
            _defaultAgent = null;
            KdTree = new KdTree();
            Obstacles = new List<Obstacle>();
            _globalTime = 0.0f;
            TimeStep = 0.1f;
            DeadKdTree = new KdTree();
            VirtualHeroKdTree = new KdTree();

            SetNumWorkers(0);
        }

        /**
         * <summary>Performs a simulation step and updates the two-dimensional
         * position and two-dimensional velocity of each agent.</summary>
         *
         * <returns>The global time after the simulation step.</returns>
         */
        public float DoStep()
        {
            UpdateDeleteAgent();

            if (_workers == null)
            {
                _workers = new Worker[_numWorkers];
                _doneEvents = new ManualResetEvent[_workers.Length];
                _workerAgentCount = GetNumAgents();

                for (int block = 0; block < _workers.Length; ++block)
                {
                    _doneEvents[block] = new ManualResetEvent(false);
                    _workers[block] = new Worker(block * GetNumAgents() / _workers.Length,
                        (block + 1) * GetNumAgents() / _workers.Length, _doneEvents[block],
                        this
                    );
                }
            }

            if (_workerAgentCount != GetNumAgents())
            {
                _workerAgentCount = GetNumAgents();
                for (int block = 0; block < _workers.Length; ++block)
                {
                    _workers[block].Config(block * GetNumAgents() / _workers.Length,
                        (block + 1) * GetNumAgents() / _workers.Length);
                }
            }

            KdTree.buildAgentTree(Agents);
            VirtualHeroKdTree.buildAgentTree(VirtualHeroAgents);

            for (int block = 0; block < _workers.Length; ++block)
            {
                _doneEvents[block].Reset();
                ThreadPool.QueueUserWorkItem(_workers[block].Step);
            }

            WaitHandle.WaitAll(_doneEvents);

            for (int block = 0; block < _workers.Length; ++block)
            {
                _doneEvents[block].Reset();
                ThreadPool.QueueUserWorkItem(_workers[block].Update);
            }

            WaitHandle.WaitAll(_doneEvents);

            _globalTime += TimeStep;

            return _globalTime;
        }

        /**
         * <summary>Returns the specified agent neighbor of the specified agent.
         * </summary>
         *
         * <returns>The number of the neighboring agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose agent neighbor is
         * to be retrieved.</param>
         * <param name="neighborNo">The number of the agent neighbor to be
         * retrieved.</param>
         */
        public int GetAgentAgentNeighbor(int agentNo, int neighborNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].agentNeighbors_[neighborNo].Value.Id;
        }

        /**
         * <summary>Returns the maximum neighbor count of a specified agent.
         * </summary>
         *
         * <returns>The present maximum neighbor count of the agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose maximum neighbor
         * count is to be retrieved.</param>
         */
        public int GetAgentMaxNeighbors(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].maxNeighbors_;
        }

        /**
         * <summary>Returns the maximum speed of a specified agent.</summary>
         *
         * <returns>The present maximum speed of the agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose maximum speed is
         * to be retrieved.</param>
         */
        public float GetAgentMaxSpeed(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].maxSpeed_;
        }

        /**
         * <summary>Returns the maximum neighbor distance of a specified agent.
         * </summary>
         *
         * <returns>The present maximum neighbor distance of the agent.
         * </returns>
         *
         * <param name="agentNo">The number of the agent whose maximum neighbor
         * distance is to be retrieved.</param>
         */
        public float GetAgentNeighborDist(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].neighborDist_;
        }

        /**
         * <summary>Returns the count of agent neighbors taken into account to
         * compute the current velocity for the specified agent.</summary>
         *
         * <returns>The count of agent neighbors taken into account to compute
         * the current velocity for the specified agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose count of agent
         * neighbors is to be retrieved.</param>
         */
        public int GetAgentNumAgentNeighbors(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].agentNeighbors_.Count;
        }

        /**
         * <summary>Returns the count of obstacle neighbors taken into account
         * to compute the current velocity for the specified agent.</summary>
         *
         * <returns>The count of obstacle neighbors taken into account to
         * compute the current velocity for the specified agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose count of obstacle
         * neighbors is to be retrieved.</param>
         */
        public int GetAgentNumObstacleNeighbors(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].obstacleNeighbors_.Count;
        }

        /**
         * <summary>Returns the specified obstacle neighbor of the specified
         * agent.</summary>
         *
         * <returns>The number of the first vertex of the neighboring obstacle
         * edge.</returns>
         *
         * <param name="agentNo">The number of the agent whose obstacle neighbor
         * is to be retrieved.</param>
         * <param name="neighborNo">The number of the obstacle neighbor to be
         * retrieved.</param>
         */
        public int GetAgentObstacleNeighbor(int agentNo, int neighborNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].obstacleNeighbors_[neighborNo].Value.id_;
        }

        /**
         * <summary>Returns the ORCA constraints of the specified agent.
         * </summary>
         *
         * <returns>A list of lines representing the ORCA constraints.</returns>
         *
         * <param name="agentNo">The number of the agent whose ORCA constraints
         * are to be retrieved.</param>
         *
         * <remarks>The halfplane to the left of each line is the region of
         * permissible velocities with respect to that ORCA constraint.
         * </remarks>
         */
        public IList<Line> GetAgentOrcaLines(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].orcaLines_;
        }

        public bool IsAgentNo(int agentNo)
        {
            return AgentNo2IndexDict.ContainsKey(agentNo);
        }

        /**
         * <summary>Returns the two-dimensional position of a specified agent.
         * </summary>
         *
         * <returns>The present two-dimensional position of the (center of the)
         * agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * position is to be retrieved.</param>
         */
        public Vector2 GetAgentPosition(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].Position;
        }

        public float GetAgentStopDis(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].StopDis;
        }

        public void getAgentPosition_Lua(int agentNo, out float x, out float y)
        {
            var pos = GetAgentPosition(agentNo);
            x = pos.x_;
            y = pos.y_;
        }

        /**
         * <summary>Returns the two-dimensional preferred velocity of a
         * specified agent.</summary>
         *
         * <returns>The present two-dimensional preferred velocity of the agent.
         * </returns>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * preferred velocity is to be retrieved.</param>
         */
        public Vector2 GetAgentPrefVelocity(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].prefVelocity_;
        }

        /**
         * <summary>Returns the radius of a specified agent.</summary>
         *
         * <returns>The present radius of the agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose radius is to be
         * retrieved.</param>
         */
        public float GetAgentRadius(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].radius_;
        }

        /**
         * <summary>Returns the time horizon of a specified agent.</summary>
         *
         * <returns>The present time horizon of the agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose time horizon is
         * to be retrieved.</param>
         */
        public float GetAgentTimeHorizon(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].timeHorizon_;
        }

        /**
         * <summary>Returns the time horizon with respect to obstacles of a
         * specified agent.</summary>
         *
         * <returns>The present time horizon with respect to obstacles of the
         * agent.</returns>
         *
         * <param name="agentNo">The number of the agent whose time horizon with
         * respect to obstacles is to be retrieved.</param>
         */
        public float GetAgentTimeHorizonObst(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].timeHorizonObst_;
        }

        /**
         * <summary>Returns the two-dimensional linear velocity of a specified
         * agent.</summary>
         *
         * <returns>The present two-dimensional linear velocity of the agent.
         * </returns>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * linear velocity is to be retrieved.</param>
         */
        public Vector2 GetAgentVelocity(int agentNo)
        {
            return Agents[AgentNo2IndexDict[agentNo]].velocity_;
        }

        private Vector3 _cacheNormal = new Vector3();

        public Vector3 GetAgentVelocityToNormal(int agentNo)
        {
            var dir = GetAgentVelocity(agentNo);
            _cacheNormal.Set(dir.x_, 0, dir.y_);
            return _cacheNormal.normalized;
        }

        /**
         * <summary>Returns the global time of the simulation.</summary>
         *
         * <returns>The present global time of the simulation (zero initially).
         * </returns>
         */
        public float GetGlobalTime()
        {
            return _globalTime;
        }

        /**
         * <summary>Returns the count of agents in the simulation.</summary>
         *
         * <returns>The count of agents in the simulation.</returns>
         */
        public int GetNumAgents()
        {
            return Agents.Count;
        }

        /**
         * <summary>Returns the count of obstacle vertices in the simulation.
         * </summary>
         *
         * <returns>The count of obstacle vertices in the simulation.</returns>
         */
        public int GetNumObstacleVertices()
        {
            return Obstacles.Count;
        }

        /**
         * <summary>Returns the count of workers.</summary>
         *
         * <returns>The count of workers.</returns>
         */
        public int GetNumWorkers()
        {
            return _numWorkers;
        }

        /**
         * <summary>Returns the two-dimensional position of a specified obstacle
         * vertex.</summary>
         *
         * <returns>The two-dimensional position of the specified obstacle
         * vertex.</returns>
         *
         * <param name="vertexNo">The number of the obstacle vertex to be
         * retrieved.</param>
         */
        public Vector2 getObstacleVertex(int vertexNo)
        {
            return Obstacles[vertexNo].point_;
        }

        /**
         * <summary>Returns the number of the obstacle vertex succeeding the
         * specified obstacle vertex in its polygon.</summary>
         *
         * <returns>The number of the obstacle vertex succeeding the specified
         * obstacle vertex in its polygon.</returns>
         *
         * <param name="vertexNo">The number of the obstacle vertex whose
         * successor is to be retrieved.</param>
         */
        public int GetNextObstacleVertexNo(int vertexNo)
        {
            return Obstacles[vertexNo].next_.id_;
        }

        /**
         * <summary>Returns the number of the obstacle vertex preceding the
         * specified obstacle vertex in its polygon.</summary>
         *
         * <returns>The number of the obstacle vertex preceding the specified
         * obstacle vertex in its polygon.</returns>
         *
         * <param name="vertexNo">The number of the obstacle vertex whose
         * predecessor is to be retrieved.</param>
         */
        public int GetPrevObstacleVertexNo(int vertexNo)
        {
            return Obstacles[vertexNo].previous_.id_;
        }

        /**
         * <summary>Returns the time step of the simulation.</summary>
         *
         * <returns>The present time step of the simulation.</returns>
         */
        public float GetTimeStep()
        {
            return TimeStep;
        }

        /**
         * <summary>Processes the obstacles that have been added so that they
         * are accounted for in the simulation.</summary>
         *
         * <remarks>Obstacles added to the simulation after this function has
         * been called are not accounted for in the simulation.</remarks>
         */
        public void ProcessObstacles()
        {
            KdTree.buildObstacleTree(ref Obstacles);
        }

        /**
         * <summary>Performs a visibility query between the two specified points
         * with respect to the obstacles.</summary>
         *
         * <returns>A boolean specifying whether the two points are mutually
         * visible. Returns true when the obstacles have not been processed.
         * </returns>
         *
         * <param name="point1">The first point of the query.</param>
         * <param name="point2">The second point of the query.</param>
         * <param name="radius">The minimal distance between the line connecting
         * the two points and the obstacles in order for the points to be
         * mutually visible (optional). Must be non-negative.</param>
         */
        public bool QueryVisibility(Vector2 point1, Vector2 point2, float radius)
        {
            return KdTree.queryVisibility(point1, point2, radius);
        }

        public int QueryNearAgent(Vector2 point, float radius)
        {
            if (GetNumAgents() == 0)
                return -1;
            return KdTree.queryNearAgent(point, radius);
        }

        /**
         * <summary>Sets the default properties for any new agent that is added.
         * </summary>
         *
         * <param name="neighborDist">The default maximum distance (center point
         * to center point) to other agents a new agent takes into account in
         * the navigation. The larger this number, the longer he running time of
         * the simulation. If the number is too low, the simulation will not be
         * safe. Must be non-negative.</param>
         * <param name="maxNeighbors">The default maximum number of other agents
         * a new agent takes into account in the navigation. The larger this
         * number, the longer the running time of the simulation. If the number
         * is too low, the simulation will not be safe.</param>
         * <param name="timeHorizon">The default minimal amount of time for
         * which a new agent's velocities that are computed by the simulation
         * are safe with respect to other agents. The larger this number, the
         * sooner an agent will respond to the presence of other agents, but the
         * less freedom the agent has in choosing its velocities. Must be
         * positive.</param>
         * <param name="timeHorizonObst">The default minimal amount of time for
         * which a new agent's velocities that are computed by the simulation
         * are safe with respect to obstacles. The larger this number, the
         * sooner an agent will respond to the presence of obstacles, but the
         * less freedom the agent has in choosing its velocities. Must be
         * positive.</param>
         * <param name="radius">The default radius of a new agent. Must be
         * non-negative.</param>
         * <param name="maxSpeed">The default maximum speed of a new agent. Must
         * be non-negative.</param>
         * <param name="velocity">The default initial two-dimensional linear
         * velocity of a new agent.</param>
         */
        public void SetAgentDefaults(float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst,
            float radius, float maxSpeed, Vector2 velocity)
        {
            if (_defaultAgent == null)
            {
                _defaultAgent = new Agent();
            }

            _defaultAgent.maxNeighbors_ = maxNeighbors;
            _defaultAgent.maxSpeed_ = maxSpeed;
            _defaultAgent.neighborDist_ = neighborDist;
            _defaultAgent.radius_ = radius;
            _defaultAgent.timeHorizon_ = timeHorizon;
            _defaultAgent.timeHorizonObst_ = timeHorizonObst;
            _defaultAgent.velocity_ = velocity;
        }

        /**
         * <summary>Sets the maximum neighbor count of a specified agent.
         * </summary>
         *
         * <param name="agentNo">The number of the agent whose maximum neighbor
         * count is to be modified.</param>
         * <param name="maxNeighbors">The replacement maximum neighbor count.
         * </param>
         */
        public void SetAgentMaxNeighbors(int agentNo, int maxNeighbors)
        {
            Agents[AgentNo2IndexDict[agentNo]].maxNeighbors_ = maxNeighbors;
        }

        /**
         * <summary>Sets the maximum speed of a specified agent.</summary>
         *
         * <param name="agentNo">The number of the agent whose maximum speed is
         * to be modified.</param>
         * <param name="maxSpeed">The replacement maximum speed. Must be
         * non-negative.</param>
         */
        public void SetAgentMaxSpeed(int agentNo, float maxSpeed)
        {
            Agents[AgentNo2IndexDict[agentNo]].maxSpeed_ = maxSpeed;
        }

        /**
         * <summary>Sets the maximum neighbor distance of a specified agent.
         * </summary>
         *
         * <param name="agentNo">The number of the agent whose maximum neighbor
         * distance is to be modified.</param>
         * <param name="neighborDist">The replacement maximum neighbor distance.
         * Must be non-negative.</param>
         */
        public void SetAgentNeighborDist(int agentNo, float neighborDist)
        {
            Agents[AgentNo2IndexDict[agentNo]].neighborDist_ = neighborDist;
        }

        /**
         * <summary>Sets the two-dimensional position of a specified agent.
         * </summary>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * position is to be modified.</param>
         * <param name="position">The replacement of the two-dimensional
         * position.</param>
         */
        public void SetAgentPosition(int agentNo, Vector2 position)
        {
            Agents[AgentNo2IndexDict[agentNo]].Position = position;
        }

        public void setAgentPosition_Lua(int agentNo, float x, float y)
        {
            Agents[AgentNo2IndexDict[agentNo]].Position = new Vector2(x, y);
        }

        /**
         * <summary>Sets the two-dimensional preferred velocity of a specified
         * agent.</summary>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * preferred velocity is to be modified.</param>
         * <param name="prefVelocity">The replacement of the two-dimensional
         * preferred velocity.</param>
         */
        public void SetAgentPrefVelocity(int agentNo, Vector2 prefVelocity)
        {
            Agents[AgentNo2IndexDict[agentNo]].prefVelocity_ = prefVelocity;
        }

        public void setAgentPrefVelocity_Lua(int agentNo, float x, float y)
        {
            if (AgentNo2IndexDict != null && Agents[AgentNo2IndexDict[agentNo]] != null)
            {
                Agents[AgentNo2IndexDict[agentNo]].prefVelocity_ = new Vector2(x, y);
            }
        }

        public void StopAgentPrefVelocity(int agentNo)
        {
            if (AgentNo2IndexDict != null && Agents[AgentNo2IndexDict[agentNo]] != null)
            {
                Agents[AgentNo2IndexDict[agentNo]].prefVelocity_ = new Vector2(0, 0);
            }
        }

        /**
         * <summary>Sets the radius of a specified agent.</summary>
         *
         * <param name="agentNo">The number of the agent whose radius is to be
         * modified.</param>
         * <param name="radius">The replacement radius. Must be non-negative.
         * </param>
         */
        public void SetAgentRadius(int agentNo, float radius)
        {
            Agents[AgentNo2IndexDict[agentNo]].radius_ = radius;
        }

        /**
         * <summary>Sets the time horizon of a specified agent with respect to
         * other agents.</summary>
         *
         * <param name="agentNo">The number of the agent whose time horizon is
         * to be modified.</param>
         * <param name="timeHorizon">The replacement time horizon with respect
         * to other agents. Must be positive.</param>
         */
        public void SetAgentTimeHorizon(int agentNo, float timeHorizon)
        {
            Agents[AgentNo2IndexDict[agentNo]].timeHorizon_ = timeHorizon;
        }

        /**
         * <summary>Sets the time horizon of a specified agent with respect to
         * obstacles.</summary>
         *
         * <param name="agentNo">The number of the agent whose time horizon with
         * respect to obstacles is to be modified.</param>
         * <param name="timeHorizonObst">The replacement time horizon with
         * respect to obstacles. Must be positive.</param>
         */
        public void SetAgentTimeHorizonObst(int agentNo, float timeHorizonObst)
        {
            Agents[AgentNo2IndexDict[agentNo]].timeHorizonObst_ = timeHorizonObst;
        }

        /**
         * <summary>Sets the two-dimensional linear velocity of a specified
         * agent.</summary>
         *
         * <param name="agentNo">The number of the agent whose two-dimensional
         * linear velocity is to be modified.</param>
         * <param name="velocity">The replacement two-dimensional linear
         * velocity.</param>
         */
        public void SetAgentVelocity(int agentNo, Vector2 velocity)
        {
            Agents[AgentNo2IndexDict[agentNo]].velocity_ = velocity;
        }

        /**
         * <summary>Sets the global time of the simulation.</summary>
         *
         * <param name="globalTime">The global time of the simulation.</param>
         */
        public void SetGlobalTime(float globalTime)
        {
            _globalTime = globalTime;
        }

        /**
         * <summary>Sets the number of workers.</summary>
         *
         * <param name="numWorkers">The number of workers.</param>
         */
        public void SetNumWorkers(int numWorkers)
        {
            _numWorkers = numWorkers;

            if (_numWorkers <= 0)
            {
                int completionPorts;
                // ThreadPool.GetMinThreads(out _numWorkers, out completionPorts);
                _numWorkers = 1;
            }

            _workers = null;
            _workerAgentCount = 0;
        }

        /**
         * <summary>Sets the time step of the simulation.</summary>
         *
         * <param name="timeStep">The time step of the simulation. Must be
         * positive.</param>
         */
        public void SetTimeStep(float timeStep)
        {
            TimeStep = timeStep;
        }

        public void SetTargetId(int agentNo, int targetId, bool isFollow = false, bool isFirst = false)
        {
            var agent = Agents[AgentNo2IndexDict[agentNo]];
            agent.SetTargetId(targetId, isFollow, isFirst);
        }

        /**
         * <summary>Constructs and initializes a simulation.</summary>
         */
        public Simulator()
        {
            Clear();
        }

        private void StopWorkers()
        {
            // 遍历所有的 Worker 并请求取消
            foreach (var worker in _workers)
            {
                worker.Cancel();
            }

            // 等待所有工作线程完成
            foreach (var doneEvent in _doneEvents)
            {
                doneEvent.WaitOne(); // 等待工作完成
            }
        }

        // 清理工作线程
        private void ReleaseWorkers()
        {
            // 停止所有线程
            StopWorkers();

            // 释放工作线程的资源
            foreach (var worker in _workers)
            {
                worker.Dispose(); // 释放资源
            }

            _workers = null; // 清空 Worker 列表
            _doneEvents = null; // 清空 ManualResetEvent 数组
        }

        public void Release()
        {
            ReleaseWorkers();
            // 清除所有代理
            if (Agents != null)
            {
                Agents.Clear(); // 清空代理列表
            }

            if (DeadAgents != null)
            {
                DeadAgents.Clear(); // 清空代理列表
            }


            // 清除代理字典
            if (AgentNo2IndexDict != null)
            {
                AgentNo2IndexDict.Clear();
            }

            if (Index2AgentNoDict != null)
            {
                Index2AgentNoDict.Clear();
            }

            if (DeadAgentNo2IndexDict != null)
            {
                DeadAgentNo2IndexDict.Clear();
            }

            if (DeadIndex2AgentNoDict != null)
            {
                DeadIndex2AgentNoDict.Clear();
            }

            // 清除障碍物
            if (Obstacles != null)
            {
                Obstacles.Clear(); // 清空障碍物列表
            }

            // 释放KdTree资源
            if (KdTree != null)
            {
                KdTree = null; // 假设KdTree是可以简单清空的对象
            }

            if (DeadKdTree != null)
            {
                DeadKdTree = null; // 假设KdTree是可以简单清空的对象
            }

            if (_doneEvents != null)
            {
                foreach (var doneEvent in _doneEvents)
                {
                    doneEvent.Dispose(); // 释放 ManualResetEvent 资源
                }

                _doneEvents = null;
            }

            // 清除其他可能占用内存的成员
            _defaultAgent = null;
            _globalTime = 0.0f;
            TimeStep = 0.1f;
        }

        public List<int> GetAgentIdsInRange(int agentId, float radius, bool isVirtual = false)
        {
            if (!IsAgentValid(agentId)) return null;
            var agent = Agents[AgentNo2IndexDict[agentId]];
            if (agent == null)
                return null;
            var pos = agent.Position;
            return KdTree.GetAgentsInRange(pos, radius, 0, isVirtual);
        }

        public List<int> GetAgentsInCircle(float x, float y, float radius, int side = 0, bool isVirtual = false,
            bool containDead = false)
        {
            if (isVirtual)
            {
                return VirtualHeroKdTree.GetAgentsInRange(x, y, radius, side, isVirtual);
            }

            if (!containDead) return KdTree.GetAgentsInRange(x, y, radius, side, isVirtual);
            var aliveAgents = KdTree.GetAgentsInRange(x, y, radius, side, isVirtual);
            var deadAgents = DeadKdTree.GetAgentsInRange(x, y, radius, side, isVirtual);
            foreach (var agent in deadAgents)
            {
                if (!aliveAgents.Contains(agent))
                {
                    aliveAgents.Add(agent);
                }
            }

            return aliveAgents;
        }

        public List<int> GetAgentsInRotatedRect(float x, float y, float width, float height, float angleRad,
            int side = 0,
            bool isVirtual = false,
            bool containDead = false)
        {
            if (isVirtual)
            {
                return VirtualHeroKdTree.GetRotatedRectFromLeftCenter(x, y, width, height, angleRad, side, isVirtual);
            }

            if (!containDead)
                return KdTree.GetRotatedRectFromLeftCenter(x, y, width, height, angleRad, side, isVirtual);
            var aliveAgents = KdTree.GetRotatedRectFromLeftCenter(x, y, width, height, angleRad, side, isVirtual);
            var deadAgents = DeadKdTree.GetRotatedRectFromLeftCenter(x, y, width, height, angleRad, side, isVirtual);
            foreach (var agent in deadAgents)
            {
                if (!aliveAgents.Contains(agent))
                {
                    aliveAgents.Add(agent);
                }
            }

            return aliveAgents;
        }

        public List<int> GetAgentsInRotatedSector(float x, float y, float radius, float angle, float angleRad,
            int side = 0,
            bool isVirtual = false,
            bool containDead = false)
        {
            if (isVirtual)
            {
                return VirtualHeroKdTree.GetAgentsInRotatedSectorRange(x, y, radius, angle, angleRad, side, isVirtual);
            }

            if (!containDead)
                return KdTree.GetAgentsInRotatedSectorRange(x, y, radius, angle, angleRad, side, isVirtual);
            var aliveAgents = KdTree.GetAgentsInRotatedSectorRange(x, y, radius, angle, angleRad, side, isVirtual);
            var deadAgents = DeadKdTree.GetAgentsInRotatedSectorRange(x, y, radius, angle, angleRad, side, isVirtual);
            foreach (var agent in deadAgents)
            {
                if (!aliveAgents.Contains(agent))
                {
                    aliveAgents.Add(agent);
                }
            }

            return aliveAgents;
        }

        public void SetAttackRange(int agentId, float attackRange)
        {
            if (!IsAgentValid(agentId)) return;
            var agent = Agents[AgentNo2IndexDict[agentId]];
            if (agent == null)
                return;
            agent.SetAttackRange(attackRange);
        }

        public void SetStopDis(int agentId, float stopDis)
        {
            if (!IsAgentValid(agentId)) return;
            var agent = Agents[AgentNo2IndexDict[agentId]];
            if (agent == null)
                return;
            agent.StopDis = stopDis;
        }

        public void SetAgentTargetPosition(int agentId, float x, float y)
        {
            if (!IsAgentValid(agentId)) return;
            var agent = (SoldierAgent)Agents[AgentNo2IndexDict[agentId]];
            if (agent == null)
                return;
            agent.SetTargetPos(x, y);
        }

        public SoldierAgent GetAgent(int agentNo)
        {
            if (AgentNo2IndexDict.ContainsKey(agentNo))
            {
                return (SoldierAgent)Agents[AgentNo2IndexDict[agentNo]];
            }

            return null;
        }

        public bool IsAgentValid(int agentNo)
        {
            return AgentNo2IndexDict.ContainsKey(agentNo);
        }

        public void SetAIType(int agentId, int aiType)
        {
            SoldierAgent agent = GetAgent(agentId);
            if (agent == null) return;
            agent.AiType = aiType;
        }

        public int IsAgentEncircle(int agentId, float attackRange)
        {
            SoldierAgent agent = GetAgent(agentId);
            if (agent == null) return 0;
            return agent.IsEncircled(attackRange);
        }

        public int NeedChangeNearestAgent(int agentId, int targetId)
        {
            SoldierAgent agent = GetAgent(agentId);
            if (agent == null) return 0;
            return agent.NeedChangeNearestAgent(targetId);
        }
    }
}