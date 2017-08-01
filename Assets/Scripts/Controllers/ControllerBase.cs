using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RTSDemo
{
    public abstract class ControllerBase : Singleton<ControllerBase>
    {
        public string BaseName
        {
            get
            {
                var typeName = this.GetType().Name;

                if (typeName.Substring(typeName.Length - "Controller".Length) != "Controller")
                {
                    Debug.LogErrorFormat("Class for the controller: {0} is not properly named.\n" +
                                         "This method expects names ending with \"Controller\"", typeName);
                    return "";
                }

                var baseName = typeName.Substring(0, typeName.Length - "Controller".Length);
                return baseName;
            }
        }

        protected Dictionary<ViewBase, ModelBase> _viewToModel;

        protected virtual void Awake()
        {
            _viewToModel = new Dictionary<ViewBase, ModelBase>();
        }

        public virtual void InitModelView(ModelBase model, ViewBase view)
        {
            model.PropertyChanged += view.ModelPropertyChanged;
            model.CollectionModified += view.ModelCollectionModified;
            _viewToModel.Add(view, model);
        }

        public ModelBase GetModelOfView<T>(T view) where T : ViewBase
        {
            return view != null ? _viewToModel[view] : null;
        }

        public void DestroyView(ViewBase view)
        {
            view.OnDestroyed();
            _viewToModel.Remove(view);
        }

        public void DestroyModel(ModelBase model)
        {
            var views = _viewToModel.Where(x => x.Value == model).ToList();

            foreach (var keyValuePair in views)
            {
                keyValuePair.Key.OnDestroyed();
                _viewToModel.Remove(keyValuePair.Key);
            }
        }

    }
}
