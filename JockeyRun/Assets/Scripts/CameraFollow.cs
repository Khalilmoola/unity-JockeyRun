using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float xOffset = 5f;
    public float yOffset = 2f;

    void LateUpdate()
    {
        transform.position = new Vector3(
            player.position.x + xOffset,
            player.position.y + yOffset,
            transform.position.z
        );
    }
}