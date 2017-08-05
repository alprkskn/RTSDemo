using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class ProductionBuildingModel : BuildingModel
    {
        private List<Type> _productionList;
        private Vector2? _rallyPoint;

        public List<Type> ProductionList
        {
            get { return _productionList; }
            set
            {
                _productionList = value;
                NotifyPropertyChange("ProductionList", value);
            }
        }

        public Vector2? RallyPoint
        {
            get { return _rallyPoint; }
            set
            {
                _rallyPoint = value;
                NotifyPropertyChange("RallyPoint", value);
            }
        }

        public override bool HasProduction()
        {
            return true;
        }

        public override List<Type> GetProductList()
        {
            return ProductionList;
        }
    }
}