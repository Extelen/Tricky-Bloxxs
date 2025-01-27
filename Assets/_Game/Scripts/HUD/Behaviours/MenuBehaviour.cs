using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehaviour : MonoBehaviour
{
    // Variables
    private static bool m_skipShowcase = false;

    [Header("Settings")]
    [SerializeField] private string m_menuPanelIdentifier = "Menu";
    [SerializeField] private string m_logosPanelIdentifier = "Logos";

    // Methods
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        if (m_skipShowcase)
        {
            UIManager.Instance.SwitchPanel(m_menuPanelIdentifier);
        }
        else
        {
            UIManager.Instance.SwitchPanel(m_logosPanelIdentifier);
            m_skipShowcase = true;
        }
    }
}
