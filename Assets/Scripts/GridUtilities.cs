using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public static class GridUtilities
    {

        public static bool RectOverlap(int sourceX, int sourceY, int sourceW, int sourceH,
            int targetX, int targetY, int targetW, int targetH)
        {
            return (sourceX < targetX + targetW && sourceX + sourceW > targetX &&
                    sourceY > targetY + targetH && sourceY + sourceH < targetY);
        }

        public static bool Overlap(short[,] map, int x, int y, int width, int height, short layerMask)
        {
            if (x < 0 || y < 0) return true;

            if (x + width >= map.GetLength(0) || y + height >= map.GetLength(1)) return true;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if ((map[x + i, y + j] & layerMask) != 0)
                        return true;
                }
            }
            return false;
        }

    }
}