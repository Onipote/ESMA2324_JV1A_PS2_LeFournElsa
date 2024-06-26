using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyRockBehaviour : MonoBehaviour
{
    public static CrazyRockBehaviour instance;

    [Header("Base's behaviour")]
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float rockPerSec;
    private float timeUntilFire;

    [Header("Rock ammunition")]
    [SerializeField] private Rigidbody2D rbCR;
    public GameObject rockPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float speed;
    public float currentDamage;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        //Rotate
        this.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        timeUntilFire += Time.deltaTime;

        //Shoot
        if (timeUntilFire >= 1f / rockPerSec)
        {
            Shoot();
            timeUntilFire = 0f;
        }
    }

    private void Shoot()
    {
        //Create a new rock from prefab at spawnpoint
        Instantiate(rockPrefab, spawnPoint.position, spawnPoint.transform.rotation);
        rbCR.velocity = spawnPoint.right * speed;
        currentDamage = UnityEngine.Random.Range(1, 15);
    }
}
