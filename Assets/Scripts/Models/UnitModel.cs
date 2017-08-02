using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class UnitModel : EntityModel, IInfoPanelElement
    {
        private float _movementRate; // time to advance one step in milliseconds.
        private List<Vector2> _path;

        public float MovementRate
        {
            get { return _movementRate; }
            set
            {
                _movementRate = value;
                NotifyPropertyChange("MovementRate", value);
            }
        }

        public List<Vector2> Path
        {
            get { return _path; }
            set
            {
                _path = value;
                NotifyPropertyChange("Path", value);
            }
        }

        public void RemovePath(Vector2 value)
        {
            if (_path.Remove(value))
            {
                NotifyCollectionModification("Path", value, CollectionModification.Remove);
            }
        }

        public virtual string GetInfoTitle()
        {
            return "";
        }

        public virtual Sprite GetThumbnailImage()
        {
            return null;
        }

        public virtual bool HasProduction()
        {
            return false;
        }

        public virtual List<Type> GetProductList()
        {
            return new List<Type>();
        }

        public override IInfoPanelElement GetInfoPanelElement()
        {
            return this;
        }
    }
}