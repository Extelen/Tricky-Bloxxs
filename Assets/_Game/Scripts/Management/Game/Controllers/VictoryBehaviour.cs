using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryBehaviour : GameStateBehaviourBase
{
    // Properties
    protected override GameState GameState => GameState.Victory;

    // Methods
    protected override void OnState()
    {
        Debug.Log("Victory");
        UIManager.Instance.SwitchPanel("Victory");
    }
}
