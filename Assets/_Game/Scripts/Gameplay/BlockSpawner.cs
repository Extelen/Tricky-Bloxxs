using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private Transform blockHolder;
    [SerializeField] private float rightHorizontalLimit = 10f;
    [SerializeField] private float leftHorizontalLimit = -10f;
    [SerializeField] private float blockSpawnDelay = 1f;

    private GameObject heldBlock;
    private bool readyToDrop = false;
    private List<GameObject> tilesToSpawn = new List<GameObject>();
    private List<GameObject> tilesDone = new List<GameObject>();


    public void SpawnBlock()
    {
        GameObject spawnedBlock = Instantiate(tilesToSpawn[0], blockHolder.position, Quaternion.identity, blockHolder);
        spawnedBlock.SetActive(true);
        spawnedBlock.GetComponent<Rigidbody2D>().isKinematic = true;
        heldBlock = spawnedBlock;
        readyToDrop = true;
    }

    public void DropBlock()
    {
        readyToDrop = false;
        heldBlock.GetComponent<Rigidbody2D>().isKinematic = false;
        heldBlock.transform.SetParent(null);
        heldBlock.GetComponent<PetroglyphBlock>().StartCheckingIfStopped();
        heldBlock.GetComponent<PetroglyphBlock>().OnRigidbodyStopped += OnBlockStoppedMoving;
        heldBlock = null;
    }

    public void RotateHeldBlock()
    {
        if (heldBlock != null)
        {
            heldBlock.transform.Rotate(0, 0, 90);
        }
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


    public void BlockWasPlaced()
    {
        tilesToSpawn.RemoveAt(0);
        tilesDone.Add(heldBlock);
    }

    private IEnumerator SpawnBlockWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnBlock();
    }

    #region Events
    // Connected through events
    public void OnTouchDrag(Vector2 delta)
    {
        MoveHorizontally(delta.x*Time.deltaTime);
    }

    public void OnTouchSwipe(TouchManager.SwipeDirection direction)
    {
        if (direction == TouchManager.SwipeDirection.Down)
        {
            if (readyToDrop)
            {
                DropBlock();
                //StartCoroutine(SpawnBlockWithDelay(blockSpawnDelay));
            }
        }
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
        // Should probably check if the block is in a valid position first
        // The block should handle it.
        BlockWasPlaced();
        StartCoroutine(SpawnBlockWithDelay(blockSpawnDelay));
    }
    #endregion
}

