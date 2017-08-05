using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSDemo
{
    [Flags]
    public enum GridLayers
    {
        Units = 1,
        Buildings = 2
    }

    public class GridManager : Singleton<GridManager>
    {
        private short[,] _map;

        public void InitializeMap(Vector2 mapSize)
        {
            _map = new short[(int)mapSize.x, (int)mapSize.y];
        }

        public bool CheckOverlap(int coordX, int coordY, int width, int height, GridLayers layerMask)
        {
            return GridUtilities.Overlap(_map, coordX, coordY, width, height, (short) layerMask);
        }

        public List<Vector2> FindPath(int coordX, int coordY, int targetX, int targetY, GridLayers layerMask = GridLayers.Buildings)
        {
            return PathFinding.PathFinding.FindPath(_map, coordX, coordY, targetX, targetY, (short) layerMask);
        }

        public void UpdateMap(int coordX, int coordY, int width, int height, GridLayers layer)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _map[coordX + i, coordY + j] = (short)layer;
                }
            }
        }

        public Vector2 GetAvailableSlot(int coordX, int coordY, int width, int height, 
            int targetX, int targetY, GridLayers layersMask = GridLayers.Buildings | GridLayers.Units)
        {
            int minX = -1, minY = -1;
            int minDist = int.MaxValue;

            for (int i = -1; i <= width; i++)
            {
                var x = i + coordX;
                for (int j = -1; j <= height; j++)
                {
                    var y = j + coordY;

                    if(x < 0 || x >= _map.GetLength(0)) continue;
                    if(y < 0 || y >= _map.GetLength(1)) continue;

                    if ((_map[x, y] & (short) layersMask) == 0)
                    {
                        var dist = Mathf.Abs(x - targetX) + Mathf.Abs(y - targetY);
                        if (dist < minDist)
                        {
                            minDist = dist;
                            minX = x;
                            minY = y;
                        }
                    }
                }
            }

            return new Vector2(minX, minY);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
