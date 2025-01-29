using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillatingMovement : MonoBehaviour
{

    public float horizontalAmplitude = 1f;
    public float horizontalFrequency = 1f;

    public float verticalAmplitude = 1f;
    public float verticalFrequency = 1f;

    private Vector3 startPosition; 

    private void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.CurrentState != GameState.Gameplay) return;
        
        float x = startPosition.x + Mathf.Cos(Time.time * horizontalFrequency) * horizontalAmplitude;
        float y = startPosition.y + Mathf.Sin(Time.time * verticalFrequency) * verticalAmplitude;
        transform.position = new Vector3(x, y, startPosition.z);
    }

}
