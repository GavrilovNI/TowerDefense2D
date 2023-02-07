using System;
using UnityEngine;

namespace Game.Core
{
    public static class Math3d
    {
        public static Vector3 ProjectPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd)
        {
            Vector3 rhs = point - lineStart;
            Vector3 vector = lineEnd - lineStart;
            float magnitude = vector.magnitude;
            Vector3 vector2 = vector;
            if(magnitude > 1E-06f)
            {
                vector2 /= magnitude;
            }

            float value = Vector3.Dot(vector2, rhs);
            value = Mathf.Clamp(value, 0f, magnitude);
            return lineStart + vector2 * value;
        }

        public static Vector3 GetClosestPointOnPath(Vector3 point, Vector3[] path, out int prevPathPointIndex)
        {
            if(path.Length == 0)
                throw new ArgumentException($"{nameof(path)} is empty");
            if(path.Length == 1)
            {
                prevPathPointIndex = 0;
                return path[0];
            }

            prevPathPointIndex = 0;
            Vector3 closestPoint = ProjectPointLine(point, path[0], path[1]);
            float smallestDistance = Vector3.Distance(point, closestPoint);

            Vector3 pointA = path[1];
            for(int i = 2; i < path.Length; ++i)
            {
                Vector3 pointB = path[i];

                Vector3 pointOnLine = ProjectPointLine(point, pointA, pointB);
                float currentDistance = Vector3.Distance(point, pointOnLine);
                if(currentDistance < smallestDistance)
                {
                    prevPathPointIndex = i - 1;
                    smallestDistance = currentDistance;
                    closestPoint = pointOnLine;
                }

                pointA = pointB;
            }

            return closestPoint;
        }

        public static Vector3 GetClosestPointOnPath(Vector3 point, params Vector3[] path)
        {
            if(path.Length == 0)
                throw new ArgumentException($"{nameof(path)} is empty");
            if(path.Length == 1)
                return path[0];

            Vector3 closestPoint = ProjectPointLine(point, path[0], path[1]);
            float smallestDistance = Vector3.Distance(point, closestPoint);

            Vector3 pointA = path[1];
            for(int i = 2; i < path.Length; ++i)
            {
                Vector3 pointB = path[i];

                Vector3 pointOnLine = ProjectPointLine(point, pointA, pointB);
                float currentDistance = Vector3.Distance(point, pointOnLine);
                if(currentDistance < smallestDistance)
                {
                    smallestDistance = currentDistance;
                    closestPoint = pointOnLine;
                }

                pointA = pointB;
            }

            return closestPoint;
        }

    }
}
