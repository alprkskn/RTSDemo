using System.Collections.Generic;
using UnityEngine.UI;

namespace RTSDemo
{
    public interface IInfoPanelElement
    {
        string GetInfoTitle();
        Image GetThumbnailImage();
        bool HasProduction();
        List<IInfoPanelElement> GetProductList();
    }

}
