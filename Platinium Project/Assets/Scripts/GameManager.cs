using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    

    ////all walls
    //[Header("Walls")]
    //public GameObject WallNorthEast;
    //public GameObject WallNorthWest;
    //public GameObject WallSouthWest;
    //public GameObject WallSouthEast;
    //public GameObject WallSouth;

    //tableaux contenant les prochaines faces selon la face où l'on est et selon le mur détruit.
    [Header("NextFaceArrays")]
    public int[] _wallNorthEastTab = new int[12] {2,9,2,3,1,10,8,3,3,9,7,8};
    public int[] _wallNorthWestTab = new int[12] {3,3,9,8,4,2,9,9,2,2,10,7};
    public int[] _wallSouthEastTab = new int[12] {4,1,4,12,12,1,10,7,10,6,6,11};
    public int[] _wallSouthTab = new int[12] {5,6,1,5,11,5,11,12,7,11,5,5 };
    public int[] _wallSouthWestTab = new int[12] {6,10,2,1,6,11,12,4,8,7,12,4};



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
