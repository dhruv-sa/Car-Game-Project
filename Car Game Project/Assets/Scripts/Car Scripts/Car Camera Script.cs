using UnityEngine;

public class CarCameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform CarTransform;
    public Transform CameraPointTransform;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(CarTransform);
        transform.position = CameraPointTransform.position;
    }
}
