using System;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    [Tooltip("Show pictures in front of the dialogue?")]
    [SerializeField] private bool m_showPicture;

    /// <summary>
    /// Show pictures in front of the dialogue?
    /// </summary>
    public bool ShowPicture
    {
        get
        {
            return m_showPicture;
        }

        set
        {
            m_showPicture = value;
        }
    }

    [Tooltip("The character name")]
    [SerializeField] private string m_title;

    /// <summary>
    /// The character name.
    /// </summary>
    public string Title
    {
        get
        {
            return m_title;
        }
        set
        {
            m_title = value;
        }
    }

    [Tooltip("The dialogue text")]
    [SerializeField][TextArea(4, 6)] private string m_text;

    /// <summary>
    /// The dialogue text.
    /// </summary>
    public string Text
    {
        get
        {
            return m_text;
        }

        set
        {
            m_text = value;
        }
    }

    [Tooltip("The picture to show")]
    [SerializeField] private Sprite m_picture;

    /// <summary>
    /// The pictures to show in sequence
    /// </summary>
    public Sprite Picture
    {
        get
        {
            return m_picture;
        }

        set
        {
            m_picture = value;
        }
    }

    [Tooltip("The dialogue has voice or it's going to play a sound per character?")]
    [SerializeField] private bool m_hasVoice;

    /// <summary>
    /// The dialogue has voice or it's going to play a sound per character?
    /// </summary>
    public bool HasVoice
    {
        get
        {
            return m_voice && m_hasVoice;
        }

        set
        {
            m_hasVoice = value;
        }
    }


    [Tooltip("The dialogue voice")]
    [SerializeField] private AudioClip m_voice;

    /// <summary>
    /// The dialogue voice
    /// </summary>
    public AudioClip Voice
    {
        get
        {
            return m_voice;
        }

        set
        {
            m_voice = value;
        }
    }

    [Tooltip("The dialogue can't be skipped until the voice audio ends?")]
    [SerializeField] private bool m_waitUntilVoiceEnd;

    /// <summary>
    /// The dialogue can't be skipped until the voice audio ends?
    /// </summary>
    public bool WaitUntilVoiceEnd
    {
        get
        {
            return m_waitUntilVoiceEnd;
        }

        set
        {
            m_waitUntilVoiceEnd = value;
        }
    }

    [SerializeField] private float m_holdTime = 0f;

    /// <summary>
    /// The dialogue can't be skipped until the hold time passes.
    /// </summary>
    public float HoldTime
    {
        get
        {
            return m_holdTime;
        }

        set
        {
            m_holdTime = value;
        }
    }


    [SerializeField] private float m_triggerNextOnTime = 0f;

    /// <summary>
    /// The dialogue pass automatically after the next time.
    /// </summary>
    public float TriggerNextOnTime
    {
        get
        {
            return m_triggerNextOnTime;
        }

        set
        {
            m_triggerNextOnTime = value;
        }
    }

    [Tooltip("Execute start and end events?")]
    [SerializeField] private bool m_executeEvents;

    /// <summary>
    /// Execute start and end events?
    /// </summary>
    public bool ExecuteEvents
    {
        get
        {
            return m_executeEvents;
        }

        set
        {
            m_executeEvents = value;
        }
    }

    [Tooltip("The event to execute at the start of the dialogue")]
    [SerializeField] private UnityEvent m_onStart;

    /// <summary>
    /// The event to execute at the start of the dialogue
    /// </summary>
    public event DelegateStart OnStart;

    [Tooltip("The event to execute at the end of the dialogue")]
    [SerializeField] private UnityEvent m_onEnd;

    /// <summary>
    /// The event to execute at the end of the dialogue
    /// </summary>
    public event DelegateEnd OnEnd;

    // Methods
    /// <summary>
    /// Execute the start events only if the ExecuteEvents bool is active.
    /// </summary>
    public void TryExecuteStartEvent()
    {
        if (!m_executeEvents) return;
        m_onStart?.Invoke();
        OnStart?.Invoke();
    }

    /// <summary>
    /// Execute the end events only if the ExecuteEvents bool is active.
    /// </summary>
    public void TryExecuteEndEvent()
    {
        if (!m_executeEvents) return;
        m_onEnd?.Invoke();
        OnEnd?.Invoke();
    }

    /// <summary>
    /// Play the voice in the current audio source.
    /// </summary>
    public bool TryPlayVoice(AudioSource source)
    {
        if (!m_hasVoice) return false;
        if (!m_voice) return false;

        source.clip = m_voice;
        source.Play();
        return true;
    }

    // Delegates
    public delegate void DelegateStart();
    public delegate void DelegateEnd();
}
