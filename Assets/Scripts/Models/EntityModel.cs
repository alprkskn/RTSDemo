using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class EntityModel : ModelBase, ISelectable
    {
        private int _coordX, _coordY;
        private int _width, _height;
        private string _name;
        private Image _mapImage;
        private Image _uiImage;

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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChange("Name", value);
            }
        }

        public Image MapImage
        {
            get { return _mapImage; }
            set
            {
                _mapImage = value;
                NotifyPropertyChange("MapImage", value);
            }
        }

        public Image UiImage
        {
            get { return _uiImage; }
            set
            {
                _uiImage = value;
                NotifyPropertyChange("UiImage", value);
            }
        }
        

        public void OnSelection()
        {
            throw new System.NotImplementedException();
        }

        public void OnDeSelection()
        {
            throw new System.NotImplementedException();
        }
    }
}