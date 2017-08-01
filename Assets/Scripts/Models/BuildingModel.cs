using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class BuildingModel : EntityModel, IInfoPanelElement
    {
        /// <summary>
        /// Designates if the building is in pre-construction state or not.
        /// </summary>
        private bool _placed;

        public bool Placed
        {
            get { return _placed; }
            set
            {
                _placed = value;
                NotifyPropertyChange("Placed", value);
            }
        }

        public virtual string GetInfoTitle()
        {
            return Name;
        }

        public virtual Sprite GetThumbnailImage()
        {
            return UiImage;
        }

        public virtual bool HasProduction()
        {
            return false;
        }

        public virtual List<Type> GetProductList()
        {
            return new List<Type>();
        }
    }
}