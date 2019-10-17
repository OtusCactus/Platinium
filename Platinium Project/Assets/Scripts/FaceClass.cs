using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClass : MonoBehaviour
{
    [System.Serializable]
    public class face
    {
        public int faceNumber;
        public Transform cameraPosition;
        public Vector3 spriteRotation;
    }

    public face[] faceTab;

}
