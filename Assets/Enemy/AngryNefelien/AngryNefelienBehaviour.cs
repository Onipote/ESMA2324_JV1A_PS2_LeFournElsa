using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryNefelienBehaviour : MonoBehaviour
{
    [SerializeField] private Rigidbody2D angryNef;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;
    
    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("RightHB"))
        {
            //to complete
        }
    }
}
