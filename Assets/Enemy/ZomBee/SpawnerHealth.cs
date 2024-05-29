using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth = 200f;
    [SerializeField] private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void OnTriggerEnter2D(PolygonCollider2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if ((collision.gameObject.CompareTag("RightHB") || collision.gameObject.CompareTag("LeftHB")) && currentHealth > 0)
        {
            float damage = PlayerFightingSkills.instance.currentDamage;
            Debug.Log("Damage detected: " + damage);

            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
            Debug.Log("ZomBee's spawner HP: " + currentHealth);
        }

        if (currentHealth == 0)
        {
            Destroy(gameObject);
            Debug.Log("ZomBee's spawner destroyed.");
        }
    }
}
