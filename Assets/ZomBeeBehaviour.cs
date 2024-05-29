using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBeeBehaviour : MonoBehaviour
{
    public static ZomBeeBehaviour instance;

    [Header("Other")]
    [SerializeField] private Rigidbody2D rbZB;

    private float moveDirection = 1f;
    public bool isGoingRight = true;

    [Header("Health")]
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;

    [Header("Speed")]
    [SerializeField] private float currentSpeed;
    [SerializeField] private float baseSpeed;

    [Header("Attack")]
    [SerializeField] Transform player;
    [SerializeField] Transform startRespawn;
    [SerializeField] private float attackSpeed;
    [SerializeField] private int currentDamage;
    private float distance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        rbZB = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentSpeed = baseSpeed;
        rbZB.velocity = new Vector2(currentSpeed * moveDirection, rbZB.velocity.y);
    }

    private void Update()
    {
        if (isGoingRight)
        {
            Flip();
        }
        else
        {
            Flip();
        }

        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position;

        if (distance <= 4)
        {
            currentSpeed = attackSpeed;
            currentDamage = UnityEngine.Random.Range(10,50);
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, currentSpeed * Time.deltaTime);
        }
        else
        {
            currentSpeed = baseSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Attack
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
            player.TakeDamage(currentDamage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("RightHB") || collision.gameObject.CompareTag("LeftHB")) && currentHealth >= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth -= PlayerFightingSkills.instance.currentDamage, 0, maxHealth);
            Debug.Log("ZomBee HP :" + currentHealth);
        }
        if (currentHealth == 0)
        {
            Destroy(gameObject);
            Debug.Log("ZomBee dies.");
        }
    }

    private void Flip()
    {
        if (isGoingRight)
        {
            rbZB.velocity = new Vector2(currentSpeed * moveDirection, rbZB.velocity.y);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            rbZB.velocity = new Vector2(-currentSpeed * moveDirection, rbZB.velocity.y);
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }
    }
}
