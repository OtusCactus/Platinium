using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceClassMenu : MonoBehaviour
{

    //Grégoire s'est occupé de ce script

    //permet de stocker différentes variables propres à chaques faces

    [System.Serializable]
    public class face
    {
        public int faceNumber;
        //public Transform cameraPosition;
        public Transform arenaRotation;
        public GameObject arenaWall;
        public Transform player1StartingPosition;
        //public MeshRenderer[] wallToHideNextToFace;
        //public MeshRenderer[] wallToHideInOtherFace;
    }

    public face[] faceTab;

}
