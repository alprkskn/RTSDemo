using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class BuildingModel : ModelBase, IInfoPanelElement
    {
        private int _width, _height;
        private int _coordX, _coordY;

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                NotifyPropertyChange("Width", value);
            }
        }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                NotifyPropertyChange("Height", value);
            }
        }

        public int CoordX
        {
            get { return _coordX; }
            set
            {
                _coordX = value;
                NotifyPropertyChange("CoordX", value);
            }
        }

        public int CoordY
        {
            get { return _coordY; }
            set
            {
                _coordY = value;
                NotifyPropertyChange("CoordY", value);
            }
        }

        public virtual string GetInfoTitle()
        {
            return "";
        }

        public virtual Image GetThumbnailImage()
        {
            return null;
        }

        public virtual bool HasProduction()
        {
            return false;
        }

        public virtual List<IInfoPanelElement> GetProductList()
        {
            return new List<IInfoPanelElement>();
        }
    }
}