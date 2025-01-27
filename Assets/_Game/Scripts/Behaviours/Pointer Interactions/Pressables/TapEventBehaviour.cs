using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TapEventBehaviour : MonoBehaviour, ITapEventHandler
{
    // Variables
    [Header("Events")]
    [SerializeField] private UnityEvent m_onTap;

    public virtual void OnTap()
    {
        m_onTap?.Invoke();
    }
}
