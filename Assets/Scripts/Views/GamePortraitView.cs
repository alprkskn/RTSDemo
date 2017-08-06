using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class GamePortraitView : GameView
    {
        protected override void Awake()
        {
            base.Awake();
            _productionMenuScrollView.Landscape = false;
        }
    }
}