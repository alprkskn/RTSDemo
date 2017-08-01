using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    /// <summary>
    /// This factory class is responsible for creation of both 
    /// Model and Views of the grid entities.
    /// </summary>
    public static class EntityFactory
    {
        // TODO: These methods are placeholders.
        // These will most probably be changed with 
        // derived models tomorrow.

        public static BarracksModel CreateBarracks()
        {
            var model = new BarracksModel();
            ViewFactory.CreateViewForModel<BuildingView>(model);
            return model;
        }

        public static SoldierModel CreateUnit()
        {
            var model = new SoldierModel();
            ViewFactory.CreateViewForModel<UnitView>(model);
            return model;
        }
    }
}