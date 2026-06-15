using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BMWController : MonoBehaviour
{
    public WheelCollider FrontRightWheelCollider;
    public WheelCollider RearRightWheelCollider;
    public WheelCollider FrontLeftWheelCollider;
    public WheelCollider RearLeftWheelCollider;

    public Transform RearLeftWheelTransform;
    public Transform RearRightWheelTransform;
    public Transform FrontLeftWheelTransform;
    public Transform FrontRightWheelTransform;

    public float maxSteerAngle = 30f; 
    // Added: How fast the wheels turn per second. Higher = faster, Lower = smoother/slower.
    public float steeringSpeed = 150f; 

    float VerticalInput;
    float HorizontalInput;
    float currentSteerAngle;
    float targetSteerAngle; // Added: Tracks where the wheels *want* to go

    void Start()
    {

    }

    void Update()
    {
        GetInput();
    }

    void FixedUpdate()
    {
        MotorForce();
        SteerCar();    
        UpdateWheels();
    }

    void GetInput()
    {
        if (Keyboard.current != null)
        {
            // Reads W/S or Up/Down Arrow keys
            VerticalInput = 0f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) VerticalInput = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) VerticalInput = -1f;

            // Reads A/D or Left/Right Arrow keys
            HorizontalInput = 0f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) HorizontalInput = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) HorizontalInput = -1f;
        }
    }

    void MotorForce()
    {
        RearRightWheelCollider.motorTorque = 2000f * VerticalInput;
        RearLeftWheelCollider.motorTorque = 2000f * VerticalInput;
    }

    void SteerCar()
    {
        targetSteerAngle = maxSteerAngle * HorizontalInput;
        currentSteerAngle = Mathf.MoveTowards(currentSteerAngle, targetSteerAngle, steeringSpeed * Time.deltaTime);
    
        FrontLeftWheelCollider.steerAngle = currentSteerAngle;
        FrontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    void UpdateWheels()
    {
        RotateWheel(FrontLeftWheelCollider, FrontLeftWheelTransform);
        RotateWheel(FrontRightWheelCollider, FrontRightWheelTransform);
        RotateWheel(RearLeftWheelCollider, RearLeftWheelTransform);
        RotateWheel(RearRightWheelCollider, RearRightWheelTransform);
    }

    void RotateWheel(WheelCollider wheelCollider, Transform WheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        WheelTransform.position = pos;
        WheelTransform.rotation = rot;
    }
}