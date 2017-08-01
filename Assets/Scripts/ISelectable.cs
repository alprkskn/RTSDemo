using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public interface ISelectable
    {
        void OnSelection();
        void OnDeSelection();
        bool CheckOverlap(int coordX, int coordY);
    }
}