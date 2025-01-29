using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroglyphBlock : MonoBehaviour
{
    public Rigidbody2D rb;
    public float stopThreshold = 0.01f;
    public float checkFrequency = 0.1f;

    public event Action OnRigidbodyStopped;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartCheckingIfStopped()
    {
        InvokeRepeating(nameof(CheckIfStopped), checkFrequency, checkFrequency);
    }	

    void CheckIfStopped()
    {
        if (rb.velocity.magnitude < stopThreshold && rb.angularVelocity < stopThreshold)
        {
            CancelInvoke(nameof(CheckIfStopped));
            OnRigidbodyStopped?.Invoke();
            Debug.Log("Block stopped moving");
        }
    }
}