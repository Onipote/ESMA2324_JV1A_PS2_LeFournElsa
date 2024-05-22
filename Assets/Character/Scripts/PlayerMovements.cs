using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public static PlayerMovements instance;
    
    [Header("Basic settings")]
    public Rigidbody2D rb;
    //private Animator anim;
    private float horizontal;
    private float vertical;

    [Header("Flip")]
    private SpriteRenderer sr;
    public float lastDirection = 1f;

    [Header("Walk")]
    [SerializeField] private float baseSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float currentSpeed;

    [Header("Jump and double jump")]
    [SerializeField] private float baseJump;
    [SerializeField] private float waterJump;
    [SerializeField] private float currentJump;
    public int jumpCounter = 0;
    [SerializeField] private int fallGravScale;
    [SerializeField] private int baseGravScale;

    /*[Header("Wall jumping")]
    private bool isWallJumping;

    [Header("Wall sliding")]
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public bool isWalled;*/

    [Header("Dash")]
    private bool powerUpFound = false;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1.5f;
    [SerializeField] private TrailRenderer tr;

    [Header("Will-o'-the-wisp")]
    public int wotwCounter = 0;
    private PlayerCoatSystem pcs;

    [Header("Breakable doors")]
    [SerializeField] private GameObject breakableDoors;
    public bool isTouchingBD;

    [Header("Collapse")]
    [SerializeField] private GameObject collapse;
    public Rigidbody2D rocksRb;
    public bool isTouchingCollTrigger = false;

    [Header("Water check & effect")]
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Transform waterRespawn;
    [SerializeField] private float waterTimerReset = 5f;
    [SerializeField] private bool inWater;
    private float waterTimer;
    private float timeRemaining;
    private bool isTimerRunning = false;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        pcs = GetComponent<PlayerCoatSystem>();
        //anim = GetComponent<Animator>();
        currentSpeed = baseSpeed;
        currentJump = baseJump;
        waterTimer = waterTimerReset;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        /*if (horizontal > 0)
        {
            lastDirection = 1; //right

        }
        else if (horizontal < 0)
        {
            lastDirection = -1; //left
        }*/

        //Cat walk
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
        }
        else
        {
            currentSpeed = baseSpeed;
        }

        //Basic jump and double jump
        
        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded || jumpCounter < 2) //2 = max jumps
            {
                Grav();
                rb.velocity = new Vector2(rb.velocity.x, currentJump);
                jumpCounter++;
                Debug.Log("basic jump");
                //TO ADD : sound
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.gravityScale = fallGravScale;
            Invoke("Grav", 0.5f);
        }

        if (isDashing)
        {
            return;
        }

        if (powerUpFound && Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(Dash());
        }

        //TIMER WATER
        if (inWater)
        {
            currentSpeed = walkSpeed;
            currentJump = waterJump;
        }
        else if (!inWater)
        {
            currentJump = baseJump;
        }

        if (inWater && !isTimerRunning)
        {
            StartTimer(waterTimer);
        }

        if (isTimerRunning)
        {
            if (timeRemaining > 0 && inWater)
            {
                timeRemaining -= Time.deltaTime;
                Debug.Log(timeRemaining);
            }
            else if (timeRemaining <= 0 && inWater)
            {
                timeRemaining = 0;
                TimerEndedRespawn();
            }
            else if (timeRemaining > 0 && !inWater)
            {
                TimerEndedHappyEnd();
            }
        }

        if (isTouchingCollTrigger)
        {
            rocksRb.gravityScale = 3;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            jumpCounter = 0; //Counter reset for double jump
        }

        /*if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWalled = true;
        }*/
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }

        /*if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWalled = false;
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wotw"))
        {
            Destroy(other.gameObject);
            wotwCounter++;
            pcs.AddWotw();
        }

        if (other.gameObject.CompareTag("OnlyDashArea"))
        {
            isTouchingBD = true;

            if (isDashing && isTouchingBD)
            {
                BreakDoors();
            }
        }

        if (other.gameObject.CompareTag("Water"))
        {
            inWater = true;
            
        }

        if (other.gameObject.CompareTag("OnlyDJumpArea"))
        {
            isTouchingCollTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            inWater = false;
        }
    }

    public void FlipSpriteBasedOnDirection(float horizontal)
    {
        if (horizontal < 0)
        {
            sr.flipX = true;
            //anim
        }
        else if (horizontal > 0)
        {
            sr.flipX = false;
            //anim
        }
    }
    private void Grav()
    {
        rb.gravityScale = baseGravScale;
    }

    /*private void WallSlide()
    {
        if (isWalled && !isGrounded && horizontal != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }*/

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(lastDirection * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private void BreakDoors()
    {
        Destroy(breakableDoors);
    }

    public void SetPowerUpFound(bool state)
    {
        this.powerUpFound = state;
    }

    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isTimerRunning = true;
    }

    private void TimerEndedRespawn()
    {
        //Respawn (& lost light) because not out of water
        rb.transform.position = waterRespawn.transform.position;
        isTimerRunning = false;
        inWater = false;
        Debug.Log("Argh ! It's cold...");
        waterTimer = waterTimerReset;
    }
    private void TimerEndedHappyEnd()
    {
        //Timer stopped because out of water
        isTimerRunning = false;
        inWater = false;
        Debug.Log("Hehe.. I'm not afraid anymore !");
        waterTimer = waterTimerReset;
    }
}