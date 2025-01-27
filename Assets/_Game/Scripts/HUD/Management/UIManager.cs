using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class UIManager : PanelsManager
{
    // Events
    public static event System.Action<string> OnPanelSwitch;

    // Variables
    public static UIManager Instance { get; private set; }

    // Methods
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;
    }

    public override void SwitchPanel(string identifier, bool instant)
    {
        base.SwitchPanel(identifier, instant);
        OnPanelSwitch?.Invoke(identifier);
    }
}