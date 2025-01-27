using UnityEngine;

[System.Serializable]
public class Timer
{
    // Variables
    [SerializeField]
    private int m_defaultMinutes;
    public int DefaultMinutes
    {
        get
        {
            return m_defaultMinutes;
        }

        set
        {
            m_defaultMinutes = value;
        }
    }


    [SerializeField]
    private int m_defaultSeconds;
    public int DefaultSeconds
    {
        get
        {
            return m_defaultSeconds;
        }

        set
        {
            m_defaultSeconds = value;
        }
    }

    private float m_currentSeconds;
    public int CurrentSeconds
    {
        get
        {
            return Mathf.CeilToInt(m_currentSeconds);
        }
    }

    // Events
    public event CallbackDelegate OnEnd;
    public event TimePassDelegate OnTimePass;

    // Constructors
    public Timer(int minutes, int seconds)
    {
        m_defaultMinutes = minutes;
        m_defaultSeconds = seconds;
    }

    // Methods
    public void Create(CallbackDelegate onEnd)
    {
        m_currentSeconds = (m_defaultMinutes * 60) + m_defaultSeconds;
        OnEnd = onEnd;

        OnTimePass?.Invoke((int)m_currentSeconds);
    }

    public void Tick()
    {
        if (m_currentSeconds == 0)
            return;

        int deltaSecs = (int)m_currentSeconds;
        m_currentSeconds -= Time.deltaTime;
        m_currentSeconds = Mathf.Max(0, m_currentSeconds);

        if (deltaSecs != (int)m_currentSeconds)
            OnTimePass?.Invoke((int)m_currentSeconds);

        if (m_currentSeconds == 0)
            OnEnd?.Invoke();
    }

    // Delegates
    public delegate void CallbackDelegate();
    public delegate void TimePassDelegate(int seconds);
}