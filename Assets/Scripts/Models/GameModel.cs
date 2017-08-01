using System;
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
        private List<Type> _availableBuildingTypes;

        private List<ISelectable> _selectables = new List<ISelectable>();

        #region PropertyListenerMethods

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

        public List<Type> AvailableBuildingTypes
        {
            get { return _availableBuildingTypes; }
            set
            {
                _availableBuildingTypes = value;
                NotifyPropertyChange("AvailableBuildingTypes", value);
            }
        }

        public void AddUnit(UnitModel unit)
        {
            _units.Add(unit);
            _selectables.Add(unit);
            NotifyCollectionModification("Units", unit, CollectionModification.Add);
        }

        public void RemoveUnit(UnitModel unit)
        {
            if (_units.Remove(unit))
            {
                _selectables.Remove(unit);
                NotifyCollectionModification("Units", unit, CollectionModification.Remove);
            }
        }

        public void ClearUnits()
        {
            _selectables.RemoveAll((x) => _units.Contains((UnitModel) x));
            _units.Clear();
            NotifyCollectionModification("Units", null, CollectionModification.Clear);
        }

        public void AddBuilding(BuildingModel building)
        {
            _buildings.Add(building);
            _selectables.Add(building);
            NotifyCollectionModification("Buildings", building, CollectionModification.Add);
        }

        public void RemoveBuilding(BuildingModel building)
        {
            if (_buildings.Remove(building))
            {
                _selectables.Remove(building);
                NotifyCollectionModification("Buildings", building, CollectionModification.Remove);
            }
        }

        public void ClearBuildings()
        {
            _selectables.RemoveAll((x) => _buildings.Contains((BuildingModel) x));
            _buildings.Clear();
            NotifyCollectionModification("Buildings", null, CollectionModification.Clear);
        }

        public void AddAvailableBuildingType<T> ()  where T : BuildingModel
        {
            _availableBuildingTypes.Add(typeof(T));
            NotifyCollectionModification("AvailableBuildingTypes", typeof(T), CollectionModification.Add);
        }

        public void RemoveAvailableBuildingType<T> () where T : BuildingModel
        {
            if (_availableBuildingTypes.Remove(typeof(T)))
            {
                NotifyCollectionModification("AvailableBuildingTypes", typeof(T), CollectionModification.Remove);
            }
        }

        public void ClearAvailableBuildingTypes()
        {
            NotifyCollectionModification("AvailableBuildingTypes", null, CollectionModification.Clear);
        }

        #endregion

        #region StandardProperties

        public List<ISelectable> Selectables
        {
            get { return _selectables; }
        }

        #endregion
    }
}
