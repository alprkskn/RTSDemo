using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    // This is a simple singleton to behave as a single
    // entry point to the game.
    public class AppRoot : Singleton<AppRoot>
    {
        private Dictionary<string, ControllerBase> _controllerDict;

        // Use this for initialization
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            _controllerDict = new Dictionary<string, ControllerBase>();

            foreach (var controller in this.GetComponentsInChildren<ControllerBase>())
            {
                var baseName = controller.BaseName;

                if (baseName == "")
                    continue;

                _controllerDict.Add(baseName, controller);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public ControllerBase GetController(string controllerName)
        {
            if (_controllerDict.ContainsKey(controllerName))
            {
                return _controllerDict[controllerName];
            }

            return null;
        }
    }
}