using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Easing
{
    public enum Type
    {
        Linear,
        InOutSmooth,
        OutElastic,
        Constant,
        InSmooth,
        OutSmooth,
        Instant,
    }

    public static float Lerp(float a, float b, float t, Type type) 
    {
        t = Evaluate(t, type);
        return Mathf.LerpUnclamped(a, b, t);
    }

    public static float Evaluate(float t, Type type)
    {
        return type switch
        {
            Type.Constant => EaseConstant(t),
            Type.Linear => EaseLinear(t),
            Type.InOutSmooth => EaseInOut(t),
            Type.OutElastic => EaseOutElastic(t),
            Type.InSmooth => EaseInSmooth(t),
            Type.OutSmooth => EaseOutSmooth(t),
            _ => t,
        };
    }

    public static float EaseConstant(float t)
    {
        if (t < 0.5f) return 0;
        else return 1;
    }

    public static float EaseLinear(float t)
    {
        return t;
    }

    public static float EaseInOut(float t)
    {
        if (t < 0.5f)
        {
            return 4 * t * t * t;
        }
        else
        {
            return 1 - Mathf.Pow(-2 * t + 2, 3) / 2f;
        }
    }

    public static float EaseInSmooth(float t)
    {
        return t * t * t;
    }

    public static float EaseOutSmooth(float t)
    {
        return 1 - Mathf.Pow(1 - t, 3);
    }

    public static float EaseOutElastic(float t)
    {
        float c4 = 2 * Mathf.PI / 3;

        if (t == 0) return 0;
        else if (t == 1) return 1;
        else
        {
            return Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
        }
    }
}
