using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RTSDemo
{
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        [SerializeField] private string _viewsPath;
        [SerializeField] private string _spritesPath;

        private Dictionary<string, ViewBase> _viewsDict;
        private Dictionary<string, Sprite> _spritesDict;

        void Awake()
        {
            // Initialize and populate views dictionary.
            _viewsDict = new Dictionary<string, ViewBase>();

            foreach (var view in Resources.LoadAll<ViewBase>(_viewsPath))
            {
                var name = view.BaseName;

                if (name != "")
                {
                    _viewsDict.Add(name, view);
                }
            }

            // Initialize and populate sprites dictionary.
            _spritesDict = new Dictionary<string, Sprite>();

            foreach (var sprite in Resources.LoadAll<Sprite>(_spritesPath))
            {
                var name = sprite.name;

                if (name != "")
                {
                    _spritesDict.Add(name, sprite);
                }
            }
        }

        public ViewBase GetView(string viewName)
        {
            if (_viewsDict.ContainsKey(viewName))
            {
                return _viewsDict[viewName];
            }

            return null;
        }

        public Sprite GetSprite(string spriteName)
        {
            if (_spritesDict.ContainsKey(spriteName))
            {
                return _spritesDict[spriteName];
            }

            return null;
        }
    }
}