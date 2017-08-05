using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class BarracksView : BuildingView
    {
        protected virtual void ProductionListChanged(object sender, List<Type> value)
        {
            
        }

        protected virtual void RallyPointChanged(object model, Vector2? value)
        {
            Debug.LogError("RallyPointChanged " + value.ToString());
        }
    }
}