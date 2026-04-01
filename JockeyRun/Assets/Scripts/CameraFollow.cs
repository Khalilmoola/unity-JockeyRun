using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float xOffset = 5f;

    void LateUpdate()
    {
        // Follow the player's X, but keep the camera's original Y and Z
        transform.position = new Vector3(player.position.x + xOffset, transform.position.y, transform.position.z);
    }
}