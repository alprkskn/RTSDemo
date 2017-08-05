using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

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
            gameView.BuildingConstructionStart += OnBuildingConstructionStart;
            gameView.UnitProduced += OnUnitProduced;
        }

        private void OnUnitProduced(GameView sender, ProductionBuildingModel building, Type producttype)
        {
            var model = (GameModel) this._viewToModel[sender];


            var initialPlace = GridManager.Instance.GetAvailableSlotAroundRect(building.CoordX, building.CoordY, building.Width,
                building.Height, 
                (building.RallyPoint.HasValue) ? (int) building.RallyPoint.Value.x : building.CoordX, 
                (building.RallyPoint.HasValue) ? (int) building.RallyPoint.Value.y : building.CoordY);

            if (initialPlace.HasValue)
            {
                var soldier = EntityFactory.CreateSoldier(sender);
                soldier.CoordX = (int) initialPlace.Value.x;
                soldier.CoordY = (int) initialPlace.Value.y;

                Debug.LogError(initialPlace);

                if (building.RallyPoint.HasValue)
                {
                    var path = GridManager.Instance.FindPath(soldier.CoordX, soldier.CoordY,
                        (int) building.RallyPoint.Value.x,
                        (int) building.RallyPoint.Value.y);

                    if (path != null)
                    {
                        soldier.Path = path;
                    }
                    else
                    {
                        Debug.LogError("Could not find a path.");
                    }
                }
                model.AddUnit(soldier);
            }
            else
            {
                Debug.LogError("There is no available spot for a soldier around the building.");
            }
        }

        private void OnBuildingConstructionStart(GameView sender, Type buildingType)
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
            _buildingPlacement.CoordY = coordY;

            var available = !GridManager.Instance.CheckOverlap(coordX, coordY, _buildingPlacement.Width, _buildingPlacement.Height,
                GridLayers.Buildings | GridLayers.Units);

            _buildingPlacement.PlacementAvailable = available;
        }

        private void OnBoardClickRegistered(GameView sender, int coordX, int coordY, UnityEngine.EventSystems.PointerEventData.InputButton btn)
        {
            var model = (GameModel) this._viewToModel[sender];

            if (btn == PointerEventData.InputButton.Left)
            {
                // There is an undergoing placement.
                // This will not select units.
                if (_buildingPlacement != null)
                {
                    var available = !GridManager.Instance.CheckOverlap(coordX, coordY, _buildingPlacement.Width,
                        _buildingPlacement.Height,
                        GridLayers.Buildings | GridLayers.Units);

                    if (available)
                    {
                        model.AddBuilding(_buildingPlacement);
                        _buildingPlacement.Placed = true;
                        _buildingPlacement = null;
                        sender.BoardHoverRegistered -= OnBoardHoverRegistered;
                    }
                }
                else
                {
                    bool hit = false;
                    foreach (var selectable in model.Selectables)
                    {
                        // Check given coords with Rectangles of selectables.
                        if (selectable.CheckOverlap(coordX, coordY))
                        {
                            hit = true;
                            if (_currentSelection != null)
                            {
                                _currentSelection.OnDeSelection();
                            }

                            selectable.OnSelection();
                            _currentSelection = selectable;
                            model.SelectedEntity = selectable;
                            break;
                        }
                    }

                    if (_currentSelection != null && !hit)
                    {
                        _currentSelection.OnDeSelection();
                        _currentSelection = null;
                        model.SelectedEntity = null;
                    }
                }
            }
            else if (btn == PointerEventData.InputButton.Right)
            {
                if (_currentSelection != null)
                {
                    // Too tired to think something anything near engineering at this point.
                    // Just getting things done.
                    // We have only to entities that get right click commands.
                    if (_currentSelection is SoldierModel)
                    {
                        SoldierModel soldier = (SoldierModel) _currentSelection;
                        var path = GridManager.Instance.FindPath(soldier.CoordX, soldier.CoordY, coordX, coordY);

                        if (path != null)
                        {
                            soldier.Path = path;
                        }
                        else
                        {
                            Debug.LogError("Could not find a path.");
                        }
                    }
                    else if (_currentSelection is BarracksModel)
                    {
                        ((BarracksModel) _currentSelection).RallyPoint = new Vector2(coordX, coordY);
                    }
                }
            }
        }

    }
}