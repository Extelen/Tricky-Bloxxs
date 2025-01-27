using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LogoShowcase : Openable
{
    // Enums
    private enum SkipState { None, Waiting, Skipping }

    // Variables
    [Header("Showcase Settings")]
    [SerializeField] private List<AnimablePanel> m_logos;
    [SerializeField] private float m_holdTime;
    [SerializeField] private float m_waitTime;
    [Space]
    [SerializeField] private InputActionReference m_skipAction;
    [SerializeField] private string m_nextPanelIdentifier = "Menu";

    private Coroutine m_currentBehaviour;
    private SkipState m_skipState;

    // Methods
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        m_logos.ForEach(c => c.gameObject.SetActive(false));
        StartShowcase(SwitchPanel);

        m_skipAction.action.Enable();
        m_skipAction.action.started += Skip;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        m_skipAction.action.Disable();
        m_skipAction.action.started -= Skip;
    }

    /// <summary>
    /// Skip the current logo.
    /// </summary>
    private void Skip(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        if (m_skipState == SkipState.Waiting)
        {
            m_skipState = SkipState.Skipping;
        }
    }

    /// <summary>
    /// Start the logo showcase.
    /// </summary>
    public Coroutine StartShowcase(System.Action onEnd = null)
    {
        gameObject.SetActive(true);

        if (m_currentBehaviour != null) StopCoroutine(m_currentBehaviour);
        m_currentBehaviour = StartCoroutine(Brain(onEnd));
        return m_currentBehaviour;
    }

    /// <summary>
    /// At showcase end, switch the panel to the exposed in the inspector.
    /// </summary>
    private void SwitchPanel()
    {
        UIManager.Instance.SwitchPanel(m_nextPanelIdentifier);
    }

    //Coroutines
    private IEnumerator Brain(System.Action onEnd = null)
    {
        yield return null;
        for (int i = 0; i < m_logos.Count; i++)
        {
            AnimablePanel logo = m_logos[i];

            logo.Open();
            yield return new WaitForSecondsRealtime(logo.OpenTime);

            m_skipState = SkipState.Waiting;

            for (float j = 0; j < m_holdTime; j += Time.unscaledDeltaTime)
            {
                if (m_skipState == SkipState.Skipping)
                {
                    m_skipState = SkipState.None;
                    break;
                }
                yield return null;
            }

            m_skipState = SkipState.None;

            logo.Close();
            yield return new WaitForSecondsRealtime(logo.CloseTime);
            yield return new WaitForSecondsRealtime(m_waitTime);
        }

        onEnd?.Invoke();
    }
}
