using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    //Basic settings
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;

    //Walk
    [SerializeField] private float baseSpeed;
    [SerializeField] private float walkSpeed;
    private float currentSpeed;

    //Jump and double jump
    [SerializeField] private float baseJump;
    [SerializeField] private float discreteJump;
    public int jumpCount = 0;

    //Dash
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;

    //Ground check
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    public bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");


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
            if (isGrounded || jumpCount < 2) //2 = max jumps
            {
                rb.velocity = new Vector2(rb.velocity.x, baseJump);
                jumpCount++;
                Debug.Log("basic jump");
                //TO ADD : sound
            }
        }

        //Discrete jump and double jump

        if (Input.GetButtonDown("Jump") && Input.GetKey(KeyCode.LeftShift))
        {
            if (isGrounded || jumpCount < 2) //2 = max jumps
            {
                rb.velocity = new Vector2(rb.velocity.x, discreteJump);
                jumpCount++;
                Debug.Log("discrete jump");
                //TO ADD : sound
            }
        }

        //Flip sprite

        Flip();

        //Dash

        if (isDashing)
        {
            return;
        }
        
        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
            jumpCount = 0; //Counter reset for double jump
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        //Dash
        if (isDashing)
        {
            return;
        }

        //Walk
        rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        anim.SetBool("goRight", horizontal > 0);
        anim.SetBool("goLeft", horizontal < 0);
    }
    
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}