using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RTSDemo
{
    public delegate void BoardClickEventHandler(GameView sender, int coordX, int coordY, PointerEventData.InputButton btn);

    public class GameView : ViewBase
    {
        #region Events

        public event BoardClickEventHandler BoardClickRegistered;

        #endregion

        #region SerializedFields
        [SerializeField] private RectTransform _productionMenu;
        [SerializeField] private RectTransform _productionMenuContent;
        [SerializeField] private RectTransform _gameBoard;
        [SerializeField] private RectTransform _gameBoardContent;
        [SerializeField] private RectTransform _informationPanel;
        [SerializeField] private RectTransform _informationPanelContent;
        [SerializeField] private GridManager _gridManager;
        #endregion

        #region PrivateFields

        // BottomLeft - TopLeft - TopRight - BottomRight
        private Vector3[] _gameBoardCorners;

        private InfiniteScrollView _productionMenuScrollView;
        #endregion

        #region PropertyListenerMethods

        protected virtual void MapSizeChanged(object model, Vector2 mapSize)
        {
            _gameBoardContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  mapSize.x * GameConstants.CellSize);
            _gameBoardContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,  mapSize.y * GameConstants.CellSize);
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
            var typeDict = new Dictionary<Type, Sprite>();

            foreach (var availableBuilding in availableBuildings)
            {
                // Assuming only the correct types are sent.
                // Don't really have time, so I cannot include
                // correctness checks everywhere. :/
                var typeName = availableBuilding.Name.Substring(0, availableBuilding.Name.Length - 5);
                typeDict.Add(availableBuilding,
                    ResourcesManager.Instance.GetSprite(typeName));
            }

            _productionMenuScrollView.SetAvailableElements(typeDict);
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _productionMenuScrollView = _productionMenuContent.GetComponent<InfiniteScrollView>();
        }

        protected override void Start()
        {
            base.Start();
            _gridManager = GridManager.Instance;
        }

        protected override void UIStart()
        {
            base.UIStart();
            _gameBoardCorners = new Vector3[4];
            _gameBoardContent.GetWorldCorners(_gameBoardCorners);
        }

        protected override void Update()
        {
            base.Update();
        }

        public void OnMouseClick(BaseEventData e)
        {
            PointerEventData ptrData = (PointerEventData) e;

            Vector2 mapPosition = ptrData.position - (Vector2) _gameBoardCorners[1];

            // Canvas scales all elements to fit them into the current screen.
            // So the actual world positions change.
            // Hence, mapPosition is normalized before finding the coordinates.2
            mapPosition /= AppRoot.Instance.Canvas.scaleFactor;

            int coordX = (int) mapPosition.x / GameConstants.CellSize;
            int coordY = (int) -mapPosition.y / GameConstants.CellSize;

            if (BoardClickRegistered != null)
            {
                BoardClickRegistered(this, coordX, coordY, ptrData.button);
            }
        }

        public void OnMouseDrag(BaseEventData e)
        {
            PointerEventData ptrData = (PointerEventData) e;
            _gameBoardContent.Translate(ptrData.delta);
            _gameBoardContent.GetWorldCorners(_gameBoardCorners);

            // TODO: Later movement can be limited to clamp the map to borders.   
        }
    }

}
