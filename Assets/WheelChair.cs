using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelChair : MonoBehaviour
{
    [SerializeField]
    private float driveSpeed = 1f;
    [SerializeField]
    private float damping = 0.05f;
    [SerializeField]
    private WheelCollider leftWheel;
    [SerializeField]
    private WheelCollider rightWheel;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetLeftSpeed(float speed)
    {
        leftWheel.motorTorque = speed * driveSpeed;
    }

    public void SetRightSpeed(float speed)
    {
        rightWheel.motorTorque = speed * driveSpeed;
    }

    public void Brake()
    {
        rightWheel.brakeTorque = 100000;
        leftWheel.brakeTorque = 100000;
    }

    public void StopBrake()
    {
        rightWheel.brakeTorque = 0;
        leftWheel.brakeTorque = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = new Vector3(0, 1f, 0);
            transform.rotation = Quaternion.identity;
            Physics.SyncTransforms();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        float motorSpeed = Input.GetAxis("Vertical") * driveSpeed;

        rightWheel.motorTorque *= (1f - damping);
        leftWheel.motorTorque *= (1f - damping);

        // rightWheel.motorTorque = motorSpeed;
        // leftWheel.motorTorque = motorSpeed;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            StopBrake();
            rightWheel.motorTorque = motorSpeed * driveSpeed;
            leftWheel.motorTorque = motorSpeed * driveSpeed;

            if (Input.GetKey(KeyCode.D))
            {
                rightWheel.motorTorque *= 1.5f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                leftWheel.motorTorque *= 1.5f;
            }
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            Brake();
        }

        // moveRight = Input.GetKeyDown(KeyCode.D);
        // moveLeft = Input.GetKeyDown(KeyCode.A);
    }


    // void FixedUpdate()
    // {

    // }
}
