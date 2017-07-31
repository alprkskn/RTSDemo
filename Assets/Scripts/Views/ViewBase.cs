using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RTSDemo
{
    public abstract class ViewBase : MonoBehaviour
    {

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public virtual void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MethodInfo method = GetType().GetMethod(e.PropertyName + "Changed", BindingFlags.NonPublic);

            if (method == null)
            {
                throw new Exception(string.Format("Cannot find the method to invoke for {0} in {1}.", e.PropertyName, GetType().Name));
            }

            method.Invoke(this, new[] {sender, e});
        }

        public virtual void ModelCollectionModified(object sender, CollectionModifiedEventArgs e)
        {
            MethodInfo method = GetType().GetMethod(e.PropertyName + e.ModificationType, BindingFlags.NonPublic);
            
            if (method == null)
            {
                throw new Exception(string.Format("Cannot find the method to invoke for {0} in {1}.", e.PropertyName, GetType().Name));
            }

            method.Invoke(this, new[] {sender, e});
        }

        public virtual void OnDestroyed()
        {
            Destroy(gameObject);
        }

    }
}