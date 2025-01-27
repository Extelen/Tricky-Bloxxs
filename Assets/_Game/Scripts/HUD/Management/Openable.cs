using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Openable : MonoBehaviour
{
    // Enums
    public enum State
    {
        Closed,
        Opening,
        Opened,
        Closing,
    }

    // Variables
    [Header("Panel Settings")]
    [SerializeField] private string m_identifier;

    // Properties
    public State CurrentState { get; set; }

    public string Identifier => m_identifier;

    public virtual float OpenTime => 0;
    public virtual float CloseTime => 0;

    // Methods
    public virtual Coroutine Open()
    {
        CurrentState = State.Opened;
        gameObject.SetActive(true);

        return null;
    }

    public virtual Coroutine Close()
    {
        CurrentState = State.Closed;
        gameObject.SetActive(false);

        return null;
    }

    public virtual void Set(bool active)
    {
        CurrentState = active ? State.Opened : State.Closed;
        gameObject.SetActive(active);
    }
}