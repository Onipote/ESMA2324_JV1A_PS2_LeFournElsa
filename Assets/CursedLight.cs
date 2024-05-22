using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedLight : MonoBehaviour
{
    [Header("Payer's info")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform eyesCaveRespawn;

    [Header("Cursed light settings")]
    [SerializeField] private float defDistanceRay = 100;
    Vector3 currentEulerAngles;
    float z;
    public float rotationSpeed;
    public LineRenderer lr;
    public Transform cursedLightPoint;
    Transform m_transform;
    private bool autoRotate = false;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void Update()
    {
        ShootLight();

        currentEulerAngles += new Vector3(0, 0, z) * Time.deltaTime * rotationSpeed;
        transform.localEulerAngles = currentEulerAngles;
    }

    public void TurnX()
    {
        z = -1;
    }
    void ShootLight()
    {
        RaycastHit2D _hit = Physics2D.Raycast(m_transform.position, transform.right);
        if (_hit)
        {
            Draw2DRay(cursedLightPoint.position, _hit.point);
            if (_hit.collider.CompareTag("Player"))
            {
                PlayerMovements.instance.rb.transform.position = eyesCaveRespawn.transform.position;
                Debug.Log("Ugh.. My ears are burning !!");
            }
        }
        else
        {
            Draw2DRay(cursedLightPoint.position, cursedLightPoint.transform.right * defDistanceRay);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        lr.SetPosition(0, startPos);
        lr.SetPosition(1, endPos);
    }
}
