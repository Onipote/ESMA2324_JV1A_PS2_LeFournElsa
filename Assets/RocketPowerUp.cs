using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPowerUp : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovements.instance.SetPowerUpFound(true);
            Destroy(gameObject);
        }
    }
}
