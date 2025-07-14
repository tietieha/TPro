using System;
using FixPoint;

namespace M.PathFinding
{
    public partial class BattleMap
    {
        //线段与地图求交。遇到不可走的grid即停下
        public bool Raycast(EUnitType layerIndex, ELayerType layerType, FixInt2 startWorld, FixInt2 endWorld, ref Integer2 interGrid)
        {
            FixInt2 startPosInGrid;
            FixInt2 endPosInGrid;
            startPosInGrid = (startWorld - _originPos); // 减少计算，不进行格子除法
            endPosInGrid = (endWorld - _originPos);

            //Utils.Assert(startPosInGrid.x >= 0 && startPosInGrid.x < _width * FixInt2.Scale);
            //Utils.Assert(startPosInGrid.y >= 0 && startPosInGrid.y < _height * FixInt2.Scale);
            //Utils.Assert(endPosInGrid.x >= 0 && endPosInGrid.x < _width * FixInt2.Scale);
            //Utils.Assert(endPosInGrid.y >= 0 && endPosInGrid.y < _height * FixInt2.Scale);

            int x0 = startPosInGrid.x;
            int y0 = startPosInGrid.y;
            int x1 = endPosInGrid.x;
            int y1 = endPosInGrid.y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int n = 1;
            int x_inc = 0;
            int y_inc = 0;
            double error = 0.0f;

            int lastCheckedGridX = -1;
            int lastCheckedGridY = -1;

            if (dx == 0)
            {
                x_inc = 0;
                error = Double.PositiveInfinity; //std;::numeric_limits<double>::infinity();正无穷
            }
            else if (x1 > x0)
            {
                x_inc = 1;
                n += x1 / _gridSize - x0 / _gridSize;
                error = (_gridSize - (x0%_gridSize)) * dy / _gridSize;
            }
            else
            {
                x_inc = -1;
                n += x0/_gridSize - x1/_gridSize;
                error = (x0 % _gridSize) * dy / _gridSize;
            }

            if (dy == 0)
            {
                y_inc = 0;
                error -= Double.PositiveInfinity;
                //std;::numeric_limits<double>::infinity();
            }
            else if (y1 > y0)
            {
                y_inc = 1;
                n += y1 / _gridSize - y0/_gridSize;
                error -= (_gridSize - (y0 % _gridSize)) * dx / _gridSize;
            }
            else
            {
                y_inc = -1;
                n += y0 / _gridSize - y1 / _gridSize;
                error -= (y0 % _gridSize) * dx / _gridSize;
            }

            Integer2 startGrid = WorldPosToGrid(startWorld);
            int x = startGrid.x;
            int y = startGrid.y;
            for (; n > 0; --n)
            {
                //visit(x, y);
                //result.push([x, y]);
                if (lastCheckedGridX != -1)
                {
                    int gridDis = (Math.Abs(x - lastCheckedGridX) + Math.Abs(y - lastCheckedGridY));
                    Utils.Assert(gridDis <= 2);

                    if (gridDis == 2)
                    {
                        Node n1 = GetNode(x, lastCheckedGridY);
                        if (n1.IsOccupiedOrLayer(layerIndex, layerType))
                        {
                            interGrid = n1.GetPos();
                            return true;
                        }

                        Node n2 = GetNode(lastCheckedGridX, y);
                        if (n2.IsOccupiedOrLayer(layerIndex, layerType))
                        {
                            interGrid = n2.GetPos();
                            return true;
                        }
                    }

                }

                Node node = GetNode(x, y);
                if (node == null)
                {
                    return true;
                }
                if (node.IsOccupiedOrLayer(layerIndex, layerType))
                {
                    interGrid = node.GetPos();
                    return true;
                }

                lastCheckedGridX = x;
                lastCheckedGridY = y;
                
                if (error > 0)
                {
                    y += y_inc;
                    error -= dx;
                }
                else
                {
                    x += x_inc;
                    error += dy;
                }


            }
            return false;
        }
        public bool RaycastObstacled(EUnitState state, Integer2 startPosInGrid, Integer2 endPosInGrid, ref Integer2 interGrid, ref Integer2 openGrid)
        {
            int x0 = startPosInGrid.x;
            int y0 = startPosInGrid.y;
            int x1 = endPosInGrid.x;
            int y1 = endPosInGrid.y;

            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);

            int n = 1;
            int x_inc = 0;
            int y_inc = 0;
            double error = 0.0f;

            int lastCheckedGridX = -1;
            int lastCheckedGridY = -1;

