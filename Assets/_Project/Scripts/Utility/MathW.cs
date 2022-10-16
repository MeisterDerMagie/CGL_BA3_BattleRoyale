//copyright by Martin M. Klöckener
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wichtel
{
public static class MathW
{
    //remap for float
    public static float Remap(float value, float low1, float high1, float low2, float high2)
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1);
    }
    
    /// <summary>
    /// returns f rooted by n
    /// </summary>
    /// <param name="f">Zahl, von der die Wurzel gezogen werden soll</param>
    /// <param name="n">n-te Wurzel</param>
    /// <returns></returns>
    public static float NthRoot(float f, float n)
    {
        return Mathf.Pow(f, 1.0f / n);
    }

    //Round towards zero. For positive numbers: Floor, for negative numbers: Ceil
    //Schneidet quasi die Kommastellen ab.
    public static int Truncate(float value)
    {
        if (value < 0f) return Mathf.CeilToInt(value);
        if (value > 0f) return Mathf.FloorToInt(value);
        return 0;
    }

    //rechnet einen Radiant-Wert in einen Grad-Wert um
    public static float RadiansToDegrees(float _radians) => (180f / Mathf.PI) * _radians;
    
    //rechnet einen Grad-Wert in einen Radiant-Wert um
    public static float DegreesToRadians(float _degrees) => (Mathf.PI / 180f) * _degrees;
}
}