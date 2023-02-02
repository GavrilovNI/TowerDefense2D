
namespace UnityEngine.Extensions
{
    public static class UnityExtensions
    {
        public static bool IsNull(this UnityEngine.Object obj)
        {
            return obj == null || obj.Equals(null);
        }

        public static bool IsNotNull(this UnityEngine.Object obj)
        {
            return IsNull(obj) == false;
        }
    }
}
