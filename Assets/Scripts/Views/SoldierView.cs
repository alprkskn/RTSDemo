using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public delegate void ArrivedWaypointEventHandler(SoldierView sender, int coordX, int coordY);
    public delegate void ArrivedDestinationEvent(SoldierView sender, int coordX, int coordY);

    public class SoldierView : UnitView
    {
        public event ArrivedWaypointEventHandler ArrivedToWaypoint;
        public event ArrivedDestinationEvent ArrivedToDestination;

        private List<Vector2> _path;
        private float _rate;
        private float _elapsedTick;
        private RectTransform _rectTransform;
        private Image _image;

        protected virtual void MovementRateChanged(object sender, float value)
        {
            _rate = value;
        }

        protected virtual void PathChanged(object sender, List<Vector2> value)
        {
            _path = value;
            _elapsedTick = 0;
        }

        protected virtual void PathAdd(object sender, Vector2 value)
        {
            
        }

        protected virtual void PathRemove(object sender, Vector2 value)
        {
            
        }

        protected virtual void PathClear(object sender, Vector2 value)
        {
            
        }

        protected override void CoordXChanged(object model, int coordX)
        {
            base.CoordXChanged(model, coordX);
            float x = coordX * GameConstants.CellSize;
            var pos = _rectTransform.anchoredPosition;
            pos.x = x;
            _rectTransform.anchoredPosition = pos;
        }

        protected override void CoordYChanged(object model, int coordY)
        {
            base.CoordYChanged(model, coordY);
            float y = coordY * GameConstants.CellSize;
            var pos = _rectTransform.anchoredPosition;
            pos.y = -y;
            _rectTransform.anchoredPosition = pos;
        }

        protected override void HighlightedChanged(object model, bool highlighted)
        {
            base.HighlightedChanged(model, highlighted);
            if (highlighted)
            {
                _image.color = Color.yellow;
            }
            else
            {
                _image.color = Color.white;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            _rectTransform = GetComponent<RectTransform>();
            _image = GetComponent<Image>();
        }

        protected override void Update()
        {
            base.Update();
            if (_path != null && _path.Count > 0)
            {
                _elapsedTick += Time.deltaTime;

                if (_elapsedTick > _rate)
                {
                    _elapsedTick -= _rate;
                    var wp = _path[0];
                    if (ArrivedToWaypoint != null)
                    {
                        ArrivedToWaypoint(this, (int) wp.x, (int) wp.y);
                    }

                    //_path.RemoveAt(0);

                    if (_path.Count == 0)
                    {
                        if (ArrivedToDestination != null)
                        {
                            ArrivedToDestination(this, (int)wp.x, (int)wp.y);
                        }
                        _elapsedTick = 0;
                    }
                }
            }
        }
    }
}