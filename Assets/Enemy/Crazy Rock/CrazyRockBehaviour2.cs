using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyRockBehaviour2 : MonoBehaviour
{
    public static CrazyRockBehaviour2 instance;

    [SerializeField] private Rigidbody2D rbCR;
    [SerializeField] private float life;
    [SerializeField] private float speed;
    [SerializeField] Transform player;
    [SerializeField] Transform startRespawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Destroy(gameObject, life);
    }

    private void Start()
    {
        SetStraightVelocity();
    }

    private void SetStraightVelocity()
    {
        rbCR.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && PlayerHealth.instance.currentHealth > 0)
        {
            PlayerHealth.instance.currentHealth = Mathf.Clamp(PlayerHealth.instance.currentHealth -= CrazyRockBehaviour.instance.currentDamage, 0, PlayerHealth.instance.maxHealth);
            Debug.Log("Player HP :" + PlayerHealth.instance.currentHealth);
            Destroy(gameObject);
        }
        if (PlayerHealth.instance.currentHealth == 0)
        {
            PlayerMovements.instance.rb.transform.position = startRespawn.transform.position; //respawn
            Debug.Log("GAME OVER"); //end message
            PlayerHealth.instance.currentHealth = PlayerHealth.instance.maxHealth; //reset health
        }
    }
}
