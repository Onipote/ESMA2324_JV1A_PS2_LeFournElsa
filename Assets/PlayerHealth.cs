using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;

    public float maxHealth = 100;
    public float currentHealth;

    public float invulnerabilityDuration = 1f;
    public bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    private SpriteRenderer spriteRenderer;
    public Color invulnerableColor = Color.red;
    private Color originalColor;
    [SerializeField] private Transform startRespawn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }
    private void Update()
    {
        if (isInvulnerable)
        {
            //timer starts
            invulnerabilityTimer -= Time.deltaTime;

            //timer ends
            if (invulnerabilityTimer <= 0f)
            {
                isInvulnerable = false;
                spriteRenderer.color = originalColor;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (!isInvulnerable)
        {
            ActivateInvulnerability();
            if (currentHealth > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth -= damage, 0, maxHealth);
                Debug.Log("Player HP :" + currentHealth);
            }
            
            if (currentHealth <= 0)
            {
                PlayerMovements.instance.rb.transform.position = startRespawn.transform.position; //respawn
                Debug.Log("GAME OVER"); //end message
                currentHealth = maxHealth; //reset health
            }
        }
    }

    private void ActivateInvulnerability()
    {
        isInvulnerable = true;
        spriteRenderer.color = invulnerableColor;
        invulnerabilityTimer = invulnerabilityDuration;
    }
}
