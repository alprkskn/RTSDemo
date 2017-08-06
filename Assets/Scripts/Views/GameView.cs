using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace RTSDemo
{
    public delegate void BoardClickEventHandler(GameView sender, int coordX, int coordY, PointerEventData.InputButton btn);
    public delegate void BoardHoverEventHandler(GameView sender, int coordX, int coordY);

    public delegate void BuildingConstructionHandler(GameView sender, Type buildingType);

    public delegate void UnitProductionHandler(GameView sender, ProductionBuildingModel building, Type productType);

    public class GameView : ViewBase
    {
        #region Events

        public event BoardClickEventHandler BoardClickRegistered;
        public event BoardHoverEventHandler BoardHoverRegistered;
        public event BuildingConstructionHandler BuildingConstructionStart;
        public event UnitProductionHandler UnitProduced;

        #endregion

        #region SerializedFields
        [SerializeField] private RectTransform _productionTooltip;
        [SerializeField] private RectTransform _productionMenu;
        [SerializeField] private RectTransform _productionMenuContent;
        [SerializeField] private RectTransform _gameBoard;
        [SerializeField] private RectTransform _gameBoardContent;
        [SerializeField] private InformationPanel _informationPanel;
        [SerializeField] private RectTransform _informationPanelContent;
        [SerializeField] private Sprite[] _backgroundMisc;
        #endregion

        #region PrivateFields

        // BottomLeft - TopLeft - TopRight - BottomRight
        private Vector3[] _gameBoardCorners;

        private RectTransform _gameBoardUnitsLayer;
        private RectTransform _gameBoardBackgroundLayer;

        protected InfiniteScrollView _productionMenuScrollView;
        private bool _trackMouseHover = false;
        #endregion

        #region PropertyListenerMethods

        protected virtual void MapSizeChanged(object model, Vector2 mapSize)
        {
            _gameBoardContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,  mapSize.x * GameConstants.CellSize);
            _gameBoardContent.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,  mapSize.y * GameConstants.CellSize);

            // Decorate the background with a few misc sprites.
            DecorateMap(mapSize);

            // For now this will refresh the grid without really cleanin the rest.
            GridManager.Instance.InitializeMap(mapSize);
        }

        protected virtual void BuildingsChanged(object model, List<BuildingModel> buildings)
        {
            
        }

        protected virtual void BuildingsAdd(object model, BuildingModel building)
        {
            GridManager.Instance.UpdateMap(building.CoordX, building.CoordY, building.Width, building.Height, GridLayers.Buildings);
        }

        protected virtual void UnitsChanged(object model, List<UnitModel> units)
        {
            
        }

        protected virtual void SelectedEntityChanged(object model, ISelectable selectable)
        {
            _informationPanel.SetEntity((selectable != null) ? selectable.GetInfoPanelElement() : null);
        }

        protected virtual void UnitsAdd(object model, UnitModel unit)
        {
            GridManager.Instance.UpdateMap(unit.CoordX, unit.CoordY, 1, 1, GridLayers.Units);
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
            _productionMenuScrollView.ElementSelected += element =>
            {
                if (BuildingConstructionStart != null)
                {
                    BuildingConstructionStart(this, element.RepresentedType);
                }
            };

            _productionMenuScrollView.RegisterTooltipObject(_productionTooltip,
                _productionTooltip.GetComponentInChildren<Text>());
        }

        #endregion

        protected override void Awake()
        {
            base.Awake();
            _gameBoardUnitsLayer = _gameBoardContent.Find("UnitsLayer").GetComponent<RectTransform>();
            _gameBoardBackgroundLayer = _gameBoardContent.Find("BackgroundLayer").GetComponent<RectTransform>();
            _productionMenuScrollView = _productionMenuContent.GetComponent<InfiniteScrollView>();
            _productionMenuScrollView.Landscape = true;
        }

        protected override void UIStart()
        {
            base.UIStart();
            _gameBoardCorners = new Vector3[4];
            _gameBoardContent.GetWorldCorners(_gameBoardCorners);
            _informationPanel.UnitProduced += OnUnitProduced;
        }

        private void OnUnitProduced(Type product, IInfoPanelElement entity)
        {
            if (UnitProduced != null)
            {
                UnitProduced(this, (ProductionBuildingModel) entity, product);
            }
        }

        protected override void Update()
        {
            base.Update();

            var canvasScale = AppRoot.Instance.CanvasTransform.localScale;
            canvasScale = new Vector3(1/canvasScale.x, 1/canvasScale.y, 1/canvasScale.z);

            _gameBoardContent.localScale = canvasScale;

            if (UIReady && _trackMouseHover)
            {
                Vector2 mapPosition = (Vector2)Input.mousePosition - (Vector2) _gameBoardCorners[1];
                //mapPosition /= AppRoot.Instance.Canvas.scaleFactor;
                int coordX = (int)mapPosition.x / GameConstants.CellSize;
                int coordY = (int)-mapPosition.y / GameConstants.CellSize;
                if (BoardHoverRegistered != null)
                {
                    BoardHoverRegistered(this, coordX, coordY);
                }
            }
        }

        public void OnMouseClick(BaseEventData e)
        {
            PointerEventData ptrData = (PointerEventData) e;

            // This event fires even when its after dragging
            // It will not register a click when thats the case
            if (!ptrData.dragging)
            {
                Vector2 mapPosition = ptrData.position - (Vector2) _gameBoardCorners[1];

                // Canvas scales all elements to fit them into the current screen.
                // So the actual world positions change.
                // Hence, mapPosition is normalized before finding the coordinates.2
                //mapPosition /= AppRoot.Instance.Canvas.scaleFactor;

                int coordX = (int) mapPosition.x / GameConstants.CellSize;
                int coordY = (int) -mapPosition.y / GameConstants.CellSize;

                if (BoardClickRegistered != null)
                {
                    BoardClickRegistered(this, coordX, coordY, ptrData.button);
                }
            }
        }

        public void OnMouseDrag(BaseEventData e)
        {
            PointerEventData ptrData = (PointerEventData) e;
            _gameBoardContent.Translate(ptrData.delta);
            _gameBoardContent.GetWorldCorners(_gameBoardCorners);

            // TODO: Later movement can be limited to clamp the map to borders.   
        }

        public void OnMouseEnter(BaseEventData e)
        {
            _trackMouseHover = true;
        }

        public void OnMouseExit(BaseEventData e)
        {
            _trackMouseHover = false;
        }

        public void AddToMap(ViewBase child)
        {
            child.GetComponent<Transform>().SetParent(_gameBoardUnitsLayer, false);
        }

        private void DecorateMap(Vector2 mapSize)
        {
            if (_backgroundMisc.Length > 0)
            {
                // Get somewhere between 10% and 20% of the tiles to put decoration on.
                var decorationCount = (int) (mapSize.x * mapSize.y * Random.Range(0.1f, 0.2f));

                for (int i = 0; i < decorationCount; i++)
                {
                    var sprite = _backgroundMisc[Random.Range(0, _backgroundMisc.Length)];
                    var go = new GameObject(sprite.name);

                    var image = go.AddComponent<Image>();
                    var rt = go.GetComponent<RectTransform>();

                    image.sprite = sprite;

                    var pos = new Vector2(Random.Range(0, (int)mapSize.x) * GameConstants.CellSize, 
                        - Random.Range(0, (int)mapSize.y) * GameConstants.CellSize);

                    rt.anchorMax = new Vector2(0, 1);
                    rt.anchorMin = new Vector2(0, 1);
                    rt.pivot = new Vector2(0, 1);

                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sprite.rect.width);
                    rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sprite.rect.width);

                    rt.anchoredPosition = pos;
                    rt.SetParent(_gameBoardBackgroundLayer, false);
                }
            }
        }
    }

}
