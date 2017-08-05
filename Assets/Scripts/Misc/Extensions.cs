using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public static class Extensions
    {
        #region Rect

        public static Vector2 TopLeft(this Rect value)
        {
            return new Vector2(value.xMin, value.yMax);
        }

        #endregion
    }
}