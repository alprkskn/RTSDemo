using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class BuildingView : EntityView
    {

        [SerializeField] private Image _mapImage;

        private RectTransform _mapImageRectTransform;
        private RectTransform _rectTransform;

        protected void PlacedChanged(object sender, bool placed)
        {
            if (placed)
            {
                _mapImage.color = Color.white;
            }
            else
            {
                _mapImage.color = Color.red;
            }
        }

        protected void PlacementAvailableChanged(object sender, bool placementAvailable)
        {
            if (placementAvailable)
            {
                _mapImage.color = Color.green;
            }
            else
            {
                _mapImage.color = Color.red;
            }
        }

        protected override void HighlightedChanged(object model, bool highlighted)
        {
            base.HighlightedChanged(model, highlighted);
            if (highlighted)
            {
                _mapImage.color = Color.yellow;
            }
            else
            {
                _mapImage.color = Color.white;
            }
        }

        protected override void MapImageChanged(object model, Sprite mapImage)
        {
            base.MapImageChanged(model, mapImage);
            _mapImage.sprite = mapImage;
        }

        protected override void WidthChanged(object model, int width)
        {
            base.WidthChanged(model, width);
            _mapImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,
                                                            width * GameConstants.CellSize);
        }

        protected override void HeightChanged(object model, int height)
        {
            base.HeightChanged(model, height);
            _mapImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,
                                                            height * GameConstants.CellSize);
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

        protected override void Awake()
        {
            base.Awake();
            _mapImageRectTransform = GetComponent<RectTransform>();
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}