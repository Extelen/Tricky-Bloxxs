using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DialogueCommonValues
{
    // Variables
    [SerializeField] private AudioSource m_dialogueSource;
    public AudioSource DialogueSource
    {
        get
        {
            return m_dialogueSource;
        }
    }

    [SerializeField] private AudioClip m_charSound;
    public AudioClip CharSound
    {
        get
        {
            return m_charSound;
        }
    }

    [Space]

    [SerializeField] private Image m_characterRenderer;
    public Image CharacterRenderer
    {
        get
        {
            return m_characterRenderer;
        }
    }

    [SerializeField] private Sprite m_characterIdle;
    public Sprite CharacterIdle
    {
        get
        {
            return m_characterIdle;
        }
    }

    [SerializeField] private Sprite m_characterTalking;
    public Sprite CharacterTalking
    {
        get
        {
            return m_characterTalking;
        }
    }
}
