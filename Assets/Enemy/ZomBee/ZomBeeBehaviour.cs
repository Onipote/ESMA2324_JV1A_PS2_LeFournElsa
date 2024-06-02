using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomBeeBehaviour : MonoBehaviour
{
    public static ZomBeeBehaviour instance;

    [SerializeField] private Rigidbody2D rbZB;
    public float baseSpeed;
    public float currentSpeed;
    public float maxHealth;
    public float currentHealth;

    [SerializeField] private float circleRadius;
    [SerializeField] private GameObject rightCheck, roofCheck, groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private bool isFacingRight = true;
    [SerializeField] private bool groundTouch, roofTouch, rightTouch;
    public float dirX = 1, dirY = 0.25f;

    [Header("Attack")]
    [SerializeField] Transform player;
    [SerializeField] Transform startRespawn;
    public float attackSpeed;
    public int currentDamage;

    private void Awake()
    {
        if (instance = null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        rbZB = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;
        currentSpeed = baseSpeed;
    }

    private void Update()
    {
        rbZB.velocity = new Vector2(dirX, dirY) * currentSpeed * Time.deltaTime;


        HitDetection();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Attack
        if (collision.gameObject.CompareTag("Player"))
        {
            AttackPlayer();
        }
    }

    public void AttackPlayer()
    {
        PlayerHealth.instance.TakeDamage(currentDamage);
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

    private void HitDetection()
    {
        //invisible circle
        rightTouch = Physics2D.OverlapCircle(rightCheck.transform.position, circleRadius, wallLayer);
        roofTouch= Physics2D.OverlapCircle(roofCheck.transform.position, circleRadius, groundLayer);
        groundTouch = Physics2D.OverlapCircle(groundCheck.transform.position, circleRadius, groundLayer);

        HitLogic();
    }

    private void HitLogic()
    {
        if (rightTouch && isFacingRight)
        {
            Flip();
        }
        else if (rightTouch && !isFacingRight)
        {
            Flip();
        }
        if (roofTouch)
        {
            dirY = -0.25f;
        }
        else if (groundTouch)
        {
            dirY = 0.25f;
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(new Vector3(0, 180, 0));
        dirX = -dirX;
    }

    private void OnDrawGizmosSelected()
    {
        //make the circle visible
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(rightCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(roofCheck.transform.position, circleRadius);
        Gizmos.DrawWireSphere(groundCheck.transform.position, circleRadius);
    }
}
