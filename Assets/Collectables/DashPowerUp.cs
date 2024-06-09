using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashPowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovements.instance.SetPowerUpFound(true);
            FindObjectOfType<Info>().DisplayInfo("Press E to dash");
            Destroy(gameObject);
        }
    }
}
