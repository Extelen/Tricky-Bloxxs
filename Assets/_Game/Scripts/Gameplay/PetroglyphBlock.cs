using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetroglyphBlock : MonoBehaviour
{
    public float stopThreshold = 0.01f;
    public float checkFrequency = 0.1f;
    public float fallSpeed = 2f;

    public event Action OnRigidbodyStopped;
    public event Action OnOutOfBounds;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    //private Vector2 defaultCollisionSize;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        //defaultCollisionSize = boxCollider.size;
        //boxCollider.size = new Vector2(0.95f, 0.95f);

        rb.angularDrag = 2f; 
        rb.drag = 1f;
    }

    void FixedUpdate()
    {
        if (rb.isKinematic)
        {
            rb.position += Vector2.down * fallSpeed * Time.fixedDeltaTime;
        }
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
        }
    }

    public void TriggerOutOfBounds()
    {
        OnOutOfBounds?.Invoke();
        Destroy(gameObject);
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        rb.isKinematic = false;
        transform.parent = null;
        StartCheckingIfStopped();
        //boxCollider.size = defaultCollisionSize;
        rb.angularDrag = 2f; 
        rb.drag = 2f;
        rb.mass = 0.5f;
        rb.gravityScale = 0.9f;
    }
}