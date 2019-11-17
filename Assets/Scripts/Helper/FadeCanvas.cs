using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class CanvasGroupExtension
{
    public static void FadeIn(this CanvasGroup can, MonoBehaviour mono, float duration)
    {
        mono.StartCoroutine(FadeInCore(can, duration));
    }

    public static void FadeOut(this CanvasGroup can, MonoBehaviour mono, float duration)
    {
        mono.StartCoroutine(FadeOutCore(can, duration));
    }

    public static void FadeInCallback(this CanvasGroup can, MonoBehaviour mono, float duration, System.Action<CanvasGroup> callback = null)
    {
        mono.StartCoroutine(FadeInCoroutine(can, duration, callback));
    }

    public static void FadeOutCallback(this CanvasGroup can, MonoBehaviour mono, float duration, System.Action<CanvasGroup> callback = null)
    {
        mono.StartCoroutine(FadeOutCoroutine(can, duration, callback));
    }

    private static IEnumerator FadeInCore(CanvasGroup can, float duration)
    {
        // Fading animation
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            float alpha = can.alpha;
            alpha = 0f + Mathf.Clamp01((Time.time - start) / duration);
            can.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
    }

    private static IEnumerator FadeOutCore(CanvasGroup can, float duration)
    {
        // Fading animation
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            float alpha = can.alpha;
            alpha = 1f - Mathf.Clamp01((Time.time - start) / duration);
            can.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }
    }

    private static IEnumerator FadeInCoroutine(CanvasGroup can, float duration, System.Action<CanvasGroup> callback)
    {
        // Fading animation
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            float alpha = can.alpha;
            alpha = 0f + Mathf.Clamp01((Time.time - start) / duration);
            can.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }

        // Callback
        if (callback != null)
            callback(can);
    }

    private static IEnumerator FadeOutCoroutine(CanvasGroup can, float duration, System.Action<CanvasGroup> callback)
    {
        // Fading animation
        float start = Time.time;
        while (Time.time <= start + duration)
        {
            float alpha = can.alpha;
            alpha = 1f - Mathf.Clamp01((Time.time - start) / duration);
            can.alpha = alpha;
            yield return new WaitForEndOfFrame();
        }

        // Callback
        if (callback != null)
            callback(can);
    }
}
