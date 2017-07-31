using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class BuildingModel : EntityModel, IInfoPanelElement
    {
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