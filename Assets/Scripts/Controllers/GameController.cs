using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class GameController : ControllerBase
    {
        #region PrivateFields

        private ISelectable _currentSelection = null;
        private BuildingModel _buildingPlacement = null;

        #endregion
        public override void InitModelView(ModelBase model, ViewBase view)
        {
            base.InitModelView(model, view);

            GameView gameView = (GameView) view;

            gameView.BoardClickRegistered += OnBoardClickRegistered;
            gameView.BuildingProductionStart += OnBuildingProductionStart;
        }

        private void OnBuildingProductionStart(GameView sender, Type buildingType)
        {
            // This means there is an unfinished placement going.
            // It will be terminated.
            if (_buildingPlacement != null)
            {
                BuildingController.Instance.DestroyModel(_buildingPlacement);
                _buildingPlacement = null;
            }

            BuildingModel model = null;

            // Going for a pretty non-generic path. Sorry, but have to finish this.
            if (buildingType == typeof(BarracksModel))
            {
                model = EntityFactory.CreateBarracks(sender);
            }
            else if (buildingType == typeof(PowerPlantModel))
            {
                model = EntityFactory.CreatePowerPlant(sender);
            }

            _buildingPlacement = model;

            sender.BoardHoverRegistered += OnBoardHoverRegistered;
        }

        private void OnBoardHoverRegistered(GameView sender, int coordX, int coordY)
        {
            _buildingPlacement.CoordX = coordX;
            _buildingPlacement.CoordY = coordX;
        }

        private void OnBoardClickRegistered(GameView sender, int coordX, int coordY, UnityEngine.EventSystems.PointerEventData.InputButton btn)
        {
            var model = (GameModel) this._viewToModel[sender];
            // There is an undergoing placement.
            // This will not select units.
            if (_buildingPlacement != null)
            {
                var available = GridManager.Instance.CheckOverlap(coordX, coordY, _buildingPlacement.Width, _buildingPlacement.Height,
                    GridLayers.Buildings | GridLayers.Units);

                if (available)
                {
                    _buildingPlacement.Placed = true;
                    _buildingPlacement = null;
                    sender.BoardHoverRegistered -= OnBoardHoverRegistered;
                }
            }
            else
            {
                foreach (var selectable in model.Selectables)
                {
                    // Check given coords with Rectangles of selectables.
                    if (selectable.CheckOverlap(coordX, coordY))
                    {
                        if (_currentSelection != null)
                        {
                            _currentSelection.OnDeSelection();
                        }

                        selectable.OnSelection();
                        _currentSelection = selectable;
                        break;
                    }
                }
            }
        }

    }
}