//(c) copyright by Martin M. Klöckener

using UnityEngine;

namespace _Project.Scripts.Utility {
public static class RandomPositiveOrNegative
{
    public static int Get()
    {
        return Random.Range(0, 2) * 2 - 1;
    }

    // Random.Range(0,2)       ==  0 or 1
    // Random.Range(0,2)*2     ==  0 or 2
    // Random.Range(0,2)*2-1   == -1 or 1
}
}