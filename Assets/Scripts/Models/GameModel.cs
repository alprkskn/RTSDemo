using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class GameModel : ModelBase
    {
        private Vector2 _mapSize;
        private List<BuildingModel> _buildings;
        private List<UnitModel> _units;

        public Vector2 MapSize
        {
            get { return _mapSize; }
            set
            {
                _mapSize = value;
                NotifyPropertyChange("MapSize", value);
            }
        }

        public List<BuildingModel> Buildings
        {
            get { return _buildings; }
            set
            {
                _buildings = value;
                NotifyPropertyChange("Buildings", value);
            }
        }

        public List<UnitModel> Units
        {
            get { return _units; }
            set
            {
                _units = value;
                NotifyPropertyChange("Units", value);
            }
        }
    }
}
