using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSDemo
{
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs args);

    public class PropertyChangedEventArgs
    {
        public string PropertyName { get; set; }
        public object Value { get; set; }
    }

    public abstract class ModelBase
    {
        public event PropertyChangedEventHandler PropertyChanged;

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
    }
}