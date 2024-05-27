using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private GameObject rockPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("uh");
            PlayerHealth.instance.currentHealth -= (damage += UnityEngine.Random.Range(5, 15));
            Destroy(CrazyRockBehaviour.instance.rockPrefab);
        }
    }
}
