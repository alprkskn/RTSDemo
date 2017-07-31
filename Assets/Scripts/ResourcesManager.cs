using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        [SerializeField] private readonly string _viewsPath;

        private Dictionary<string, ViewBase> _viewsDict;

        void Awake()
        {
            _viewsDict = new Dictionary<string, ViewBase>();

            foreach (var view in Resources.LoadAll<ViewBase>(_viewsPath))
            {
                var name = view.name.Substring(0, view.name.Length - 4);
                _viewsDict.Add(name, view);
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
    }
}