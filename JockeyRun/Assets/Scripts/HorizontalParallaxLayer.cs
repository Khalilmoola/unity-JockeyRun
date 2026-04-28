using UnityEngine;

public class HorizontalParallaxLayer : MonoBehaviour
{
    public Transform cameraTransform;

    [Range(0f, 1f)]
    public float horizontalParallax = 0.3f;

    private float startX;
    private float startY;
    private float cameraStartX;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        startX = transform.position.x;
        startY = transform.position.y;
        cameraStartX = cameraTransform.position.x;
    }

    void LateUpdate()
    {
        float cameraDeltaX = cameraTransform.position.x - cameraStartX;

        transform.position = new Vector3(
            startX + cameraDeltaX * horizontalParallax,
            startY, // fixed vertically
            transform.position.z
        );
    }
}