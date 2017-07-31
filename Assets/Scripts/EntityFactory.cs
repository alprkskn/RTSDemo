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

        public static BuildingModel CreateBuilding()
        {
            var model = new BuildingModel();
            ViewFactory.CreateViewForModel<BuildingView>(model);
            return model;
        }

        public static UnitModel CreateUnit()
        {
            var model = new UnitModel();
            ViewFactory.CreateViewForModel<UnitView>(model);
            return model;
        }
    }
}