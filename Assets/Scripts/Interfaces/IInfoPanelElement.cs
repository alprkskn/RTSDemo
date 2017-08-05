using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public interface IInfoPanelElement
    {
        string GetInfoTitle();
        Sprite GetThumbnailImage();
        bool HasProduction();
        List<Type> GetProductList();
    }

}
