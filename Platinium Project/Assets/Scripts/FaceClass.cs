using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClass : MonoBehaviour
{

    //Grégoire s'est occupé de ce script

    //permet de stocker différentes variables propres à chaques faces

    [System.Serializable]
    public class face
    {
        public int faceNumber;
        public Transform cameraPosition;
        public Vector3 spriteRotation;
    }

    public face[] faceTab;

}
