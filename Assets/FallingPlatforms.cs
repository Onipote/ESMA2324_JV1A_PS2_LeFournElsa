using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatforms : MonoBehaviour
{
    [Header("FallingPlatforms")]
    [SerializeField] private Rigidbody2D fallingPlatforms;
    private float fallDelay = 0.5f;
    private float destroyDelay = 5f;

    private void Start()
    {
        fallingPlatforms = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke("DropPlatform", fallDelay);
            Destroy(gameObject, destroyDelay);
        }
    }

    void DropPlatform()
    {
        fallingPlatforms.isKinematic = false;
    }
}
