using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class GameView : ViewBase
    {
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
