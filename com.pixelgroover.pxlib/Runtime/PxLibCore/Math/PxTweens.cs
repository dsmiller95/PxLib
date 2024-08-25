using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PxTweens 
{
    //Scale
    public static IEnumerator IEScale(Transform transform, Vector2 startScale, Vector2 midScale, Vector2 endScale, float scaleTime)
    {
        for (float t = 0; t < scaleTime; t += Time.deltaTime)
        {
            if (t > scaleTime)
                t = scaleTime;
            transform.localScale = PxBezier.GetPoint(startScale, midScale, endScale, t / scaleTime);
            yield return null;
        }
        transform.localScale = endScale;
    }
    public static IEnumerator IEScale(Transform transform, Vector2 startScale, Vector2 endScale, float scaleTime)
    {
        for (float t = 0; t < scaleTime; t += Time.deltaTime)
        {
            if (t > scaleTime)
                t = scaleTime;
            transform.localScale = PxBezier.GetPoint(startScale, (startScale + endScale) / 2, endScale, t / scaleTime);
            yield return null;
        }
        transform.localScale = endScale;
    }
    public static IEnumerator IEScaleSmallToLarge(Transform transform, float scaleTime)
    {
        var startScale = new Vector2(0, 0);
        var midScale = new Vector2(.7f, .7f);
        var endScale = new Vector2(1f, 1f);
        if(transform.localScale.x < 0)
        {
            startScale.x *= -1;
            midScale.x *= -1;
            endScale.x *= -1;
        }
        for (float t = 0; t < scaleTime; t += Time.deltaTime)
        {
            if (t > scaleTime)
                t = scaleTime;
            transform.localScale = PxBezier.GetPoint(startScale, midScale, endScale, t / scaleTime);
            yield return null;
        }
        startScale = new Vector2(1, 1);
        midScale = new Vector2(1.1f, 1.1f);
        endScale = new Vector2(1f, 1f);
        if (transform.localScale.x < 0)
        {
            startScale.x *= -1;
            midScale.x *= -1;
            endScale.x *= -1;
        }
        for (float t = 0; t < .1f; t += Time.deltaTime)
        {
            if (t > scaleTime)
                t = scaleTime;
            transform.localScale = PxBezier.GetPoint(startScale, midScale, endScale, t / scaleTime);
            yield return null;
        }
        transform.localScale = endScale;
    }
    //Move Over Time
    public static IEnumerator IEMoveOverTime(Transform transform, Vector2 start, Vector2 mid, Vector2 end, float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = PxBezier.GetPoint(start, mid, end, t / time);
            yield return null;
        }
    }
    public static IEnumerator IEMoveOverTime(Transform transform, Vector2 start, Vector2 end, float time)
    {
        var mid = start + end / 2;
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = PxBezier.GetPoint(start, mid, end, t / time);
            yield return null;
        }
    }
    public static IEnumerator IEMoveOverTime(Transform transform, Vector3 start, Vector3 mid, Vector3 end, float time)
    {
        for (float t = 0; t < time; t += Time.deltaTime)
        {
            transform.position = PxBezier.GetPoint(start, mid, end, t / time);
            yield return null;
        }
    }
}
