using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    // Since we have a limited number of models and only one type of view for each of them
    // I will stick to one base factory class, generating the name of the view prefab from the
    // name.

    public static class ViewFactory
    {
        public static T CreateViewForModel<T>(ModelBase model) where T : ViewBase
        {
            var typeName = typeof(T).Name;

            if (typeName.Substring(typeName.Length - "View".Length) != "View")
            {
                Debug.LogErrorFormat("Class for the view: {0} is not properly named.\n" +
                                     "This method expects names ending with \"View\"", typeName);
            }

            var baseName = typeName.Substring(0, typeName.Length - "View".Length);

            var prefab = ResourcesManager.Instance.GetView(baseName);
            var viewInstance = Object.Instantiate(prefab).GetComponent<T>();
            var controller = AppRoot.Instance.GetController(baseName);

            controller.InitModelView(model, viewInstance);

            return viewInstance;
        }

    }
}