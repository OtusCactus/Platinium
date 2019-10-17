using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{

    public int _wallLife;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int WallFaceChange(int[] wallOrientFaceChangeTab, int currentFace)
    {
        int nextface;
        //wallOrientFaceChangeTab = new int[12];
        //for(int i = 0; i < wallOrientFaceChangeTab.Length; i++)
        //{
        //    wallOrientFaceChangeTab[i] = i;
        //}
        if (currentFace != 0)
        {
            nextface = wallOrientFaceChangeTab[currentFace - 1];

        }
        else
        {
            nextface = wallOrientFaceChangeTab[currentFace];
        }
        return nextface;
    }
}
