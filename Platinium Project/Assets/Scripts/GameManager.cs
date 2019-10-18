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
    public int[] _wallNorthEastTab;
    public int[] _wallNorthWestTab;
    public int[] _wallSouthEastTab;
    public int[] _wallSouthTab;
    public int[] _wallSouthWestTab;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
