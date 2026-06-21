using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform carTransform;
    public float heightAboveCar = 50f;

    
    public float rotationOffset = 0f;

    void LateUpdate()
    {
        if (carTransform == null) return;

        Vector3 newPosition = new Vector3(carTransform.position.x, carTransform.position.y + heightAboveCar, carTransform.position.z);
        transform.position = newPosition;

        float carYaw = carTransform.eulerAngles.y;

        
        transform.rotation = Quaternion.Euler(90f, carYaw + rotationOffset, 0f);
    }
}