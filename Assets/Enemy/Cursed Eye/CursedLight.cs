using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedLight : MonoBehaviour
{
    public static CursedLight instance;

    [Header("Payer's info")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform eyesCaveRespawn;
    public bool isBurnt = false;

    [Header("Cursed light settings")]
    [SerializeField] private float defDistanceRay = 100;
    public LineRenderer lr;
    public Transform cursedLightPoint;
    Transform m_transform;

    [Header("Rotation")]
    [SerializeField] private Rigidbody2D eye;
    [SerializeField] private float rotationSpeed = 45f;
    private bool isGoingRight = true;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();

        if (instance == null)
        {
            instance = this;
        }
    }

    private void Update()
    {
        ShootLight();
        ChangeDirection();
    }

    void ShootLight()
    {
        RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, transform.right);
        
        if (_hit.collider.CompareTag("PointA"))
        {
            isGoingRight = true;
        }
        else if (_hit.collider.CompareTag("PointB"))
        {
            isGoingRight = false;
        }


        if (_hit)
        {
            isBurnt = true;
            Draw2DRay(cursedLightPoint.position, _hit.point); //ray between initial pos and hit point

            if (_hit.collider.CompareTag("Player")) //effect on player if he's touched by the ray
            {
                PlayerHealth player = _hit.collider.gameObject.GetComponent<PlayerHealth>();
                player.TakeDamage(Random.Range(20,50));
                isBurnt = false;
            }
        }
        else
        {
            Draw2DRay(cursedLightPoint.position, cursedLightPoint.transform.right * defDistanceRay);
            isBurnt = false;
        }
    }

    void ChangeDirection()
    {
        if (isGoingRight)
        {
            this.transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else
        {
            this.transform.Rotate(Vector3.back, rotationSpeed * Time.deltaTime);
        }
    }
    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }
}
