using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLookCamera : MonoBehaviour
{
    //Matilde a fait ce script
    //Ce script permet de faire en sorte que les objets regardent la caméra
    //On le garde dans l'optique de faire des sprites à la mode de "Don't Starve", où les sprites sont toujours face à la caméra

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
