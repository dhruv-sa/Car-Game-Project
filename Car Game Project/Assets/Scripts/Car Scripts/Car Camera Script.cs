using UnityEngine;

public class CarCameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform CarTransform;
    public Transform CameraPointTransform;
    private Vector3 Velocity = Vector3.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(CarTransform);
        transform.position = Vector3.SmoothDamp(transform.position, CameraPointTransform.position, ref Velocity, 100f*Time.deltaTime); // To add delay in the camera movement
    }
}
