using UnityEngine;
using UnityEngine.InputSystem;

public class CarCameraScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Transform CarTransform;
    public Transform MainCameraPointTransform;
    public Transform ReverseCameraPointTransform; 
    public Transform LeftCameraPointTransform;
    public Transform RightCameraPointTransform;
    private Vector3 Velocity = Vector3.zero;


    bool Left;
    bool Right;
    bool Reverse;

    void Start()
    {
        
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
        
        if (Left == true) // To make camera change when reversing
            {       
                transform.LookAt(CarTransform);
                transform.position = Vector3.SmoothDamp(transform.position, LeftCameraPointTransform.position, ref Velocity, 1f * Time.deltaTime); 
            }
        if (Right == true) // To make camera change when reversing
            {       
                transform.LookAt(CarTransform);
                transform.position = Vector3.SmoothDamp(transform.position, ReverseCameraPointTransform.position, ref Velocity, 1f * Time.deltaTime); 
            }
        if (Reverse == true) // To make camera change when reversing
        {       
            transform.LookAt(CarTransform);
            transform.position = Vector3.SmoothDamp(transform.position, ReverseCameraPointTransform.position, ref Velocity, 67f * Time.deltaTime); 
        }
    }

    void GetInput()
    {
        if (Keyboard.current != null)
        {
            Reverse = false;
  
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) Reverse = true;
        }
    }
}
