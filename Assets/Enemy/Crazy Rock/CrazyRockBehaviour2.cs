using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyRockBehaviour2 : MonoBehaviour
{
    public static CrazyRockBehaviour2 instance;

    [SerializeField] private Rigidbody2D rbCR;
    [SerializeField] private float life;
    [SerializeField] private float speed;

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
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            player.TakeDamage(Random.Range(10,15));
            Destroy(gameObject);
        }
    }
}
