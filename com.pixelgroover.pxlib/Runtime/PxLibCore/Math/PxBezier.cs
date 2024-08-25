using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public static class PxBezier
{
    //Quadratic
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        t = Mathf.Clamp01(t);
        return
            (1f - t) * (1f - t) * p0 +
            2f * (1f - t) * t * p1 +
            t * t * p2;

    }
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return
            2f * (1f - t) * (p1 - p0) +
            2f * t * (p2 - p1);
    }
    //Cubic
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        return
            (1f - t) * (1f - t) * (1f - t) * p0 +
            3f * (1f - t) * (1f - t) * t * p1 +
            3f * (1f - t) * t * t * p2 +
            t * t * t * p3;
    }
    public static Vector3 GetFirstDerivative(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        t = Mathf.Clamp01(t);
        return
            3f * (1f - t) * (1f - t) * (p1 - p0) +
            6f * (1f - t) * t * (p2 - p1) +
            3f * t * t * (p3 - p2);
    }
}
