using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{

    public float ejectionPower;
    public float shaderAppearanceTime = 1;
    private GameManager _gameManagerScript;
    private int nbrPlayers;

    [Header("Wall Life")]
    public float wallLifeMaxTwo = 2;
    public float wallLifeMaxThree = 50;
    public float wallLifeMaxFour = 200;

    [Header ("Frictions")]
    public float wallFriction = 15;
    public float wallBouncyFriction = 5;

    [Header ("Shake")]
    public float wallShakeMax = 1;
    public float wallShakeIntensity = 1;
    public float wallShakeTimerMax;

    //Wall Apperance
    [Header("All Walls Appearance")]
    public Mesh[] wallNormalAppearance;
    public Mesh[] wallNormalShadowAppearance;
    public Mesh[] wallIndestructibleAppearance;
    public Mesh[] wallIndestructibleShadowAppearance;
    public Mesh[] wallBouncyAppearance;
    public Mesh[] wallBouncyShadowAppearance;

    [Header("Bibliothèques de LD")]
    public RandomizerArena normalLD;
    public RandomizerArena bouncyLD;
    public RandomizerArena indestructibleLD;
    private RandomizerArena _thisRoundBibli;
    private int _arenaIndex;

    // Start is called before the first frame update
    void Awake()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        nbrPlayers = _gameManagerScript.playerList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Définit la vie du mur selon le nombre de joueur
    public float GetWallLifeMax()
    {
        switch (nbrPlayers)
        {
            case 2:
                return wallLifeMaxTwo;
            case 3:
                return wallLifeMaxThree;
            case 4:
                return wallLifeMaxFour;
            default:
                return wallLifeMaxFour;
        }
    }
    //Changement de face en fonction de la face actuelle et des faces proches.
    public int WallFaceChange(int[] wallOrientFaceChangeTab, int currentFace)
    {
        int nextface;

        nextface = wallOrientFaceChangeTab[currentFace];
        
        return nextface;
    }
    //definit la puissance du rebond du joueur
    public void Bounce(Vector3 playerVelocity, Vector3 collisionNormal, Rigidbody2D playerRb, float playerSpeed, float myFriction)
    {
        Vector3 direction = Vector3.Reflect(playerVelocity.normalized, collisionNormal);
        playerRb.velocity = new Vector3(direction.x, direction.y).normalized * ((playerVelocity.magnitude / myFriction) * playerSpeed);
        //playerRb.transform.eulerAngles = new Vector3(playerRb.transform.eulerAngles.x, playerRb.transform.eulerAngles.y, Vector3.Angle(playerVelocity.normalized, playerRb.velocity.normalized));
        
        if (playerRb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(playerRb.velocity.y, playerRb.velocity.x) * Mathf.Rad2Deg;
            playerRb.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        }

    }
    //non utilisé
    public void StickyWall(Rigidbody2D _myRb)
    {
        _myRb.velocity = Vector3.zero;
    }
    //définit les murs connectés
    public GameObject SetConnectedWall(GameObject thisWall)
    {
        for (int i = 0; i < thisWall.transform.parent.childCount; i++)
        {
            if (thisWall.transform.parent.GetChild(i).GetComponent<WallProprieties>().GetIsConnected() && (thisWall.transform.parent.GetChild(i).name != thisWall.name))
            {
                return thisWall.transform.parent.GetChild(i).gameObject;
            }
        }
        return null;
    }

    //gère les dommages des murs connectés
    public void ConnectedtWallDammage(float myDammage, GameObject thisWall, WallProprieties connectedWallProprieties, WallChange connectedWallChange)
    {
 
        if (!connectedWallProprieties.GetIsIndestructible())
        {
            connectedWallChange.SetDammageFromConnect(myDammage);
        }
    }
    //définit l'apparence du mur selon sa propriété
    public Mesh[] UpdateWallAppearance(WallProprieties wallProprieties)
    {
        if (wallProprieties.GetIsBouncy())
        {
            Mesh[] tempWallAppearance = new Mesh[wallBouncyAppearance.Length];
            for (int i = 0; i < wallBouncyAppearance.Length; i++)
            {
                tempWallAppearance[i] = wallBouncyAppearance[i];
            }
            return tempWallAppearance;
        }
        else if (wallProprieties.GetIsIndestructible())
        {
            Mesh[] tempWallAppearance = new Mesh[wallIndestructibleAppearance.Length];
            for (int i = 0; i < wallIndestructibleAppearance.Length; i++)
            {
                tempWallAppearance[i] = wallIndestructibleAppearance[i];
            }
            return tempWallAppearance;
        }
        else
        {
            Mesh[] tempWallAppearance = new Mesh[wallNormalAppearance.Length];
            for (int i = 0; i < wallNormalAppearance.Length; i++)
            {
                tempWallAppearance[i] = wallNormalAppearance[i];
            }
            return tempWallAppearance;
        }
    }
    //définit l'apparence de l'outline du mur selon sa propriété
    public Mesh[] UpdateWallShadowAppearance(WallProprieties wallProprieties)
    {
        if (wallProprieties.GetIsBouncy())
        {
            Mesh[] tempWallShadowAppearance = new Mesh[wallBouncyShadowAppearance.Length];
            for (int i = 0; i < wallBouncyShadowAppearance.Length; i++)
            {
                tempWallShadowAppearance[i] = wallBouncyShadowAppearance[i];
            }
            return tempWallShadowAppearance;
        }
        else if (wallProprieties.GetIsIndestructible())
        {
            Mesh[] tempWallShadowAppearance = new Mesh[wallIndestructibleShadowAppearance.Length];
            for (int i = 0; i < wallIndestructibleShadowAppearance.Length; i++)
            {
                tempWallShadowAppearance[i] = wallIndestructibleShadowAppearance[i];
            }
            return tempWallShadowAppearance;
        }
        else
        {
            Mesh[] tempWallShadowAppearance = new Mesh[wallNormalShadowAppearance.Length];
            for (int i = 0; i < wallNormalShadowAppearance.Length; i++)
            {
                tempWallShadowAppearance[i] = wallNormalShadowAppearance[i];
            }
            return tempWallShadowAppearance;
        }
    }
    //définit quel mur affiché selon sa propriété
    public void WhichWall(WallProprieties wallProprieties) {
        if (wallProprieties.GetIsBouncy())
        {
            wallProprieties.theWalls[2].SetActive(true);
        }
        else if (wallProprieties.GetIsIndestructible())
        {
            wallProprieties.theWalls[1].SetActive(true);
        }
        else
        {
            wallProprieties.theWalls[0].SetActive(true);
        }

        if(wallProprieties.GetIsConnected())
        {
            wallProprieties.theWalls[3].SetActive(true);
        }
    }
    //Choisit un ld au hasard selon la face
    public void GetRandomArena()
    {
        if (_gameManagerScript.currentFace == 0 || _gameManagerScript.currentFace == 7 || _gameManagerScript.currentFace == 8 || _gameManagerScript.currentFace == 9)
        {
            print("WALMAN curr face " + _gameManagerScript.currentFace);
            _thisRoundBibli = normalLD;
            _arenaIndex = Random.Range(0, normalLD.arenas.Count);
        }
        else if (_gameManagerScript.currentFace == 1 || _gameManagerScript.currentFace == 2 || _gameManagerScript.currentFace == 3 || _gameManagerScript.currentFace == 5)
        {
            print("WALMAN curr face " + _gameManagerScript.currentFace);
            _thisRoundBibli = bouncyLD;
            _arenaIndex = Random.Range(0, bouncyLD.arenas.Count);
        }
        else
        {
            print("WALMAN curr face " + _gameManagerScript.currentFace);
            _thisRoundBibli = indestructibleLD;
            _arenaIndex = Random.Range(0, indestructibleLD.arenas.Count);
        }
    }
    public int GetRandomArenaIndex()
    {
        return _arenaIndex;
    }
    public RandomizerArena GetThisRoundBibli()
    {
        return _thisRoundBibli;
    }
}
