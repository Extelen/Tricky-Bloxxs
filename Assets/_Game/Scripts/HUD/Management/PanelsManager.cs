using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField] private bool m_onEnableInstant = true;
    [SerializeField] private string m_initialIdentifier;
    [SerializeField] private List<Openable> m_panels;

    // Properties
    public string CurrentIdentifier { get; private set; }

    // Methods
    private void OnEnable()
    {
        SwitchPanel(m_initialIdentifier, m_onEnableInstant);
    }

    public virtual void SwitchPanel(string identifier) => SwitchPanel(identifier, false);
    public virtual void SwitchPanel(string identifier, bool instant)
    {
        CurrentIdentifier = identifier;

        foreach (Openable panel in m_panels)
        {
            bool active = panel.Identifier == identifier;

            if (instant)
            {
                panel.Set(active);
                continue;
            }

            if (active) panel.Open();
            else panel.Close();
        }
    }
}