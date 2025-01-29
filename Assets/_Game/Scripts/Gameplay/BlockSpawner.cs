using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private float rightHorizontalLimit = 10f;
    [SerializeField] private float leftHorizontalLimit = -10f;


    public void SpawnBlock()
    {
        Instantiate(blockPrefab, transform.position, Quaternion.identity);
    }

    public void MoveHorizontally(float amount)
    {
        transform.position += new Vector3(amount, 0, 0);

        if (transform.position.x > rightHorizontalLimit)
        {
            transform.position = new Vector3(rightHorizontalLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < leftHorizontalLimit)
        {
            transform.position = new Vector3(leftHorizontalLimit, transform.position.y, transform.position.z);
        }
    }

    public void OnTouchDrag(Vector2 delta)
    {
        MoveHorizontally(delta.x*Time.deltaTime);
    }

    public void OnTouchSwipe(TouchManager.SwipeDirection direction)
    {
        if (direction == TouchManager.SwipeDirection.Down)
        {
            SpawnBlock();
        }
    }
}

