using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMWController : MonoBehaviour
{

    public WheelCollider FrontRightWheelCollider;
    public WheelCollider RearRightWheelCollider;
    public WheelCollider FrontLeftWheelCollider;
    public WheelCollider RearLeftWheelCollider;

    // Start is to begin
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
       MotorForce();
    }

    void MotorForce()
    {
        RearRightWheelCollider.motorTorque = 100f;
        RearLeftWheelCollider.motorTorque = 100f; 
    }

}