            if (dx == 0)
            {
                x_inc = 0;
                error = Double.PositiveInfinity; //std;::numeric_limits<double>::infinity();正无穷
            }
            else if (x1 > x0)
            {
                x_inc = 1;
                n = dx;
                error = dy / _gridSize;
            }
            else
            {
                x_inc = -1;
                n = dx;
                error = dy / _gridSize;
            }

            if (dy == 0)
            {
                y_inc = 0;
                error -= Double.PositiveInfinity;
                //std;::numeric_limits<double>::infinity();
            }
            else if (y1 > y0)
            {
                y_inc = 1;
                n += dy;
                error -= dx / _gridSize;
            }
            else
            {
                y_inc = -1;
                n += dy;
                error -= dx / _gridSize;
            }

            int x = startPosInGrid.x;
            int y = startPosInGrid.y;
            for (; n > 0; --n)
            {
                //visit(x, y);
                //result.push([x, y]);
                if (lastCheckedGridX != -1)
                {
                    int gridDis = (Math.Abs(x - lastCheckedGridX) + Math.Abs(y - lastCheckedGridY));
                    Utils.Assert(gridDis <= 2);

                    if (gridDis == 2)
                    {
                        Node n1 = GetNode(x, lastCheckedGridY);
                        if (!n1.IsCanMove(state))
                        {
                            openGrid.x = lastCheckedGridX;
                            openGrid.y = lastCheckedGridY!=-1 ? lastCheckedGridY : startPosInGrid.y;
                            interGrid = n1.GetPos();
                            return true;
                        }

                        Node n2 = GetNode(lastCheckedGridX, y);
                        if (!n2.IsCanMove(state))
                        {
                            openGrid.x = lastCheckedGridX;
                            openGrid.y = lastCheckedGridY != -1 ? lastCheckedGridY : startPosInGrid.y;
                            interGrid = n2.GetPos();
                            return true;
                        }
                    }

                }

                Node node = GetNode(x, y);
                if (node == null)
                {
                    openGrid.x = lastCheckedGridX != -1 ? lastCheckedGridX : startPosInGrid.x;
                    openGrid.y = lastCheckedGridY != -1 ? lastCheckedGridY : startPosInGrid.y;
                    return true;
                }
                if (!node.IsCanMove(state))
                {
                    openGrid.x = lastCheckedGridX != -1 ? lastCheckedGridX : startPosInGrid.x;
                    openGrid.y = lastCheckedGridY != -1 ? lastCheckedGridY : startPosInGrid.y;
                    interGrid = node.GetPos();
                    return true;
                }

                lastCheckedGridX = x;
                lastCheckedGridY = y;

                if (error > 0)
                {
                    y += y_inc;
                    error -= dx;
                }
                else
                {
                    x += x_inc;
                    error += dy;
                }
            }
            return false;
        }
    }

   
}

/*

    //查询线段与grid的相交的点.支持输入浮点数
    export function raytraceGrid_float(x0: number, y0: number, x1: number, y1: number): [number, number][]
    {
        let result: [number, number][] = [];

        let dx = Math.abs(x1 - x0);
        let dy = Math.abs(y1 - y0);

        let x = Math.floor(x0);
        let y = Math.floor(y0);

        let n = 1;
        let x_inc, y_inc;
        let error;

        if(dx === 0)
        {
            x_inc = 0;
            error = Infinity;          //std;::numeric_limits<double>::infinity();
        }
        else if(x1 > x0)
        {
            x_inc = 1;
            n += Math.floor(x1) - x;
            error = (Math.floor(x0) + 1 - x0) * dy;
        }
        else
        {
            x_inc = -1;
            n += x - (Math.floor(x1));
            error = (x0 - Math.floor(x0)) * dy;
        }

        if(dy === 0)
        {
            y_inc = 0;
            error -= Infinity;
            //std;::numeric_limits<double>::infinity();
        }
        else if(y1 > y0)
        {
            y_inc = 1;
            n += (Math.floor(y1)) - y;
            error -= (Math.floor(y0) + 1 - y0) * dx;
        }
        else
        {
            y_inc = -1;
            n += y - (Math.floor(y1));
            error -= (y0 - Math.floor(y0)) * dx;
        }

        for(; n > 0; --n)
        {
            //visit(x, y);
            result.push([x, y]);

            if(error > 0)
            {
                y += y_inc;
                error -= dx;
            }
            else
            {
                x += x_inc;
                error += dy;
            }
        }
        return result;
    }
 */