using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class GameView : ViewBase
    {
        #region SerializedFields
        [SerializeField] private RectTransform _productionMenu;
        [SerializeField] private RectTransform _productionMenuContent;
        [SerializeField] private RectTransform _gameBoard;
        [SerializeField] private RectTransform _gridContent;
        [SerializeField] private RectTransform _informationPanel;
        [SerializeField] private RectTransform _informationPanelContent;
        [SerializeField] private GridManager _gridManager;

        #endregion


        #region PropertyListenerMethods

        protected virtual void MapSizeChanged(object model, Vector2 mapSize)
        {
            Debug.Log(mapSize);

            // TODO: Get Cell size from a Constants class.
            _gridContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  mapSize.x * 32);
            _gridContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,  mapSize.y * 32);
        }

        protected virtual void BuildingsChanged(object model, List<BuildingModel> buildings)
        {
            
        }

        protected virtual void BuildingsAdd(object model, BuildingModel building)
        {
            
        }

        protected virtual void UnitsChanged(object model, List<UnitModel> units)
        {
            
        }

        protected virtual void UnitsAdd(object model, UnitModel unit)
        {
            
        }

        protected virtual void AvailableBuildingTypesChanged(object model, List<Type> availableBuildings)
        {
            // TODO: Later this will gather the images and pass them to an InfiniteScrollView.
        }

        #endregion


        protected override void Start()
        {
            _gridManager = GridManager.Instance;
            _gridManager.MouseUp += OnMouseUp;
            _gridManager.MouseDown += OnMouseDown;
            _gridManager.MouseClick += OnMouseClick;
            _gridManager.MouseHover += OnMouseHover;
        }

        protected override void Update()
        {
            // TODO: Keep track of Input.Mouse to fire the mouse events below.
        }

        private void OnMouseUp(int x, int y, MouseButton btn)
        {
            
        }
        private void OnMouseDown(int x, int y, MouseButton btn)
        {
            
        }
        private void OnMouseClick(int x, int y, MouseButton btn)
        {
            
        }
        private void OnMouseHover(int x, int y, MouseButton btn)
        {
            
        }
    }

}
