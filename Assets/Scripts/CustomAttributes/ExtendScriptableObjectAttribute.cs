using System;
using UnityEngine;

namespace Game.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExtendScriptableObjectAttribute : PropertyAttribute
    {
        public bool Editable;

        public ExtendScriptableObjectAttribute(bool editable = false)
        {
            Editable = editable;
        }
    }
}
