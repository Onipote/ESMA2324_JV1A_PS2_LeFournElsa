using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngryNefelienBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject angryNef;
    [SerializeField] private Rigidbody2D rbAN;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float currentHealth;

    [SerializeField] private float speed;
    private float moveDirection = 1f;
    private bool isGoingRight = true;
    private void Start()
    {
        rbAN = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        rbAN.velocity = new Vector2(speed * moveDirection, rbAN.velocity.y);
    }

    private void Update()
    {
        if (isGoingRight)
        {
            rbAN.velocity = new Vector2(speed * moveDirection, rbAN.velocity.y);
        }
        else
        {
            rbAN.velocity = new Vector2(-speed * moveDirection, rbAN.velocity.y);
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

        if ((collision.gameObject.CompareTag("RightHB") || collision.gameObject.CompareTag("LeftHB")) && currentHealth >= 1)
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
}
