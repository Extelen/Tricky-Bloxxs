using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityGroupBehaviour : IOBase
{
    // Variables
    [Header("Settings")]
    [SerializeField][Range(0, 1)] private float m_alpha = 1;

    [Header("Animation")]
    [SerializeField] private float m_animationTime = 0.5f;

    private SpriteRenderer[] m_renderers;
    private Coroutine m_inOutBehaviour;

    // Properties
    public float Alpha
    {
        get => m_alpha;
        set
        {
            m_alpha = Mathf.Clamp01(value);
            SetAlpha(m_renderers);
        }
    }

    public override float AnimationTime => m_animationTime;

    // Methods
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        m_renderers = GetComponentsInChildren<SpriteRenderer>();
        SetAlpha(m_renderers);
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    private void OnValidate()
    {
        m_alpha = Mathf.Clamp01(m_alpha);

        var renderers = GetComponentsInChildren<SpriteRenderer>();
        SetAlpha(renderers);
    }

    private void SetAlpha(SpriteRenderer[] renderers)
    {
        if (renderers == null)
            return;

        if (renderers.Length == 0)
            return;

        foreach (var renderer in renderers)
        {
            if (renderer == null)
                continue;

            Color color = renderer.color;
            color.a = m_alpha;
            renderer.color = color;
        }
    }

    public override void Open(bool instant, Callback OnEnd)
    {
        gameObject.SetActive(true);

        if (instant)
        {
            Alpha = 1;
            OnEnd?.Invoke();
            return;
        }

        this.RestartCoroutine(ref m_inOutBehaviour, DoInOut(true));
    }

    public override void Close(bool instant, Callback OnEnd)
    {
        void Disable()
        {
            OnEnd?.Invoke();
            gameObject.SetActive(false);
        }

        if (instant)
        {
            Alpha = 0;
            Disable();
            return;
        }

        this.RestartCoroutine(ref m_inOutBehaviour, DoInOut(false, Disable));
    }

    // Coroutines
    private IEnumerator DoInOut(bool active, Callback onEnd = null)
    {
        float a = Alpha;
        float b = active ? 1 : 0;

        for (float i = 0; i < m_animationTime; i += Time.deltaTime)
        {
            float t = Mathf.Lerp(a, b, i / m_animationTime);
            Alpha = t;
            yield return null;
        }

        Alpha = b;
        onEnd?.Invoke();
    }
}
