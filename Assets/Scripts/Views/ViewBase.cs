using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace RTSDemo
{
    public abstract class ViewBase : MonoBehaviour
    {

        public string BaseName
        {
            get
            {
                var typeName = this.GetType().Name;

                if (typeName.Substring(typeName.Length - "View".Length) != "View")
                {
                    Debug.LogErrorFormat("Class for the view: {0} is not properly named.\n" +
                                         "This method expects names ending with \"View\"", typeName);
                    return "";
                }

                var baseName = typeName.Substring(0, typeName.Length - "View".Length);
                return baseName;
            }
        }

        protected virtual void Awake()
        {
            
        }

        protected virtual void Start()
        {
            // 2 is an arbitrary number.
            // A single WaitForEndOfFrame
            // might not be enough at times.
            StartCoroutine(waitForUI(2));
        }

        private IEnumerator waitForUI(int frameCount)
        {
            for (int i = 0; i < frameCount; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            UIStart();
        }

        protected virtual void Update()
        {

        }

        /// <summary>
        /// UI elements are not always ready when Start is called.
        /// So I will call this after a full frame.
        /// </summary>
        protected virtual void UIStart()
        {
            
        }

        public virtual void ModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MethodInfo method = GetType().GetMethod(e.PropertyName + "Changed",
                BindingFlags.Instance | BindingFlags.NonPublic);

            if (method == null)
            {
                throw new Exception(string.Format("Cannot find the method to invoke for {0} in {1}.", e.PropertyName, GetType().Name));
            }

            method.Invoke(this, new[] {sender, e.Value});
        }

        public virtual void ModelCollectionModified(object sender, CollectionModifiedEventArgs e)
        {
            MethodInfo method = GetType().GetMethod(e.PropertyName + e.ModificationType, 
                BindingFlags.Instance | BindingFlags.NonPublic);
            
            if (method == null)
            {
                throw new Exception(string.Format("Cannot find the method to invoke for {0} in {1}.", e.PropertyName, GetType().Name));
            }

            method.Invoke(this, new[] {sender, e.Value});
        }

        public virtual void OnDestroyed()
        {
            Destroy(gameObject);
        }

    }
}