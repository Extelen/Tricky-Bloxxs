using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounds : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Block"))
        {
            other.gameObject.GetComponent<PetroglyphBlock>().TriggerOutOfBounds();
            Debug.Log("Block out of bounds");
        }
    }
}
