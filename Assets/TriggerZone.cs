using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    [Header("Before attack")]
    public bool isChasing;

    [Header("After attack")]
    public bool hasAttacked;
    private Vector3 randomDirection;

    private void Start()
    {
        hasAttacked = false;
    }

    private void Update()
    {
        if (isChasing && !hasAttacked)
        {
            if (transform.position.x > playerTransform.position.x) //if the player is on the monster's left
            {
                ZomBeeBehaviour.instance.currentSpeed = ZomBeeBehaviour.instance.attackSpeed;
                transform.position += Vector3.left * ZomBeeBehaviour.instance.currentSpeed * Time.deltaTime;

            }
            else if (transform.position.x < playerTransform.position.x) //if the player is on the monster's right
            {
                ZomBeeBehaviour.instance.currentSpeed = ZomBeeBehaviour.instance.attackSpeed;
                transform.position += Vector3.right * ZomBeeBehaviour.instance.currentSpeed * Time.deltaTime;

            }
            else //if the player is exactly at the same x position as the monster
            {
                //Attack the player
                ZomBeeBehaviour.instance.AttackPlayer();
                hasAttacked = true;
                
                //Generate a random direction to move after attack
                randomDirection = GenerateRandomDirection();
            }
        }
        else if (hasAttacked)
        {
            // Move in the random direction
            ZomBeeBehaviour.instance.currentSpeed = ZomBeeBehaviour.instance.baseSpeed;
            transform.position += randomDirection * ZomBeeBehaviour.instance.currentSpeed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = true;
        }
    }
    private Vector3 GenerateRandomDirection()
    {
        float angle = Random.Range(0f, 360f);
        return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f).normalized;
    }
}
