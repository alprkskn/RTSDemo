using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSDemo
{
    public class GridManager : Singleton<GridManager>
    {
        private short[,] map;

        public void InitializeMap(Vector2 mapSize)
        {
            map = new short[(int)mapSize.x, (int)mapSize.y];
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
