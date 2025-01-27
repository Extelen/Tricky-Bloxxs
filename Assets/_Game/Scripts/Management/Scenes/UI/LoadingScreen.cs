using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingScreen : MonoBehaviour
{
    //Variables
    [Header("Animation")]
    [SerializeField] private float m_animationTime = 0.5f;
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Openable m_openable;

    private Coroutine m_coroutine;

    //Methods
    /// <summary>
    /// Show the loading screen.
    /// </summary>
    public Coroutine Show()
    {
        gameObject.SetActive(true);

        if (m_coroutine != null) StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(IOCoroutine(true));

        m_openable.Open();

        return m_coroutine;
    }

    /// <summary>
    /// Hide the loading screen 
    /// </summary>
    public Coroutine Hide()
    {
        if (!gameObject.activeSelf) return null;

        if (m_coroutine != null) StopCoroutine(m_coroutine);
        m_coroutine = StartCoroutine(IOCoroutine(false));

        m_openable.Close();

        return m_coroutine;
    }

    //Coroutines
    private IEnumerator IOCoroutine(bool active)
    {
        float a = active ? 0 : 1;
        float b = active ? 1 : 0;

        m_canvasGroup.alpha = a;

        bool triggerAudio = true;

        for (float i = 0; i < m_animationTime; i += Time.deltaTime)
        {
            if (triggerAudio && i > m_animationTime * 0.4f)
            {
                // AudioManager.Instance.PlaySFX(SFX.Transition);
                triggerAudio = false;
            }

            m_canvasGroup.alpha = Mathf.Lerp(a, b, i / m_animationTime);
            yield return null;
        }

        m_canvasGroup.alpha = b;
        yield return null;

        if (!active) gameObject.SetActive(false);
    }
}