using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private GameObject rockPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth.instance.currentHealth -= CrazyRockBehaviour2.instance.damage;
            Destroy(CrazyRockBehaviour.instance.rockPrefab);
        }
    }
}
