using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static string NumberOneDecimalPlace(float value)
    {
        return string.Format("{0:0.0}", value).Replace(',', '.');
    }

    public static string NumberTwoDecimalPlaces(float value)
    {
        return string.Format("{0:0.00}", value).Replace(',', '.');
    }

    public static string NumberSevenDecimalPlaces(double value)
    {
        return string.Format("{0:0.0000000}", value).Replace(',', '.');
    }
}