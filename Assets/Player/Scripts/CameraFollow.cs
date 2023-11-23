using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public float cameraSmoothSpeed = 0.1f;
    public float accelerationFactor = 0.1f;

    private Camera mainCamera;
    private float originalOrthographicSize;
    void Start()
    {
        mainCamera = Camera.main;
        originalOrthographicSize = mainCamera.orthographicSize;
        target = GameObject.FindAnyObjectByType<player>().gameObject.transform;
    }
    private void FixedUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;

            float playerSpeed = target.GetComponent<Rigidbody2D>().velocity.magnitude;
            float targetOrthographicSize = originalOrthographicSize + playerSpeed * accelerationFactor;
            if (mainCamera.orthographicSize + 1 > originalOrthographicSize)
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, cameraSmoothSpeed / 4);
            else
                mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, cameraSmoothSpeed);
        }
    }
}
