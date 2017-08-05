using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class ProductionBuildingView : BuildingView
    {
        [SerializeField]
        private RectTransform _rallyPointPrefab;

        private RectTransform _rallyPointObject;

        protected virtual void ProductionListChanged(object sender, List<Type> value)
        {

        }

        protected virtual void RallyPointChanged(object model, Vector2? value)
        {
            Debug.LogError("RallyPointChanged " + value.ToString());
            ProductionBuildingModel m = (ProductionBuildingModel)model;

            if (m.Highlighted)
            {
                CreateRallyPointObject(m);
            }
        }

        protected override void HighlightedChanged(object model, bool highlighted)
        {
            base.HighlightedChanged(model, highlighted);

            ProductionBuildingModel m = (ProductionBuildingModel)model;

            if (m.RallyPoint.HasValue)
            {
                if (highlighted)
                {
                    CreateRallyPointObject(m);
                }
                else
                {
                    DestroyRallyPointObject();
                }
            }
        }


        private void CreateRallyPointObject(ProductionBuildingModel model)
        {
            // Clean the previous one if it exists
            DestroyRallyPointObject();
            if (model.RallyPoint.HasValue)
            {
                _rallyPointObject = Instantiate(_rallyPointPrefab);
                _rallyPointObject.SetParent(_rectTransform.parent, false);

                var pos = _rectTransform.anchoredPosition;
                pos.x += (model.RallyPoint.Value.x - model.CoordX) * GameConstants.CellSize;

                // Subtract on the y axis to move correctly on UI space.
                pos.y -= (model.RallyPoint.Value.y - model.CoordY) * GameConstants.CellSize;

                _rallyPointObject.anchoredPosition = pos;
            }
        }

        private void DestroyRallyPointObject()
        {
            if (_rallyPointObject != null)
            {
                Destroy(_rallyPointObject.gameObject);
                _rallyPointObject = null;
            }
        }
    }
}