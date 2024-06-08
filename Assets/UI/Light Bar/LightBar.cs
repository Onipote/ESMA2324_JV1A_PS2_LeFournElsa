using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightBar : MonoBehaviour
{
    [SerializeField] private Image lightFill;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lightFill.fillAmount = PlayerCoatSystem.instance.currentIntensity * 0.25f;
    }
}
