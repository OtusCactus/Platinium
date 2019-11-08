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
    public void Bounce(Vector3 playerVelocity, Vector3 collisionNormal, Rigidbody2D _myRb, float playerSpeed, float myFriction)
    {
        Vector3 direction = Vector3.Reflect(playerVelocity.normalized, collisionNormal);
        _myRb.velocity = new Vector3(direction.x, direction.y).normalized * ((playerVelocity.magnitude / myFriction) * playerSpeed);
    }
    public void StickyWall(Rigidbody2D _myRb)
    {
        _myRb.velocity = Vector3.zero;
    }
    public void ConnectedRightWall(float myDammage, GameObject thisWall)
    {
        GameObject rightWall = null;
        switch (thisWall.name)
        {
            case "WallNorthEast":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthWest")
                    {
                        rightWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!rightWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    rightWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallNorthWest":
                for(int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if(thisWall.transform.parent.GetChild(i).name == "WallSouthWest")
                    {
                        rightWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!rightWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    rightWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallSouthWest":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "South")
                    {
                        rightWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!rightWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    rightWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallSouth":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthEast")
                    {
                        rightWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!rightWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    rightWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallSouthEast":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthEast")
                    {
                        rightWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!rightWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    rightWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
        }
    }

    public void ConnectedLeftWall(float myDammage, GameObject thisWall)
    {
        GameObject leftWall = null;
        switch (thisWall.name)
        {
            case "WallNorthEast":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthEast")
                    {
                        leftWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!leftWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    leftWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallNorthWest":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthEast")
                    {
                        leftWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!leftWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    leftWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallSouthWest":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallNorthWest")
                    {
                        leftWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!leftWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    leftWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallSouth":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouthWest")
                    {
                        leftWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!leftWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    leftWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
            case "WallSouthEast":
                for (int i = 0; i <= thisWall.transform.parent.childCount; i++)
                {
                    if (thisWall.transform.parent.GetChild(i).name == "WallSouth")
                    {
                        leftWall = thisWall.transform.parent.GetChild(i).gameObject;
                        break;
                    }
                }
                if (!leftWall.GetComponent<WallProprieties>().isIndestructible)
                {
                    leftWall.GetComponent<WallChange>().SetDammageFromConnect(myDammage);
                }
                break;
        }
    }
}
