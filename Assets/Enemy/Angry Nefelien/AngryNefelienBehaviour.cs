using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class AngryNefelienBehaviour : MonoBehaviour
{
    public static AngryNefelienBehaviour instance;

    [Header("Other")]
    [SerializeField] private GameObject angryNef;
    [SerializeField] private Rigidbody2D rbAN;

    private float moveDirection = 1f;
    public bool isGoingRight = true;
    [SerializeField] private Collider2D groundCheckPoint;
    public bool isGrounded;

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
    s
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        rbAN = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        currentSpeed = baseSpeed;
        rbAN.velocity = new Vector2(currentSpeed * moveDirection, rbAN.velocity.y);
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
            currentDamage = UnityEngine.Random.Range(0,50);
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
        if (collision.gameObject.CompareTag("PointA"))
        {
            isGoingRight = true;
        }
        else if (collision.gameObject.CompareTag("PointB"))
        {
            isGoingRight = false;
        }

        if ((collision.gameObject.CompareTag("RightHB") || collision.gameObject.CompareTag("LeftHB")) && currentHealth >= 0)
        {
            currentHealth = Mathf.Clamp(currentHealth -= PlayerFightingSkills.instance.currentDamage, 0, maxHealth);
            Debug.Log("Angry nefelien HP :" + currentHealth);
        }
        if (currentHealth == 0)
        {
            Destroy(gameObject);
            Debug.Log("ANGRY NEFELIEN dies.");
        }
    }

    private void Flip()
    {
        if (isGoingRight)
        {
            rbAN.velocity = new Vector2(currentSpeed * moveDirection, rbAN.velocity.y);
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            rbAN.velocity = new Vector2(-currentSpeed * moveDirection, rbAN.velocity.y);
            transform.rotation = Quaternion.Euler(0, 180, 0);

        }
    }
}
