using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public MouvementPlayer[] allPlayers;

    public static PlayerManager Instance = null;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  //va vouloir ne pas detruire juste le script pas va détruire le gameobject si on met juste this
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string[] controllers = Input.GetJoystickNames();
        int j = 0;
        for (int i = 0; i < controllers.Length; i++)
        {
            if (controllers[i] != "")
            {
                allPlayers[j].controllerNumber = i + 1;
                print("je viens d'assigner la manette" + (i + 1) + " au joueur " + j);
                j++;
            }
        }
        if (j < allPlayers.Length)
        {
            Debug.Log("Il manque " + (allPlayers.Length - j) + " manettes");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
