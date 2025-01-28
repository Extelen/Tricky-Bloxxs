using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateBehaviourBase : MonoBehaviour
{
    // Variables
    protected abstract GameState GameState { get; }

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
        if (state == GameState)
            OnState();
    }

    protected abstract void OnState();
}
