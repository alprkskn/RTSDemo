using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class UnitModel : ModelBase, IInfoPanelElement
    {
        private int _coordX, _coordY;

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