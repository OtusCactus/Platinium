using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall1 : MonoBehaviour
{
    public int wallLife;
    public float wallLimitVelocity;
    public float wallMinVelocity;

    //differents scripts 
    private WallManager _wallManagerScript;
    private GameManager _gameManagerScript;
    private CameraMouvements _cameraMouvementsScript;

    private int _currentFace;
    private int _nextFace;

    public float _playerVelocity;

    private LineRenderer line;
    public float xradius;
    public float yradius;

    public Transform[] points;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = points.Length;
    }

    // Update is called once per frame
    void Update()
    {        
        if (wallLife <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
        }
        _UpdateWall();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        _playerVelocity = collision.GetComponent<Rigidbody2D>().velocity.sqrMagnitude;
        if (_playerVelocity >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocity >= wallMinVelocity && _playerVelocity < wallLimitVelocity)
        {
            //CreatePoints();
            //WallElasticity();
            wallLife -= 1;
        }
    }
    
    private void _UpdateWall()
    {
        for (int i = 0; i < points.Length; i++)
        {
            line.SetPosition(i, points[i].position);
        }
    }


    /*void WallElasticity()
    {
        Vector3 firstPointPosition = line.GetPosition(0);
        Vector3 lastPointPosition = line.GetPosition(1);

        float x;
        float y;
        float z = 0f;
        int segments = 12;

        float angle = 180/segments;
        line.SetVertexCount(segments);

        for (int i = 0; i < (segments); i++)
        {
            if (i == 0)
            {
                line.SetPosition(i, firstPointPosition);
            }
            else if (i == segments - 1)
            {
                line.SetPosition(i, lastPointPosition);
            }
            else
            {
                x = (line.GetPosition(i - 1).x) + Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
                y = (line.GetPosition(i - 1).y) + Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

                line.SetPosition(i, new Vector3(x, y, z));

                angle -= (180f / segments - 2);
            }
        }

        line.SetPosition(0, firstPointPosition);
        line.SetPosition((segments - 1), lastPointPosition);
    }*/
   

}
