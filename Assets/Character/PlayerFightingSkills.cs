using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightingSkills : MonoBehaviour
{
    private int facedDirection;
    public GameObject rightHitbox;
    public GameObject leftHitbox;

    public PlayerMovements pm;

    //Damages according to the attack
    [SerializeField] private float baseScratch; //when walking + attack
    [SerializeField] private float lightScratch; //when jumping + attack
    [SerializeField] private float heavyScratch; //when running + attack
    private float currentStrenght;

    void Start()
    {
        
    }

    void Update()
    {
        if (pm.isGrounded && Input.GetKey(KeyCode.LeftShift) && Input.GetButtonDown("Fire1"))
        {
            baseS_Attack(facedDirection);
        }
        if (pm.isGrounded && Input.GetButtonDown("Fire1"))
        {
            lightS_Attack(facedDirection);
        }
    }

    public void baseS_Attack(int id)
    {
        baseScratch = UnityEngine.Random.Range(15,70);
        Debug.Log("Base scratch : " + baseScratch);
    }

    public void lightS_Attack(int id)
    {
        lightScratch = UnityEngine.Random.Range(1, 30);
        Debug.Log("Light scratch : " + lightScratch);
    }
    public void heavyS_Attack(int id)
    {
        heavyScratch = UnityEngine.Random.Range(45, 100);
        Debug.Log("Heavy scratch : " + heavyScratch);
    }

}
