using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnGameState : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField]
    private GameState m_state;

    [SerializeField]
    private UnityEvent m_onState;

    [SerializeField]
    private UnityEvent m_onOtherState;

    // Methods
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        GameManager.OnStateSwitch += OnStateSwitch;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        GameManager.OnStateSwitch -= OnStateSwitch;
    }

    private void OnStateSwitch(GameState state)
    {
        if (state == m_state)
            m_onState?.Invoke();

        else
            m_onOtherState?.Invoke();
    }
}
