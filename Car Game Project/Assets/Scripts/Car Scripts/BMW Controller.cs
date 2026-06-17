using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BMWController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider FrontRightWheelCollider;
    public WheelCollider RearRightWheelCollider;
    public WheelCollider FrontLeftWheelCollider;
    public WheelCollider RearLeftWheelCollider;

    [Header("Wheel Visual Mesh Transforms")]
    public Transform RearLeftWheelTransform;
    public Transform RearRightWheelTransform;
    public Transform FrontLeftWheelTransform;
    public Transform FrontRightWheelTransform;

    [Header("Car Physics Settings")]
    public float MotorForceValue = 10000f;
    public float maxSteerAngle = 30f;
    public float steeringSpeed = 150f;

    float VerticalInput;
    float HorizontalInput;
    float currentSteerAngle;
    float targetSteerAngle;

    // This line tells Unity what 'rb' is. If it's missing, you get a red line!
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.maxAngularVelocity = 100f;
        }
        else
        {
            Debug.LogError("Missing a Rigidbody component on this object! Please add one to the Car.");
        }

        // SELF-COLLISION FIX: Stops the car from shaking and vibrating in place
        Collider carBodyCollider = GetComponent<Collider>();
        if (carBodyCollider != null)
        {
            if (FrontRightWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, FrontRightWheelCollider);
            if (FrontLeftWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, FrontLeftWheelCollider);
            if (RearRightWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, RearRightWheelCollider);
            if (RearLeftWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, RearLeftWheelCollider);
        }
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
            VerticalInput = 0f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) VerticalInput = 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) VerticalInput = -1f;

            HorizontalInput = 0f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) HorizontalInput = 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) HorizontalInput = -1f;
        }
    }

    void MotorForce()
    {

        Debug.Log(MotorForceValue);

        if (rb == null) return;

        RearRightWheelCollider.motorTorque = MotorForceValue * VerticalInput;
        RearLeftWheelCollider.motorTorque = MotorForceValue * VerticalInput;
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
        if (WheelTransform == null) return;

        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        WheelTransform.position = pos;
        WheelTransform.rotation = rot;
    }
}