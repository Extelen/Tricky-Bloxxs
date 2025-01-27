using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class SetDialogue : MonoBehaviour
{
    // Variables
    [Header("Settings")]
    [SerializeField] private Dialogue[] m_dialogue;
    [SerializeField] private DialogueType m_variationType;
    [SerializeField] private UnityEvent m_onEnd;

    // Methods
    public void Set()
    {
        DialogueManager.Instance.SetDialogue(m_dialogue, m_variationType, m_onEnd);
    }
}
