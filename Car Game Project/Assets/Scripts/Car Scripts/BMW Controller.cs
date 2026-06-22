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
    public Transform CarCOMTransform;
    public Rigidbody Rigidbody;
    public float CarBraking = 2000f;

    [Header("Drift Marks")]
    public GameObject FL_DriftMark;
    public GameObject RL_DriftMark;
    public GameObject FR_DriftMark;
    public GameObject RR_DriftMark;

    float VerticalInput;
    float HorizontalInput;
    float currentSteerAngle;
    float targetSteerAngle;

    private Rigidbody rb;
    private TrailRenderer fl_Trail;
    private TrailRenderer rl_Trail;
    private TrailRenderer fr_Trail;
    private TrailRenderer rr_Trail;

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

        Rigidbody.centerOfMass = CarCOMTransform.localPosition;

        Collider carBodyCollider = GetComponent<Collider>();
        if (carBodyCollider != null)
        {
            if (FrontRightWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, FrontRightWheelCollider);
            if (FrontLeftWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, FrontLeftWheelCollider);
            if (RearRightWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, RearRightWheelCollider);
            if (RearLeftWheelCollider != null) Physics.IgnoreCollision(carBodyCollider, RearLeftWheelCollider);
        }

        if (FL_DriftMark != null) fl_Trail = FL_DriftMark.GetComponent<TrailRenderer>();
        if (RL_DriftMark != null) rl_Trail = RL_DriftMark.GetComponent<TrailRenderer>();
        if (FR_DriftMark != null) fr_Trail = FR_DriftMark.GetComponent<TrailRenderer>();
        if (RR_DriftMark != null) rr_Trail = RR_DriftMark.GetComponent<TrailRenderer>();

        if (fl_Trail != null) fl_Trail.emitting = false;
        if (rl_Trail != null) rl_Trail.emitting = false;
        if (fr_Trail != null) fr_Trail.emitting = false;
        if (rr_Trail != null) rr_Trail.emitting = false;
    }

    void Update()
    {
        GetInput();
        ApplyBrakes();
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

    void ApplyBrakes()
    {
        bool isBraking = false;

        if (Keyboard.current != null && Keyboard.current.spaceKey.isPressed)
        {
            float speed = rb != null ? rb.linearVelocity.magnitude : 0f;
            if (speed > 0.5f)
            {
                isBraking = true;
            }
        }

        if (isBraking)
        {
            FrontRightWheelCollider.brakeTorque = CarBraking;
            RearRightWheelCollider.brakeTorque = CarBraking;
            FrontLeftWheelCollider.brakeTorque = CarBraking;
            RearLeftWheelCollider.brakeTorque = CarBraking;
        }
        else
        {
            FrontRightWheelCollider.brakeTorque = 0f;
            RearRightWheelCollider.brakeTorque = 0f;
            FrontLeftWheelCollider.brakeTorque = 0f;
            RearLeftWheelCollider.brakeTorque = 0f;
        }

        WheelHit hit;
        bool fl_Grounded = FrontLeftWheelCollider != null && FrontLeftWheelCollider.GetGroundHit(out hit);
        bool rl_Grounded = RearLeftWheelCollider != null && RearLeftWheelCollider.GetGroundHit(out hit);
        bool fr_Grounded = FrontRightWheelCollider != null && FrontRightWheelCollider.GetGroundHit(out hit);
        bool rr_Grounded = RearRightWheelCollider != null && RearRightWheelCollider.GetGroundHit(out hit);

        if (fl_Trail != null) fl_Trail.emitting = isBraking && fl_Grounded;
        if (rl_Trail != null) rl_Trail.emitting = isBraking && rl_Grounded;
        if (fr_Trail != null) fr_Trail.emitting = isBraking && fr_Grounded;
        if (rr_Trail != null) rr_Trail.emitting = isBraking && rr_Grounded;
    }

    void MotorForce()
    {
        if (rb == null) return;

        RearRightWheelCollider.motorTorque = MotorForceValue * VerticalInput;
        RearLeftWheelCollider.motorTorque = MotorForceValue * VerticalInput;
        FrontRightWheelCollider.motorTorque = MotorForceValue * VerticalInput;
        FrontLeftWheelCollider.motorTorque = MotorForceValue * VerticalInput;
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