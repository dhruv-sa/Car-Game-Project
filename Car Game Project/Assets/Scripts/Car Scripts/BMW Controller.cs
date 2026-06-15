using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is to begin
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MotorForce();
        UpdateWheels();
    }
    // void GetInput()
    // {
    //     GetInput.GetAxis();
    // }
    void MotorForce()
    {
        RearRightWheelCollider.motorTorque = 10f;
        RearLeftWheelCollider.motorTorque = 10f;
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
