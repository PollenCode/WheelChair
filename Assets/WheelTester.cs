using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class WheelTester : MonoBehaviour
{
    [SerializeField]
    private string portName = "COM3";  // Change to your port (e.g. "/dev/ttyUSB0" on Linux/Mac)
    [SerializeField]
    private int baudRate = 9600;
    [SerializeField]
    private float speedMultiplier = 0.1f;
    [SerializeField]
    private WheelChair wheelChair;

    private SerialPort serialPort;
    private Thread readThread;
    private bool keepReading = false;
    private float angle = 0f;

    private Queue<string> messages = new();

    void Start()
    {
        OpenSerialPort();

        Cursor.lockState = CursorLockMode.Locked;
    }

    void OpenSerialPort()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 100;
            serialPort.Open();

            keepReading = true;
            readThread = new Thread(ReadSerial);
            readThread.Start();

            Debug.Log("Serial port opened: " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    void ReadSerial()
    {
        while (keepReading)
        {
            try
            {
                string message = serialPort.ReadLine();
                lock (messages)
                {
                    messages.Enqueue(message);
                }
            }
            catch (System.TimeoutException) { }
            catch (System.Exception e)
            {
                Debug.LogWarning("Serial read error: " + e.Message);
            }
        }
    }

    void Update()
    {
        lock (messages)
        {
            while (messages.Count > 0)
            {
                string message = messages.Dequeue();
                string[] splitMessage = message.Split(":");

                if (splitMessage.Length == 2 && int.TryParse(splitMessage[1], out var timeBetweenSpokes))
                {
                    float speed = (1_000_000f / timeBetweenSpokes) * speedMultiplier;
                    Debug.Log("Speed: " + speed);
                    angle += speed * speedMultiplier;

                    if (splitMessage[0] == "right")
                    {
                        wheelChair.SetLeftSpeed(speed);
                    }
                    else
                    {
                        wheelChair.SetRightSpeed(speed);
                    }
                    // ApplyWheelForce(splitMessage[0] == "right" ? rightWheel : leftWheel, timeBetweenSpokes);
                }
                else
                {
                    Debug.LogWarning("Invalid message: " + message);
                }
            }
        }
        // transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    void OnApplicationQuit()
    {
        keepReading = false;
        if (readThread != null && readThread.IsAlive)
            readThread.Join();

        if (serialPort != null && serialPort.IsOpen)
            serialPort.Close();
    }
}
