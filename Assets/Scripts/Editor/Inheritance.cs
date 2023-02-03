using Game.Waves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Game.Editors
{
#if UNITY_EDITOR
    public static class Inheritance
    {
        public static IEnumerable<Type> GetNonAbstractChildrenOfType<T>()
        {
            var typeOfT = typeof(T);

            var allTypes = Assembly.GetAssembly(typeOfT).GetTypes();
            var result = allTypes.Where(currentType => currentType.IsClass && !currentType.IsAbstract && typeOfT.IsAssignableFrom(currentType));

            return result;
        }
    }
#endif
}
