using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGameState : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField] private GameState m_gameState;

    // Methods
    public void Trigger()
    {
        GameManager.SwitchState(m_gameState);
    }
}
