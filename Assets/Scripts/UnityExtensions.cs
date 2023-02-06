
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

        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x,
                                               globalScale.y / transform.lossyScale.y,
                                               globalScale.z / transform.lossyScale.z);
        }
    }
}
