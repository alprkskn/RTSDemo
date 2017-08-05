using System;
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
            var view = ViewFactory.CreateViewForModel<BarracksView>(model);
            parentView.AddToMap(view);

            model.Name = "Barracks";
            model.Placed = false;
            model.Width = (int)GameConstants.BarracksSize.x;
            model.Height = (int)GameConstants.BarracksSize.y;
            model.MapImage = ResourcesManager.Instance.GetSprite("BarracksMap");
            model.UiImage = ResourcesManager.Instance.GetSprite("Barracks");
            model.ProductionList = new List<Type>() { typeof(SoldierModel) };

            return model;
        }

        public static PowerPlantModel CreatePowerPlant(GameView parentView)
        {
            var model = new PowerPlantModel();
            var view = ViewFactory.CreateViewForModel<PowerPlantView>(model);
            parentView.AddToMap(view);

            model.Name = "PowerPlant";
            model.Placed = false;
            model.Width = (int)GameConstants.PowerPlantSize.x;
            model.Height = (int)GameConstants.PowerPlantSize.y;
            model.MapImage = ResourcesManager.Instance.GetSprite("PowerPlantMap");
            model.UiImage = ResourcesManager.Instance.GetSprite("PowerPlant");

            return model;
        }

        public static SoldierModel CreateSoldier(GameView parentView)
        {
            var model = new SoldierModel();
            var view = ViewFactory.CreateViewForModel<SoldierView>(model);
            parentView.AddToMap(view);

            model.Name = "Soldier";
            model.MovementRate = 0.5f;

            return model;
        }
    }
}