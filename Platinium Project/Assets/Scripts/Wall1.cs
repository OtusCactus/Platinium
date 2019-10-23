using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall1 : MonoBehaviour
{
    //Même script que Wall mais sans tous les mouvements de caméra pour que je puisse travailler sur ma scène sans rajouter tous les assets nécessaires
    //et Grégoire peut utiliser le script Wall sur sa scène
    //
    //Ce script n'est plus utilisé car nous avons maintenant des murs 3D, mais je le garde juste au cas, et notamment pour garder la mécanique de Line Renderer

    //Propriétés du mur
    [Header("Propriétés")]
    public int wallLifeMax;
    public float wallLimitVelocity;
    public int wallLife;

    private float _playerVelocityRatio;

    [Header("Apparence")]
    public Material[] wallAppearance;

    //Variables pour les murs en LineRenderer, pour l'instant inutile, mais on les garde au cas où
    /* private LineRenderer line;
     public float xradius;
     public float yradius;
     public Transform[] points;*/

    // Start is called before the first frame update
    void Start()
    {
        wallLife = wallLifeMax;
        GetComponent<MeshRenderer>().material = wallAppearance[0];
        //line = GetComponent<LineRenderer>();
        //line.positionCount = points.Length;
    }

    // Update is called once per frame
    void Update()
    {        
        if (wallLife <= 0)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            //GetComponent<LineRenderer>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
        }
        if (wallLife < wallLifeMax)
        {
            GetComponent<MeshRenderer>().material = wallAppearance[1];
        }
        if (wallLife == 1)
        {
            print("hello");
            GetComponent<MeshRenderer>().material = wallAppearance[2];
        }
        //_UpdateWall();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        _playerVelocityRatio = collision.GetComponent<PlayerEntity>().GetVelocityRatio();
        if (_playerVelocityRatio >= wallLimitVelocity)
        {
            wallLife = 0;
        }
        else if (_playerVelocityRatio < wallLimitVelocity)
        {
            wallLife -= 1;
        }
    }
    
    /*private void _UpdateWall()
    {
        for (int i = 0; i < points.Length; i++)
        {
            line.SetPosition(i, points[i].position);
        }
    }*/
   

}
