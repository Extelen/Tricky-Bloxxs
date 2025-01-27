using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { Waiting, Gameplay, Victory, GameOver }

public class GameManager : MonoBehaviour
{
    // Events
    public static event GameStateDelegate OnStateSwitch;

    // Variables
    [Header("Settings")]
    [SerializeField] private GameState m_initialState = GameState.Gameplay;

    // Properties
    public static GameState CurrentState { get; private set; }

    // Methods
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    protected virtual void Awake()
    {

#if !UNITY_EDITOR
        // Limits the frame rate only in the standalone build
        Application.targetFrameRate = 60;
#endif

        CurrentState = m_initialState;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    protected virtual void Start()
    {
        OnStateSwitch?.Invoke(CurrentState);
    }

    /// <summary>
    /// Switch the current game manager state and invoke the OnStateSwitch event.
    /// </summary>
    public static void SwitchState(GameState state)
    {
        if (CurrentState == state) return;

        CurrentState = state;
        OnStateSwitch?.Invoke(state);
    }

    // Delegate
    public delegate void GameStateDelegate(GameState state);
}
