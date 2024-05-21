using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedLight : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public Transform cursedLightPoint;
    public LineRenderer lr;
    Transform transf;

    private void Awake()
    {
        transf = GetComponent<Transform>();
    }


    private void Update()
    {
        ShootLight();
    }
    void ShootLight()
    {
        if (Physics2D.Raycast(transf.position, transform.right))
        {
            RaycastHit2D _hit = Physics2D.Raycast(transf.position, transform.right);
            Draw2DRay(cursedLightPoint.position, _hit.point);
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
