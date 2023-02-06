
namespace UnityEngine.Extensions
{
    public static class UnityExtensions
    {
        public static bool IsNull(this System.Object obj)
        {
            return obj == null || obj.Equals(null);
        }

        public static bool IsNotNull(this System.Object obj)
        {
            return IsNull(obj) == false;
        }
    }
}
