using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetMenuInformation : MonoBehaviour
{
    //Grégoire à fait ce script
    public int numbersOfPlayers;
    private bool[] playerMouvementMode;

    private void Awake()
    {
        //permet de garder cet objet lors de la transition du menu à la scène de jeu
        DontDestroyOnLoad(this.gameObject);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        numbersOfPlayers = 4;
        playerMouvementMode = new bool[4];
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(playerMouvementMode[0]);
    }

    //permet de changer le nombre de joeuur qui seront présent dans la scène de jeu
    public void GetPlayerNumbers(float sliderValue)
    {
        numbersOfPlayers = (int) sliderValue;
    }

    public void setPlayerMouvementMode(int boolNumber, bool state)
    {
        playerMouvementMode[boolNumber] = state;
    }

    public bool[] getPlayerMouvementMode()
    {
        return playerMouvementMode;
    }

}
