using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{ 
    public class SoldierModel : UnitModel
    {
        public override Sprite GetThumbnailImage()
        {
            return ResourcesManager.Instance.GetSprite("Soldier");
        }

        public override string GetInfoTitle()
        {
            return "Soldier";
        }

    }
}