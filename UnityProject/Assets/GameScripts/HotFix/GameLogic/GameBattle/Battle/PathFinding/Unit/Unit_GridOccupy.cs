using System.Collections.Generic;

namespace M.PathFinding
{
    public partial class Unit
    {
        //当前是否占据了格子
        private bool _isOccupyGrid = false;
        //当前占据的格子的中心坐标
        private Integer2 _occupiedGrid = new Integer2(0, 0);

        //获取本单位在某一层上的真实Extend
        public int GetRealExtendOnLayer(EUnitType t)
        {
            int result = Extend_Modifier_Array2D[(int)t, (int)this._unitType];
            result += GetRealExtend();
            if (result < 0)
                result = -1;

            return result;
        }

        //当前单位是否占据格子
        public bool IsOccupiedGird()
        {
            return _isOccupyGrid;
        }

        //获取当前占据的格子
        public Integer2 GetOccupiedGrid()
        {
            if (!_isOccupyGrid)
                return new Integer2(-1, -1);
            return _occupiedGrid;
        }

        //移除当前格子的占用
        public void Internal_RemoveCurrentGridOccupy()
        {
            if (_isOccupyGrid)
            {
                // if (!IsHero())
                // {
                //     var node = _map.GetNode(this._occupiedGrid.x, this._occupiedGrid.y);
                //     node.UnitOnMe.Remove(this);
                //     //SoldierUnitOnMe = null;
                // }

                _map.Internal_RemoveUnitOccupy_GridPos(this, this._occupiedGrid.x, this._occupiedGrid.y, EUnitType.All);
                _isOccupyGrid = false;
                _occupiedGrid.x = -1;
                _occupiedGrid.y = -1;
            }

        }

        //更改当前占据的格子
        public void Internal_ChangeOccupyGrid(int x, int y)
        {
            if (!_isOccupyGrid)
            {
                // if (!IsHero())
                // {
                //     var node = _map.GetNode(x, y);
                //     //node.SoldierUnitOnMe = this;
                //     node.UnitOnMe.Add(this);
                // }

                _map.Internal_AddUnitOccupy_GridPos(this, x, y, EUnitType.All);
                _isOccupyGrid = true;
                _occupiedGrid.x = x;
                _occupiedGrid.y = y;

                //如果是英雄，变更小兵的位置
                // if (IsHero())
                // {
                //     Internal_UpdateSoldierPositionNearHero();
                // }

            }
            else
            {
                if (_occupiedGrid.x == x && _occupiedGrid.y == y)
                    return;

                Internal_RemoveCurrentGridOccupy();

                // if (!IsHero())
                // {
                //     var node = _map.GetNode(x, y);
                //     //node.SoldierUnitOnMe = this;
                //     node.UnitOnMe.Add(this);
                // }

                _map.Internal_AddUnitOccupy_GridPos(this, x, y, EUnitType.All);
                _isOccupyGrid = true;
                _occupiedGrid.x = x;
                _occupiedGrid.y = y;

                //如果是英雄，变更小兵的位置
                // if (IsHero())
                // {
                //     Internal_UpdateSoldierPositionNearHero();
                // }
            }

        }
        //确保当前站在空格子上。会改变Unit的位置！
        public void InternalEnableOccupy_Stand()
        {
            Internal_RemoveCurrentGridOccupy();
            if (!_isOccupyGrid)
            {
                var p = _map.FindNearestNotOccupiedNode(_gridPos.x, _gridPos.y, _unitType);
                if (p == _gridPos)
                {;
                }
                else
                {
                    WorldPos = _map.GetGridCenter(p.x, p.y);
                    SetGridPos(p);
                }

                Internal_ChangeOccupyGrid(_gridPos.x, _gridPos.y);
            }
        }

        //临时移除当前格子的占用
        public void Internal_RemoveCurrentGridOccupy_Temp(EUnitType unitType)
        {
            _map.Internal_RemoveUnitOccupy_GridPos(this, this._occupiedGrid.x, this._occupiedGrid.y, unitType);
        }

        //临时更改当前占据的格子
        public void Internal_AddCurrentGridOccupy_Temp(EUnitType unitType)
        {
            _map.Internal_AddUnitOccupy_GridPos(this, this._occupiedGrid.x, this._occupiedGrid.y, unitType);
        }

        public bool IsCurrStateCanMove(ELayerType layerType)
        {
            return StateLayoutArray[(int)UnitState, (int)layerType] > 0;
        }
        //private List<Unit> tempUnitList = new List<Unit>(20);
        
    }
}