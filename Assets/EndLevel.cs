using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<Info>().DisplayInfo("At least ! You will maybe find hapiness in the \"Real World\" !");
    }
}
