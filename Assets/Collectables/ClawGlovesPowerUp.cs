using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawGlovesPowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerFightingSkills.instance.SetPowerUpFound2(true);
            FindObjectOfType<Info>().DisplayInfo("Press Shift + Mouse 0 to do a heavy attack");
            Destroy(gameObject);
        }
    }
}
