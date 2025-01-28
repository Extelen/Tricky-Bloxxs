using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverBehaviour : GameStateBehaviourBase
{
    // Properties
    protected override GameState GameState => GameState.GameOver;

    // Methods
    protected override void OnState()
    {
        Debug.Log("Game Over");
        UIManager.Instance.SwitchPanel("Game Over");
    }
}
