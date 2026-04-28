using UnityEngine;

public class HorizontalLooper : MonoBehaviour
{
    public Transform cameraTransform;

    [Range(0f, 1f)]
    public float horizontalParallax = 0.5f;

    public float extraBuffer = 2f;

    private Transform[] tiles;
    private float tileWidth;

    private Vector3 startPosition;
    private Vector3 cameraStartPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        startPosition = transform.position;
        cameraStartPosition = cameraTransform.position;

        tiles = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
            tiles[i] = transform.GetChild(i);

        SpriteRenderer sr = tiles[0].GetComponent<SpriteRenderer>();
        tileWidth = sr.bounds.size.x;

        Debug.Log(gameObject.name + " tile width: " + tileWidth);

        ArrangeTiles();
    }

    void LateUpdate()
    {
        Vector3 cameraDelta = cameraTransform.position - cameraStartPosition;

        transform.position = new Vector3(
            startPosition.x + cameraDelta.x * horizontalParallax,
            startPosition.y,
            startPosition.z
        );

        LoopTiles();
    }

    void ArrangeTiles()
    {
        if (tiles.Length < 3) return;

        tiles[0].localPosition = new Vector3(-tileWidth, tiles[0].localPosition.y, tiles[0].localPosition.z);
        tiles[1].localPosition = new Vector3(0f, tiles[1].localPosition.y, tiles[1].localPosition.z);
        tiles[2].localPosition = new Vector3(tileWidth, tiles[2].localPosition.y, tiles[2].localPosition.z);
    }

    void LoopTiles()
    {
        foreach (Transform tile in tiles)
        {
            float distance = cameraTransform.position.x - tile.position.x;

            if (distance > tileWidth + extraBuffer)
            {
                tile.position = new Vector3(
                    GetRightMostX() + tileWidth,
                    tile.position.y,
                    tile.position.z
                );
            }
            else if (distance < -tileWidth - extraBuffer)
            {
                tile.position = new Vector3(
                    GetLeftMostX() - tileWidth,
                    tile.position.y,
                    tile.position.z
                );
            }
        }
    }

    float GetRightMostX()
    {
        float rightMost = tiles[0].position.x;

        foreach (Transform tile in tiles)
        {
            if (tile.position.x > rightMost)
                rightMost = tile.position.x;
        }

        return rightMost;
    }

    float GetLeftMostX()
    {
        float leftMost = tiles[0].position.x;

        foreach (Transform tile in tiles)
        {
            if (tile.position.x < leftMost)
                leftMost = tile.position.x;
        }

        return leftMost;
    }
}