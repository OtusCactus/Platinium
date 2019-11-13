using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    public float wallFriction = 15;
    public float wallBouncyFriction = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Changement de face en fonction de la face actuelle et des faces proches.
    public int WallFaceChange(int[] wallOrientFaceChangeTab, int currentFace)
    {
        int nextface;

        nextface = wallOrientFaceChangeTab[currentFace];
        
        return nextface;
    }
    public void Bounce(Vector3 playerVelocity, Vector3 collisionNormal, Rigidbody2D playerRb, float playerSpeed, float myFriction)
    {
        Vector3 direction = Vector3.Reflect(playerVelocity.normalized, collisionNormal);
        playerRb.velocity = new Vector3(direction.x, direction.y).normalized * ((playerVelocity.magnitude / myFriction) * playerSpeed);
    }
    public void StickyWall(Rigidbody2D _myRb)
    {
        _myRb.velocity = Vector3.zero;
    }
    public GameObject SetConnectedWallRight(GameObject thisWall)
    {
        switch (thisWall.name)
        {
            case "WallNorthEast":
                for (int i = 0; i < thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthWest")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallNorthWest":
                for (int i = 0; i < thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthWest")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallSouthWest":
                for (int i = 0; i < thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouth")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallSouth":
                for (int i = 0; i < thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthEast")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallSouthEast":
                for (int i = 0; i < thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthEast")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
        }
        return null;
    }

    public GameObject SetConnectedLeftWall(GameObject thisWall)
    {
        switch (thisWall.name)
        {
            case "WallNorthEast":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthEast")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallNorthWest":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthEast")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallSouthWest":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthWest")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallSouth":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthWest")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
            case "WallSouthEast":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouth")
                    {
                        return thisWall.transform.parent.GetChild(i).gameObject;
                    }
                }
                break;
        }
        return null;
    }


    //A CHANGER, METTRE LES SCRIPTS EN PARAMETRES DIRECTEMENT
    public void ConnectedRightWall(float myDammage, GameObject thisWall, WallProprieties rightWallProprieties, WallChange rightWallChange)
    {
        if (!rightWallProprieties.isIndestructible)
        {
            rightWallChange.SetDammageFromConnect(myDammage);
        }
    }

    public void ConnectedLeftWall(float myDammage, GameObject thisWall, WallProprieties leftWallProprieties, WallChange leftWallChange)
    {
        if (!leftWallProprieties.isIndestructible)
        {
            leftWallChange.SetDammageFromConnect(myDammage);
        }
    }
}
