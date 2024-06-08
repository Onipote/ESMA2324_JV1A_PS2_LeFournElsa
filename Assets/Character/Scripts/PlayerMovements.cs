using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    public static PlayerMovements instance;

    [Header("Basic settings")]
    public Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private float vertical;

    [Header("Flip")]
    private SpriteRenderer sr;
    public int lastDirection = 1;

    [Header("Walk")]
    public float baseSpeed;
    public float walkSpeed;
    public float currentSpeed;

    [Header("Jump and double jump")]
    [SerializeField] private float baseJump;
    [SerializeField] private float waterJump;
    [SerializeField] private float currentJump;
    public int jumpCounter = 0;
    [SerializeField] private int fallGravScale;
    [SerializeField] private int baseGravScale;

    [Header("Will-o'-the-wisp")]
    public float wotwCounter = 0;

    [Header("Dash")]
    public bool powerUpFound = false;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1.5f;
    [SerializeField] private TrailRenderer tr;

    [Header("Breakable doors")]
    [SerializeField] private GameObject breakableDoors;
    public bool isTouchingBD;

    [Header("Water check & effect")]
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private Transform waterRespawn;
    [SerializeField] private float waterTimerReset = 5f;
    public bool inWater;
    private float waterTimer;
    private float timeRemaining;
    private bool isTimerRunning = false;

    [Header("Ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    [Header("Wall")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private bool isWalled;
    //[SerializeField] private bool isWallSliding;

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
        anim = GetComponent<Animator>();
        currentSpeed = baseSpeed;
        currentJump = baseJump;
        waterTimer = waterTimerReset;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (horizontal > 0)
        {
            lastDirection = 1; //right

        }
        else if (horizontal < 0)
        {
            lastDirection = -1; //left
        }

        if (isDashing)
        {
            return;
        }

        FlipSpriteBasedOnDirection(horizontal);
        ProcessWallSlide();

        //Cat walk
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = walkSpeed;
            anim.SetBool("walking", true);
        }
        else
        {
            currentSpeed = baseSpeed;
            anim.SetBool("walking", false);
            anim.SetBool("running", true);

        }

        //Basic jump and double jump
        if (Input.GetButtonDown("Jump") && !Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded || jumpCounter < 2) //2 = max jumps
            {
                anim.SetBool("jumping", true);
                Grav();
                rb.velocity = new Vector2(rb.velocity.x, currentJump);
                jumpCounter++;
                //TO ADD : sound
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.gravityScale = fallGravScale;
            Invoke("Grav", 0.5f);
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            jumpCounter = 0; //Counter reset for double jump
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWalled = true;
        }

        if (collision.gameObject.CompareTag("OnlyDashArea"))
        {
            if (isDashing)
            {
                isTouchingBD = true;
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            isWalled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wotw"))
        {
            Destroy(other.gameObject);
            wotwCounter++;
            PlayerCoatSystem.instance.AddWotw();
        }

        if (other.gameObject.CompareTag("Water"))
        {
            inWater = true;
            PlayerCoatSystem.instance.lostLight = 1;
            PlayerCoatSystem.instance.RemoveWotw();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Water"))
        {
            inWater = false;
            PlayerCoatSystem.instance.lostLight = 0;
        }
    }

    public void FlipSpriteBasedOnDirection(float horizontal)
    {
        if (horizontal < 0) //watching to the right
        {
            sr.flipX = true;
            //anim
        }
        else if (horizontal > 0) //watching to the left
        {
            sr.flipX = false;
            //anim
        }
    }

    private void Grav()
    {
        rb.gravityScale = baseGravScale;
    }

    //Power-up : Dash
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

    //Wall Sliding
    private void ProcessWallSlide()
    {
        if (!isGrounded && isWalled)
        {
            //isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed));
        }
        else
        {
            //isWallSliding = false;
        }
    }

    //Breakable doors
    private void BreakDoors()
    {
        Destroy(breakableDoors);
    }

    //PowerUp : Dash (Dash badge found)
    public void SetPowerUpFound(bool state)
    {
        this.powerUpFound = state;
    }

    //WATER PART (timer, effects)
    public void StartTimer(float duration)
    {
        timeRemaining = duration;
        isTimerRunning = true;
    }

    private void TimerEndedRespawn()
    {
        //Respawn & lost light because not out of water
        isTimerRunning = false;
        rb.transform.position = waterRespawn.transform.position;
        PlayerCoatSystem.instance.RemoveWotw();
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