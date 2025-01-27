using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void RestartCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine coroutine, IEnumerator enumerator)
    {
        if (coroutine != null) monoBehaviour.StopCoroutine(coroutine);
        coroutine = monoBehaviour.StartCoroutine(enumerator);
    }

    public static void TryStopCoroutine(this MonoBehaviour monoBehaviour, Coroutine coroutine)
    {
        if (coroutine != null)
            monoBehaviour.StopCoroutine(coroutine);
    }
}
