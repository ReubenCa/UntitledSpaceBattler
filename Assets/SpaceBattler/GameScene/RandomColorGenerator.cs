using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomColorGenerator 
{
   public static Color GetColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
