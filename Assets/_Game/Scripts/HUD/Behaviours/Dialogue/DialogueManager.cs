using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DialogueSystem;

public enum DialogueType
{
    Common = 0,
}

public class DialogueManager : Singleton<DialogueManager>
{
    // Structs
    [System.Serializable]
    private struct DialogueVariation
    {
        // Variables
        [SerializeField]
        private DialogueType m_type;
        public DialogueType Type
        {
            get
            {
                return m_type;
            }

            set
            {
                m_type = value;
            }
        }

        [SerializeField]
        private DialogueController m_controller;
        public DialogueController Controller
        {
            get
            {
                return m_controller;
            }

            set
            {
                m_controller = value;
            }
        }

    }

    // Variables
    [Header("Settings")]
    [SerializeField]
    private bool m_useVariations = false;

    [SerializeField]
    private DialogueController m_dialogueController;

    [SerializeField]
    private List<DialogueVariation> m_dialogueVariations;

    // Methods
    protected override void Awake()
    {
        base.Awake();
        m_dialogueController.Initialize();
        m_dialogueVariations.ForEach(c => c.Controller.Initialize());
    }

    public void SetDialogue(Dialogue[] dialogue, DialogueType variation = DialogueType.Common, UnityEvent onEnd = null)
    {
        DialogueController controller = null;

        if (m_useVariations)
        {
            foreach (var item in m_dialogueVariations)
            {
                if (item.Type != variation)
                    continue;

                controller = item.Controller;
                break;
            }

            if (controller == null)
            {
                controller = m_dialogueVariations[0].Controller;
                Debug.LogError("The variation was not found");
            }
        }

        else
            controller = m_dialogueController;

        controller.SetDialogue(dialogue, onEnd);
    }
}
