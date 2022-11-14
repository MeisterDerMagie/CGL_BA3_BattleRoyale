using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wichtel.Extensions{
public static class Vector2Extensions
{
    //use this to change only one value of a Vector2
    public static Vector2 With(this Vector2 original, float? x = null, float? y = null)
    {
        float newX = x ?? original.x;
        float newY = y ?? original.y;
        return new Vector2(newX, newY);
    }
    
    public static float AngleBetweenTwoPoint(this Vector2 point1, Vector2 point2)
    {
        var deltaPoint = new Vector2(point2.x - point1.x, point2.y - point1.y);
        float radianAngle = Mathf.Atan2(deltaPoint.y, deltaPoint.x);
        float degreeAngle = MathW.RadiansToDegrees(radianAngle);

        return degreeAngle;
    }
}
}