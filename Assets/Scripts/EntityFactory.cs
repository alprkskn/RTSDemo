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
        public static BarracksModel CreateBarracks(GameView parentView)
        {
            var model = new BarracksModel();
            var view = ViewFactory.CreateViewForModel<BuildingView>(model);
            view.GetComponent<Transform>().SetParent(parentView.transform, false);

            model.Placed = false;
            model.Width = (int) GameConstants.BarracksSize.x;
            model.Height = (int) GameConstants.BarracksSize.y;

            return model;
        }

        public static PowerPlantModel CreatePowerPlant(GameView parentView)
        {
            var model = new PowerPlantModel();
            var view = ViewFactory.CreateViewForModel<BuildingView>(model);
            view.GetComponent<Transform>().SetParent(parentView.transform, false);

            model.Placed = false;
            model.Width = (int) GameConstants.PowerPlantSize.x;
            model.Height = (int) GameConstants.PowerPlantSize.y;

            return model;
        }

        public static SoldierModel CreateSoldier(GameView parentView)
        {
            var model = new SoldierModel();
            var view = ViewFactory.CreateViewForModel<UnitView>(model);
            view.GetComponent<Transform>().SetParent(parentView.transform, false);

            return model;
        }
    }
}