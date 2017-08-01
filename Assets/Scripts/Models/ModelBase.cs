using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{

    public enum CollectionModification
    {
        Add, Remove, Clear
    }

    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs args);

    public delegate void CollectionModifiedEventHandler(object sender, CollectionModifiedEventArgs args);

    public class PropertyChangedEventArgs
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }

    public class CollectionModifiedEventArgs
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
        public CollectionModification ModificationType { get; set; }
    }

    public abstract class ModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event CollectionModifiedEventHandler CollectionModified;

        public string BaseName
        {
            get
            {
                var typeName = this.GetType().Name;

                if (typeName.Substring(typeName.Length - "Model".Length) != "Model")
                {
                    Debug.LogErrorFormat("Class for the model: {0} is not properly named.\n" +
                                         "This method expects names ending with \"Model\"", typeName);
                    return "";
                }

                var baseName = typeName.Substring(0, typeName.Length - "Model".Length);
                return baseName;
            }
        }


        protected void NotifyPropertyChange(string propName, object value)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs()
                {
                    PropertyName = propName,
                    Value = value
                });
            }
        }

        protected void NotifyCollectionModification(string propName, object value, CollectionModification modification)
        {
            if (CollectionModified != null)
            {
                CollectionModified(this, new CollectionModifiedEventArgs()
                {
                    PropertyName = propName,
                    Value = value,
                    ModificationType = modification
                });
            }
        }
    }
}