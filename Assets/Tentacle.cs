using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Tentacle : MonoBehaviour
{
    [SerializeField] private Transform tentacleRotationPoint;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private float targetingRange;
    [SerializeField] private float rotationSpeed;
    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            FindTarget();
            return;
        }
        RotateTowardsPlayer();

        if (!CheckTargetingIsInRange())
        {
            target = null;
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, playerMask);
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    private bool CheckTargetingIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void RotateTowardsPlayer()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg -90f;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        tentacleRotationPoint.rotation = Quaternion.RotateTowards(tentacleRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);
    }
}
