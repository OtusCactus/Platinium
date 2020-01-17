using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLogoMenu : MonoBehaviour
{
    public GameObject pressA;
    private MenuPlayerManager _menuManagerScript;

    // Start is called before the first frame update
    void Awake()
    {
        _menuManagerScript = GameObject.FindWithTag("GameController").GetComponent<MenuPlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowPressA()
    {
        pressA.SetActive(true);
    }

    public void PressAIsHere()
    {
        _menuManagerScript.SetIsPressA(true);
    }
}
