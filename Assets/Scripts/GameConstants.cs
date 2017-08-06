using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public static class GameConstants
    {
        #region Misc

        public const int CellSize = 32;

        #endregion

        #region BuildingSizes

        public static readonly Vector2 HouseSize = new Vector2(2, 2);
        public static readonly Vector2 SupplyCenterSize = new Vector2(3, 3);
        public static readonly Vector2 CommandCenterSize = new Vector2(3, 4);
        public static readonly Vector2 BarracksSize = new Vector2(4, 4);
        public static readonly Vector2 PowerPlantSize = new Vector2(2, 3);

        #endregion
    }
}