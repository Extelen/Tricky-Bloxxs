using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimablePanel : Openable
{
    // Variables
    [Header("Animation Settings")]
    [SerializeField] private float m_startDelay;

    [UnityEngine.Serialization.FormerlySerializedAs("m_animationTime")]
    [SerializeField] private float m_openTime = 0.25f;

    [UnityEngine.Serialization.FormerlySerializedAs("m_animationTime")]
    [SerializeField] private float m_closeTime = 0.25f;

    [Header("Rect Animation")]
    [SerializeField] private RectTransform m_rectTransform;
    [Space]
    [SerializeField] private Easing.Type m_positionEasingIn = Easing.Type.InOutSmooth;
    [SerializeField] private Easing.Type m_positionEasingOut = Easing.Type.InOutSmooth;
    [Space]
    [SerializeField] private Vector2 m_enabledPosition;
    [SerializeField] private Vector2 m_disabledPosition = Vector2.down * 32;

    [Header("Opacity Animation")]
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private Easing.Type m_opacityEasingIn = Easing.Type.OutSmooth;
    [SerializeField] private Easing.Type m_opacityEasingOut = Easing.Type.OutSmooth;

    [Header("Scale Animation")]
    [SerializeField] private Easing.Type m_scaleEasingIn = Easing.Type.InOutSmooth;
    [SerializeField] private Easing.Type m_scaleEasingOut = Easing.Type.InOutSmooth;
    [Space]
    [SerializeField] private bool m_flipX = false;
    [SerializeField] private float m_disabledScale = 1;
    [SerializeField] private float m_enabledScale = 1;

    private Coroutine m_currentAnimation;

    // Properties
    public override float OpenTime => m_startDelay + m_openTime;
    public override float CloseTime => m_closeTime;

    // Methods
    private void Reset()
    {
        TryGetComponent(out m_canvasGroup);
        TryGetComponent(out m_rectTransform);

        if (!m_canvasGroup) m_canvasGroup = gameObject.AddComponent<CanvasGroup>();

        m_enabledPosition = m_rectTransform.anchoredPosition;
        m_disabledPosition = m_rectTransform.anchoredPosition + new Vector2(0, -32);

        m_enabledScale = Mathf.Abs(m_rectTransform.localScale.x);
        m_disabledScale = m_enabledScale;

        m_flipX = Mathf.Sign(m_rectTransform.localScale.x) == -1;
    }

    /// <summary>
    /// Called when the script is loaded or a value is changed in the
    /// inspector (Called in the editor only).
    /// </summary>
    private void OnValidate()
    {
        m_rectTransform.localScale = ProcessScale(1);
    }

    public override Coroutine Open() => Open(null);
    public Coroutine Open(EndCallback OnEnd = null)
    {
        gameObject.SetActive(true);

        if (m_currentAnimation != null) StopCoroutine(m_currentAnimation);
        m_currentAnimation = StartCoroutine(EaseIn(OnEnd));

        return m_currentAnimation;
    }

    public override Coroutine Close() => Close(null);
    public Coroutine Close(EndCallback OnEnd = null)
    {
        if (!gameObject.activeSelf) return null;

        if (m_currentAnimation != null) StopCoroutine(m_currentAnimation);
        m_currentAnimation = StartCoroutine(EaseOut(OnEnd));

        return m_currentAnimation;
    }

    public override void Set(bool active)
    {
        base.Set(active);
        CurrentState = active ? State.Opened : State.Closed;
        m_rectTransform.anchoredPosition = active ? m_enabledPosition : m_disabledPosition;
        if (m_canvasGroup) m_canvasGroup.alpha = active ? 1 : 0;
    }

    private Vector3 ProcessScale(float scale)
    {
        Vector3 vector = Vector3.one * scale;
        if (m_flipX) vector.x *= -1;
        return vector;
    }

    // Coroutines
    private IEnumerator EaseIn(EndCallback OnEnd)
    {
        CurrentState = State.Opening;

        m_rectTransform.anchoredPosition = m_disabledPosition;
        m_rectTransform.localScale = ProcessScale(m_disabledScale);

        if (m_canvasGroup) m_canvasGroup.alpha = 0;

        yield return new WaitForSeconds(m_startDelay);

        for (float i = 0; i < m_openTime; i += Time.deltaTime)
        {
            float t = i / m_openTime;

            // Positions
            float posT = Easing.Evaluate(t, m_positionEasingIn);
            m_rectTransform.anchoredPosition = Vector2.LerpUnclamped(m_disabledPosition, m_enabledPosition, posT);

            // Scale
            float scaleT = Easing.Evaluate(t, m_scaleEasingIn);
            m_rectTransform.localScale = ProcessScale(Mathf.LerpUnclamped(m_disabledScale, m_enabledScale, scaleT));

            // Alpha
            if (m_canvasGroup)
            {
                if (m_opacityEasingIn == Easing.Type.Instant)
                    m_canvasGroup.alpha = 1;

                else
                {
                    float alphaT = Easing.Evaluate(t, m_opacityEasingIn);
                    m_canvasGroup.alpha = Mathf.LerpUnclamped(0, 1, alphaT);
                }
            }

            yield return null;
        }

        m_rectTransform.anchoredPosition = m_enabledPosition;
        m_rectTransform.localScale = ProcessScale(m_enabledScale);
        if (m_canvasGroup) m_canvasGroup.alpha = 1;

        CurrentState = State.Opened;
        OnEnd?.Invoke();
    }

    private IEnumerator EaseOut(EndCallback OnEnd)
    {
        CurrentState = State.Closing;

        for (float i = 0; i < m_closeTime; i += Time.deltaTime)
        {
            float t = i / m_closeTime;

            // Positions
            float posT = Easing.Evaluate(t, m_positionEasingOut);
            m_rectTransform.anchoredPosition = Vector2.LerpUnclamped(m_enabledPosition, m_disabledPosition, posT);

            // Scale
            float scaleT = Easing.Evaluate(t, m_scaleEasingOut);
            m_rectTransform.localScale = ProcessScale(Mathf.LerpUnclamped(m_enabledScale, m_disabledScale, scaleT));

            // Alpha
            if (m_canvasGroup)
            {
                if (m_opacityEasingOut == Easing.Type.Instant)
                    m_canvasGroup.alpha = 0;

                else
                {
                    float alphaT = Easing.Evaluate(t, m_opacityEasingOut);
                    m_canvasGroup.alpha = Mathf.LerpUnclamped(1, 0, alphaT);
                }
            }

            yield return null;
        }

        m_rectTransform.anchoredPosition = m_disabledPosition;
        m_rectTransform.localScale = ProcessScale(m_disabledScale);
        if (m_canvasGroup) m_canvasGroup.alpha = 0;

        CurrentState = State.Closed;
        gameObject.SetActive(false);
        OnEnd?.Invoke();
    }

    // Delegates
    public delegate void EndCallback();
}