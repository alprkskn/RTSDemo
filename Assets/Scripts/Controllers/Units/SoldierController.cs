using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class SoldierController : UnitController
    {
        public override void InitModelView(ModelBase model, ViewBase view)
        {
            base.InitModelView(model, view);

            SoldierView soldierView = (SoldierView) view;

            soldierView.ArrivedToWaypoint += OnArrivedToWaypoint;
        }

        private void OnArrivedToWaypoint(SoldierView sender, int coordX, int coordY)
        {
            SoldierModel model = (SoldierModel)_viewToModel[sender];

            GridManager.Instance.UpdateMap(model.CoordX, model.CoordY, 1, 1, 0);

            model.CoordX = coordX;
            model.CoordY = coordY;

            GridManager.Instance.UpdateMap(coordX, coordY, 1, 1, GridLayers.Units);

            model.RemovePath(new Vector2(coordX, coordY));
        }
    }
}