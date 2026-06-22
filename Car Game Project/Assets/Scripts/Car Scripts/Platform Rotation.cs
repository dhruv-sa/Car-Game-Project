using UnityEngine;

public class RotatingPlatform : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 15f;
    public Vector3 rotationAxis = Vector3.up;

    void FixedUpdate()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.fixedDeltaTime);
    }
}