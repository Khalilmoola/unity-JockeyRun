using UnityEngine;

public class BackBoundaryFollower : MonoBehaviour
{
    public Transform cameraTransform;
    public float xBehindCamera = 10f;

    void LateUpdate()
    {
        if (cameraTransform == null) return;

        Vector3 pos = transform.position;
        pos.x = cameraTransform.position.x - xBehindCamera;
        transform.position = pos;
    }
}
