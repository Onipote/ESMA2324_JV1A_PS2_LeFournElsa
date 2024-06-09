using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingInfo : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<Info>().DisplayInfo("Press Mouse 0 to do a light attack");
    }
}
