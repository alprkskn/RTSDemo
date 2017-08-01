using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class UnitModel : EntityModel, IInfoPanelElement
    {
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
    }
}