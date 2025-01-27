using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSets : Openable
{
    //Variables
    [Header("References")]
    [SerializeField] private List<Openable> m_panels;

    //Methods
    /// <summary>
    /// Open all the panels at the same time.
    /// </summary>
    public override Coroutine Open()
    {
        base.Open();
        m_panels.ForEach(c => c.Open());
        StopAllCoroutines();
        return null;
    }

    /// <summary>
    /// Close all the panels at the same time.
    /// </summary>
    public override Coroutine Close()
    {
        if (!gameObject.activeSelf) return null;

        float time = 0;

        foreach (Openable panel in m_panels)
        {
            panel.Close();
            if (panel.CloseTime > time)
            {
                time = panel.CloseTime;
            }
        }

        return StartCoroutine(DisableOnTime(time));
    }

    public override void Set(bool active)
    {
        base.Set(active);
        m_panels.ForEach(c => c.Set(active));
    }

    //Coroutines
    private IEnumerator DisableOnTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        gameObject.SetActive(false);
    }
}