using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RTSDemo
{
    public enum MouseButton
    {
        LeftMouse,
        RightMouse,
        MiddleMouse,
        None
    }

    public delegate void MouseInputEvent(int x, int y, MouseButton btn);

    public class GridManager : Singleton<GridManager>
    {
        public event MouseInputEvent MouseHover;
        public event MouseInputEvent MouseDown;
        public event MouseInputEvent MouseUp;
        public event MouseInputEvent MouseClick;

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
