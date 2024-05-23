using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightingSkills : MonoBehaviour
{
    public static PlayerFightingSkills instance;

    public GameObject rightHitbox;
    public GameObject leftHitbox;

    public PlayerMovements pm;

    //Damages according to the attack
    private float baseScratch; //while running/jumping + attack
    private float heavyScratch; //while walking + attack

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (pm.isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Fire1")) //while walking
        {
            heavyS_Attack();
        }
        else if (pm.isGrounded && !Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Fire1")) //while running
        {
            baseS_Attack();
        }
        else if (!pm.isGrounded && Input.GetButtonDown("Fire1")) //while jumping
        {
            baseS_Attack();
        }
    }

    public void baseS_Attack()
    {
        if (IsMouseOnRight())
        {
            Debug.Log("Activating right hitbox for base attack.");
            rightHitbox.SetActive(true);
        }
        else
        {
            Debug.Log("Activating left hitbox for base attack.");
            leftHitbox.SetActive(true);
        }
        baseScratch = UnityEngine.Random.Range(15, 70);
        Debug.Log("atk " + baseScratch);
        DisableHitboxes();
    }

    public void heavyS_Attack()
    {
        if (IsMouseOnRight())
        {
            Debug.Log("Activating right hitbox for heavy attack.");
            rightHitbox.SetActive(true);
        }
        else
        {
            Debug.Log("Activating left hitbox for heavy attack.");
            leftHitbox.SetActive(true);
        }
        heavyScratch = UnityEngine.Random.Range(45, 100);
        Debug.Log("atk " + heavyScratch);
        DisableHitboxes();
    }

    private bool IsMouseOnRight()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosition.x > transform.position.x;
    }

    public void DisableHitboxes()
    {
        StartCoroutine(DisableHitboxesCoroutine());
    }

    private IEnumerator DisableHitboxesCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.1 seconds before disabling
        rightHitbox.SetActive(false);
        leftHitbox.SetActive(false);
    }
}
