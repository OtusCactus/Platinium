using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPointCreation : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    //Store les transform pour les différentes position de la caméra
    public Transform[] cameraPosition;

    private GameObject _cameraPointObj;
    public float diceCameraDistance;
    // Start is called before the first frame update
    void Awake()
    {
        //permet de set la distance des positions des caméras par rapport au centre de l'arène
        for (int i = 0; i < cameraPosition.Length; i++)
        {
            cameraPosition[i].transform.position = (cameraPosition[i].transform.position - transform.position).normalized * diceCameraDistance + transform.position;
        }
    }

}
