using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Random = UnityEngine.Random;

public struct Range
{
    public float minimum { get; private set; }
    
    public float maximum { get; private set; }

    /// <summary>Splits a string into a minimum and maximum</summary>
    /// <param name="inputString">Input must be in the format of "x y", where x and y are any float value</param>
    public Range(string inputString)
    {
        var stringArray = inputString.Split(' ');
        
        if (stringArray.Length != 2) 
            throw new Exception($"THERE CAN ONLY BE TWO NUMBERS. FOUND {stringArray.Length}");
        
        if (!float.TryParse(stringArray[0], out float value1))
            throw new Exception($"EXPECTED A NUMBER, GOT {stringArray[0]}");
        
        if (!float.TryParse(stringArray[1], out float value2))
            throw new Exception($"EXPECTED A NUMBER, GOT {stringArray[1]}");

        minimum = Math.Min(value1, value2);
        maximum = Math.Max(value1, value2);
    }
    
    public Range(string minimum, string maximum)
    {
        this.minimum = float.Parse(minimum);
        this.maximum = float.Parse(maximum);
    }

    public Range(float minimum, float maximum)
    {
        this.minimum = minimum;
        this.maximum = maximum;
    }

    public float GetRandomValue => Random.Range(minimum, maximum);
}
