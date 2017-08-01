using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class BarracksModel : BuildingModel
    {
        public override bool HasProduction()
        {
            return true;
        }

        public override List<Type> GetProductList()
        {
            return new List<Type>(1) {typeof(SoldierModel)};
        }
    }
}