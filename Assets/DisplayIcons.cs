using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayIcons : MonoBehaviour
{
    [SerializeField] private Image dashIcon;
    [SerializeField] private Image heavyAttackIcon;

    private void Start()
    {
        dashIcon.enabled = false;
        heavyAttackIcon.enabled = false;
    }

    void Update()
    {
        if (PlayerMovements.instance.powerUpFound == true)
        {
            dashIcon.enabled = true;
        }
        if (PlayerFightingSkills.instance.powerUpFound2 == true)
        {
            heavyAttackIcon.enabled = true;
        }
    }
}
