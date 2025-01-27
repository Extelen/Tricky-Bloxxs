using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnSceneState : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField] private SceneManager.State m_state;
    [SerializeField] private UnityEvent m_event;

    // Methods
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.OnStateChange += OnStateChange;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.OnStateChange -= OnStateChange;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        if (SceneManager.IsLoading) return;

        if (m_state == SceneManager.State.Loaded)
            m_event?.Invoke();
    }

    private void OnStateChange(SceneManager.State state)
    {
        if (state == m_state)
        {
            m_event?.Invoke();
        }
    }
}
