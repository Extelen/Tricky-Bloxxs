using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPanel : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField] private string m_identifier;
    [SerializeField] private bool m_triggerOnEnable;

    // Methods
    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (m_triggerOnEnable)
        {
            Switch();
        }
    }

    public void Switch()
    {
        UIManager.Instance.SwitchPanel(m_identifier);
    }
}
