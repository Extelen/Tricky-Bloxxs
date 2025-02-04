using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        
        if (other.attachedRigidbody.CompareTag("Block"))
        {
            other.attachedRigidbody.gameObject.GetComponent<PetroglyphBlock>().TriggerOutOfBounds();
            Debug.Log("Block out of bounds");
        }
    }
}
