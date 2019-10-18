using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Changement de face
    public int WallFaceChange(int[] wallOrientFaceChangeTab, int currentFace)
    {
        int nextface;
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
