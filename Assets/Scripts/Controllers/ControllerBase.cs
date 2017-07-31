using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public abstract class ControllerBase : Singleton<ControllerBase>
    {
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

        protected void DestroyView(ViewBase view)
        {
            view.OnDestroyed();
            _viewToModel.Remove(view);
        }

    }
}
