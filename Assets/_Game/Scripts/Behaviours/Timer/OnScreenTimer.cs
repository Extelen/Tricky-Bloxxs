using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OnScreenTimer : Singleton<OnScreenTimer>
{
    // Variables
    [Header("References")]
    [SerializeField]
    private TextMeshProUGUI m_renderer;

    private Timer m_currentTimer;

    // Methods
    protected override void Awake()
    {
        base.Awake();
        m_currentTimer = null;
        m_renderer.enabled = false;
    }

    public void SetTimer(Timer timer)
    {
        m_currentTimer = timer;
        m_renderer.enabled = true;

        timer.OnTimePass += OnTimerUpdate;
        OnTimerUpdate(timer.CurrentSeconds);
    }

    public void RemoveTimer()
    {
        m_renderer.enabled = false;

        if (m_currentTimer == null)
            return;

        m_currentTimer.OnTimePass -= OnTimerUpdate;
        m_currentTimer = null;
    }

    private void OnTimerUpdate(int seconds)
    {
        if (!m_renderer)
            return;

        int mins = Mathf.FloorToInt(seconds / 60f);
        int secs = Mathf.FloorToInt(seconds % 60f);
        m_renderer.text = $"{mins:00}:{secs:00}";
    }
}
