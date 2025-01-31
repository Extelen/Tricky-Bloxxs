using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private GameObject blockPrefab;
    [SerializeField] private float snapInterval = 0.25f;
    [SerializeField] private float rightHorizontalLimit = 10f;
    [SerializeField] private float leftHorizontalLimit = -10f;
    [SerializeField] private float blockSpawnDelay = 1f;

    private Vector3 realPosition;
    private GameObject activeBlock;
    private List<GameObject> tilesToSpawn = new List<GameObject>();
    private List<GameObject> tilesDone = new List<GameObject>();


    void Start()
    {
        SpawnBlock();
        realPosition = transform.position;
    }

    public void SpawnBlock()
    {
        GameObject spawnedBlock = Instantiate(blockPrefab, transform.position, Quaternion.identity, transform);
        activeBlock = spawnedBlock;
        activeBlock.GetComponent<PetroglyphBlock>().OnRigidbodyStopped += OnBlockStoppedMoving;
        activeBlock.GetComponent<PetroglyphBlock>().OnOutOfBounds += OnBlockStoppedMoving;
        //spawnedBlock.GetComponent<Rigidbody2D>().isKinematic = true;
        
    }
/*
    public void DropBlock()
    {
        heldBlock.GetComponent<Rigidbody2D>().isKinematic = false;
        heldBlock.transform.SetParent(null);
        heldBlock.GetComponent<PetroglyphBlock>().StartCheckingIfStopped();
        heldBlock.GetComponent<PetroglyphBlock>().OnRigidbodyStopped += OnBlockStoppedMoving;
        heldBlock = null;
    }*/

    public void RotateHeldBlock()
    {
        if (activeBlock != null)
        {
            activeBlock.GetComponent<Rigidbody2D>().MoveRotation(activeBlock.transform.rotation.eulerAngles.z + 90);
        }
    }

    public void MoveHorizontally(float amount)
    {
        realPosition += new Vector3(amount,0,0) * Time.deltaTime;
        Vector3 snappedPosition = SnapToInterval(realPosition);
        transform.position = snappedPosition;

        if (transform.position.x > rightHorizontalLimit)
        {
            realPosition = new Vector3(rightHorizontalLimit, transform.position.y, transform.position.z);
            //transform.position = new Vector3(rightHorizontalLimit, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < leftHorizontalLimit)
        {
            realPosition = new Vector3(leftHorizontalLimit, transform.position.y, transform.position.z);
            //transform.position = new Vector3(leftHorizontalLimit, transform.position.y, transform.position.z);
        }

        
    }

    private IEnumerator SpawnBlockWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBlock();
    }

    Vector3 SnapToInterval(Vector3 position)
    {
        position.x = Mathf.Round(position.x / snapInterval) * snapInterval;
        //position.y = Mathf.Round(position.y / snapInterval) * snapInterval;
        //position.z = Mathf.Round(position.z / snapInterval) * snapInterval;
        return position;
    }

    #region Events
    // Connected through events
    public void OnTouchDrag(Vector2 delta)
    {
        MoveHorizontally(delta.x);
    }

    public void OnTouchSwipe(TouchManager.SwipeDirection direction)
    {
        Debug.Log("Swipe: " + direction);
    }

    public void OnTouchTap(Vector2 position)
    {
        RotateHeldBlock();
    }

    public void OnGenerationFinished(List<GameObject> tiles)
    {
        tilesToSpawn = tiles;
        SpawnBlock();
    }

    public void OnBlockStoppedMoving()
    {
        activeBlock.GetComponent<PetroglyphBlock>().OnRigidbodyStopped -= OnBlockStoppedMoving;
        activeBlock.GetComponent<PetroglyphBlock>().OnOutOfBounds -= OnBlockStoppedMoving;
        activeBlock = null;
        StartCoroutine(SpawnBlockWithDelay(blockSpawnDelay));
    }
    #endregion
}

