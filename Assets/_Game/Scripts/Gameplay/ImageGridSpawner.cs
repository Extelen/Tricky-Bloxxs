using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ImageGridSpawner : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Sprite fullImage;
    [SerializeField] private int gridSize = 4; // 4x4 grid for 16 tiles
    [SerializeField] private float spacing = 1.1f; // Spacing between tiles
    [SerializeField] private Vector3 gridOffset = new Vector3(0, 2, 0);

    public List<GameObject> tiles = new List<GameObject>();
    
    public UnityEvent<List<GameObject>> OnFinishedGeneratingGrid;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        int totalTiles = gridSize * gridSize;
        Texture2D imageTexture = fullImage.texture;
        int pieceWidth = imageTexture.width / gridSize;
        int pieceHeight = imageTexture.height / gridSize;

        Vector2 startPosition = transform.position - new Vector3((gridSize - 1) * spacing / 2, (gridSize - 1) * spacing / 2) + gridOffset;

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                int index = y * gridSize + x;
                GameObject tile = Instantiate(tilePrefab, startPosition + new Vector2(x * spacing, -y * spacing), Quaternion.identity, transform);
                tile.name = $"Tile {index + 1}";

                // Extract sprite from the full image
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                sr.sprite = Sprite.Create(imageTexture, new Rect(x * pieceWidth, imageTexture.height - (y + 1) * pieceHeight, pieceWidth, pieceHeight), new Vector2(0.5f, 0.5f));
                
                tiles.Add(tile);
            }
        }

        OnFinishedGeneratingGrid.Invoke(tiles);
        foreach (GameObject tile in tiles)
        {
            tile.SetActive(false);
        }
    }
}

