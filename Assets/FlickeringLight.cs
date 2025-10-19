using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlickeringLight : MonoBehaviour
{
    [SerializeField]
    private float threshold = 0.8f;

    private new Light light;

    void Awake()
    {
        light = GetComponent<Light>();
    }

    void Update()
    {
        light.enabled = Random.value > threshold;
    }
}
