using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Info : MonoBehaviour
{
    public Image infoBg;
    public TextMeshProUGUI info;

    void Start()
    {
        HideInfo();
    }

    public void DisplayInfo(string infoText,float duration=5f)
    {
        infoBg.enabled = true;
        info.enabled = true;
        info.text = infoText;
        Invoke(nameof(HideInfo), duration);
    }
    public void HideInfo()
    {
        infoBg.enabled = false;
        info.enabled = false;
    }
}
