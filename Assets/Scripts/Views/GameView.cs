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

        #endregion


        private GridManager _gridManager;


        protected virtual void MapSizeChanged(object model, Vector2 mapSize)
        {
            
        }

        protected virtual void BuildingsChanged(object model, List<BuildingModel> buildings)
        {
            
        }

        protected virtual void UnitsChanged(object model, List<UnitModel> units)
        {
            
        }

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
