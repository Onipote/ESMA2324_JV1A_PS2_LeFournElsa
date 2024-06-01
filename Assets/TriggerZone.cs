using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    public bool isChasing;
    private float chaseDistance;

    private void Update()
    {
        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x) //if the player is on the monster's left
            {
                ZomBeeBehaviour.instance.currentSpeed = ZomBeeBehaviour.instance.attackSpeed;
                transform.position += Vector3.left * ZomBeeBehaviour.instance.currentSpeed * Time.deltaTime;

            }
            if (transform.position.x < playerTransform.position.x) //if the player is on the monster's right
            {
                ZomBeeBehaviour.instance.currentSpeed = ZomBeeBehaviour.instance.attackSpeed;
                transform.position += Vector3.right * ZomBeeBehaviour.instance.currentSpeed * Time.deltaTime;

            }
            else
            {
                ZomBeeBehaviour.instance.currentSpeed = ZomBeeBehaviour.instance.baseSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isChasing = true;
        }
    }
}
