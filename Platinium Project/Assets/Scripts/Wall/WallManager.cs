using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    //Grégoire s'est occupé de ce script

    public float wallFriction = 15;
    public float wallBouncyFriction = 5;

    //Wall Apperance
    [Header("All Walls Appearance")]
    public Mesh[] wallNormalAppearance;
    public Mesh[] wallNormalShadowAppearance;
    public Mesh[] wallIndestructibleAppearance;
    public Mesh[] wallIndestructibleShadowAppearance;
    public Mesh[] wallBouncyAppearance;
    public Mesh[] wallBouncyShadowAppearance;

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
    public GameObject SetConnectedWall(GameObject thisWall)
    {
        for (int i = 0; i < thisWall.transform.parent.childCount; i++)
        {
            if (thisWall.transform.parent.GetChild(i).GetComponent<WallProprieties>().isConnected && (thisWall.transform.parent.GetChild(i).name != thisWall.name))
            {
                return thisWall.transform.parent.GetChild(i).gameObject;
            }
        }
        return null;
    }   
    public void ConnectedtWallDammage(float myDammage, GameObject thisWall, WallProprieties connectedWallProprieties, WallChange connectedWallChange)
    {
        if (!connectedWallProprieties.isIndestructible)
        {
            connectedWallChange.SetDammageFromConnect(myDammage);
        }
    }
    public void UpdateWallAppearance(Mesh[] theWallAppearance, Mesh[] theWallShadowAppearance, WallProprieties wallProprieties)
    {
        if (wallProprieties.isBouncy)
        {
            theWallAppearance = new Mesh[wallBouncyAppearance.Length];
            theWallShadowAppearance = new Mesh[wallBouncyShadowAppearance.Length];
            for (int i = 0; i < theWallAppearance.Length; i++)
            {
                theWallAppearance[i] = wallBouncyAppearance[i];
                theWallShadowAppearance[i] = wallBouncyShadowAppearance[i];
            }
        }
        else if (wallProprieties.isIndestructible)
        {
            theWallAppearance = new Mesh[wallIndestructibleAppearance.Length];
            theWallShadowAppearance = new Mesh[wallIndestructibleShadowAppearance.Length];
            print("length : " + wallIndestructibleAppearance.Length);
            print("he dezksjnl");
            for (int i = 0; i < theWallAppearance.Length; i++)
            {
                theWallAppearance[i] = wallIndestructibleAppearance[i];
                theWallShadowAppearance[i] = wallIndestructibleShadowAppearance[i];
            }
        }
        else
        {
            theWallAppearance = new Mesh[wallNormalAppearance.Length];
            theWallShadowAppearance = new Mesh[wallNormalShadowAppearance.Length];
            for (int i = 0; i < theWallAppearance.Length; i++)
            {
                theWallAppearance[i] = wallNormalAppearance[i];
                theWallShadowAppearance[i] = wallNormalShadowAppearance[i];
            }
        }
    }

    public Mesh[] SetWallAppearance(WallProprieties wallProprieties)
    {
        if (wallProprieties.isBouncy)
        {
            return wallBouncyAppearance;
        }
        else if (wallProprieties.isIndestructible)
        {
            return wallIndestructibleAppearance;
        }
        else
        {
            return wallNormalAppearance;
        }
    }

    public void WhichWall(WallProprieties wallProprieties) { 
        if (wallProprieties.isBouncy)
        {
            wallProprieties.theWalls[0].SetActive(false);
            wallProprieties.theWalls[1].SetActive(false);
        }
        else if (wallProprieties.isIndestructible)
        {
            wallProprieties.theWalls[0].SetActive(false);
            wallProprieties.theWalls[2].SetActive(false);
        }
        else
        {
            wallProprieties.theWalls[1].SetActive(false);
            wallProprieties.theWalls[2].SetActive(false);
        }
    }
}
