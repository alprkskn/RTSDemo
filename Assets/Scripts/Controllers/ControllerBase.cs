using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public abstract class ControllerBase
    {
        protected static Dictionary<ViewBase, ModelBase> ViewToModel = new Dictionary<ViewBase, ModelBase>();

        public virtual void InitModelView(ModelBase model, ViewBase view)
        {
            model.PropertyChanged += view.ModelPropertyChanged;
            ViewToModel.Add(view, model);
        }

        protected ModelBase GetModelOfView<T>(T view) where T : ViewBase
        {
            return view != null ? ViewToModel[view] : null;
        }

        protected void DestroyView(ViewBase view)
        {
            view.OnDestroyed();
            ViewToModel.Remove(view);
        }

    }
}
