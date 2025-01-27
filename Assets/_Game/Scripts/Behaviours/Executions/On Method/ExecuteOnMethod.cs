using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExecuteOnMethod : MonoBehaviour
{
    // Enums
    public enum Method { Awake, OnEnable, OnDisable, Start }

    // Structs
    [System.Serializable]
    private struct Event
    {
        // Variables
        [SerializeField] private Method m_method;
        [SerializeField] private UnityEvent m_event;

        // Methods
        public void TryInvoke(Method method)
        {
            if (m_method == method)
                m_event?.Invoke();
        }
    }

    // Variables
    [Header("Settings")]
    [SerializeField] private List<Event> m_events;

    // Methods
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        TryExecute(Method.Awake);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        TryExecute(Method.OnEnable);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        TryExecute(Method.OnDisable);
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        TryExecute(Method.Start);
    }

    private void TryExecute(Method method)
    {
        m_events.ForEach(c => c.TryInvoke(method));
    }
}
