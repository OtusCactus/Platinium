using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public int wallLife;
    public float wallLimitVelocity;
    public float wallMinVelocity;

    private WallManager wallManagerScript;

    public float _playerVelocity;

    private LineRenderer line;
    public float xradius;
    public float yradius;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wallLife <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<LineRenderer>().enabled = false;
        }
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
            CreatePoints();
            wallLife -= 1;
        }
    }

    void CreatePoints()
    {
        float x;
        float y;
        float z = 0f;
        int segments = 25;

        float angle = 20f;
        line.SetVertexCount(segments + 1);
        line.useWorldSpace = false;

        for (int i = 0; i < ((segments + 1)/2); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * xradius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition(i, new Vector3(x, y, z));

            angle += (360f / segments/2);
        }
    }

}
