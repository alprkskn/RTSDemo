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
        private Sprite _mapImage;
        private Sprite _uiImage;
        private bool _highlighted;

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

        public Sprite MapImage
        {
            get { return _mapImage; }
            set
            {
                _mapImage = value;
                NotifyPropertyChange("MapImage", value);
            }
        }

        public Sprite UiImage
        {
            get { return _uiImage; }
            set
            {
                _uiImage = value;
                NotifyPropertyChange("UiImage", value);
            }
        }

        public bool Highlighted
        {
            get { return _highlighted; }
            set
            {
                _highlighted = value;
                NotifyPropertyChange("Highlighted", value);
            }
        }

        public virtual  void OnSelection()
        {
            Highlighted = true;
        }

        public virtual void OnDeSelection()
        {
            Highlighted = false;
        }

        public bool CheckOverlap(int coordX, int coordY)
        {
            // Im using whole pixels, instead of the outline rectangles
            // So widths and heights should be represented as 1 less.
            return GridUtilities.RectOverlap(coordX, coordY, 0, 0,
                _coordX, _coordY, _width - 1, _height - 1);
        }
    }
}