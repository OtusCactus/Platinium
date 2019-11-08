using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public int numbersOfPlayers;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        numbersOfPlayers = 4;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPlayerNumbers(float sliderValue)
    {
        numbersOfPlayers = (int) sliderValue;
    }
}
