using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerCoatSystem : MonoBehaviour
{
    [SerializeField] private Light2D coat;
    [SerializeField] private float baseIntensity;
    [SerializeField] private float currentIntensity;
    [SerializeField] private float baseRange;
    [SerializeField] private float currentRange;

    private void Start()
    {
        coat = GetComponent<Light2D>();
        currentIntensity = baseIntensity;
        currentRange = baseRange;
    }

    public void AddWotw()
    {
        coat.intensity = currentIntensity + PlayerMovements.instance.wotwCounter; //change intensity
        coat.pointLightOuterRadius = currentRange++; //change outer radius
    }
}