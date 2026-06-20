using UnityEngine;
using UnityEngine.InputSystem;

public class CarCameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform CarTransform;
    public Transform MainCameraPointTransform;
    public Transform ReverseCameraPointTransform;
    public Transform StartCameraPointTransform;
    public Transform StartCameraPoint2Transform;
    private Vector3 Velocity = Vector3.zero;

    bool Reverse;

    void Start()
    {
        StartCam();
        
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
    void LateUpdate()
    {
        transform.LookAt(CarTransform);
        transform.position = Vector3.SmoothDamp(transform.position, MainCameraPointTransform.position, ref Velocity, 15f * Time.deltaTime); // To add delay in the camera movement
        
        if (Reverse == true) // To make camera change when reversing
        {       
            transform.LookAt(CarTransform);
            transform.position = Vector3.SmoothDamp(transform.position, ReverseCameraPointTransform.position, ref Velocity, 67f * Time.deltaTime); 
        }
    }

    void GetInput()
    {  
        if (Keyboard.current != null) // Loop to use the reverse camera
        {
            Reverse = false;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed || Keyboard.current.rKey.isPressed) Reverse = true;
        }
    }

    void StartCam()
    {
        transform.LookAt(CarTransform);
        transform.position = Vector3.SmoothDamp(transform.position, StartCameraPointTransform.position, ref Velocity, 100f * Time.deltaTime); // To add delay in the camera movement

        transform.LookAt(CarTransform);
        transform.position = Vector3.SmoothDamp(transform.position, StartCameraPoint2Transform.position, ref Velocity, 100f * Time.deltaTime); // To add delay in the camera movement

    }


}
