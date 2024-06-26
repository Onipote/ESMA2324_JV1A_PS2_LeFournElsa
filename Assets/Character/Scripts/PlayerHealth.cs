using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public Animator anim;

    [Header("Player's health system)")]
    public float maxHealth = 100;
    public float currentHealth;

    [Header("Player's health system UI)")]
    [SerializeField] private Image healthBar;

    [Header("Invulnerability's frame")]
    public float invulnerabilityDuration = 1f;
    public bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;

    private SpriteRenderer spriteRenderer;
    public Color invulnerableColor = Color.red;
    private Color originalColor;

    [SerializeField] private Vector3 startRespawn;

    [Header("Health Gem")]
    public GameObject lootMob;
    public int gemCounter = 0;
    public bool gem = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        anim = GetComponent<Animator>();
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

        healthBar.fillAmount = Mathf.Clamp(currentHealth/maxHealth, 0, 1);

        if (Input.GetButtonDown("Fire2") && gemCounter >= 1)
        {
            currentHealth += Mathf.Clamp(currentHealth += Random.Range(20, 30), 0, maxHealth);
            gemCounter -= 1;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("HealthGem"))
        {
            FindObjectOfType<Info>().DisplayInfo("Press Mouse 1 to use the health gem");
            Destroy(collision.gameObject);
            gemCounter++;
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
                PlayerMovements.instance.wotwCounter = Mathf.Clamp(PlayerMovements.instance.wotwCounter -= PlayerCoatSystem.instance.lostLight, 0, 1000);
                Debug.Log("Player HP :" + currentHealth);
            }

            if (currentHealth <= 0)
            {
                anim.SetTrigger("dying");
            }
        }
    }

    private void ActivateInvulnerability()
    {
        isInvulnerable = true;
        spriteRenderer.color = invulnerableColor;
        invulnerabilityTimer = invulnerabilityDuration;
    }

    private void Respawn()
    {
        //Respawn
        PlayerMovements.instance.rb.transform.position = startRespawn; //respawn
        Debug.Log("GAME OVER !!"); //end message
        currentHealth = maxHealth; //reset health
    }
}
