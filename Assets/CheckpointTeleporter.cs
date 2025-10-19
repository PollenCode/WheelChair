using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTeleporter : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    void OnTriggerEnter(Collider other)
    {
        var wheelChair = other.GetComponentInParent<WheelChair>();
        if (wheelChair != null)
        {
            wheelChair.transform.position = target.position;
            wheelChair.transform.rotation = Quaternion.identity;
            var rb = wheelChair.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
