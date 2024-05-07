using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;
    private float horizontal;
    private bool isFacingRight = true;

    [SerializeField] private float baseSpeed;
    [SerializeField] private float baseJump;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButton("Jump") && isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, baseJump);
        }

        Flip();
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * baseSpeed, rb.velocity.y);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}