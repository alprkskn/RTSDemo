using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSDemo
{
    [Flags]
    public enum GridLayers
    {
        Units,
        Buildings
    }

    public class GridManager : Singleton<GridManager>
    {
        private short[,] map;

        public void InitializeMap(Vector2 mapSize)
        {
            map = new short[(int)mapSize.x, (int)mapSize.y];
        }

        public bool CheckOverlap(int coordX, int coordY, int width, int height, GridLayers layerMask)
        {
            return GridUtilities.Overlap(map, coordX, coordY, width, height, (short) layerMask);
        }

        public void UpdateMap(int coordX, int coordY, int width, int height, GridLayers layer)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[coordX + i, coordY + j] = (short)layer;
                }
            }
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
