using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomArena : MonoBehaviour
{
    [Header ("Bibliothèques de LD")]
    public List<string> normalLD;
    public List<string> bouncyLD;
    public List<string> indestructibleLD;

    private GameManager _gameManagerScript;

    // Start is called before the first frame update
    void Awake()
    {
        _gameManagerScript = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManagerScript.currentFace == 0 || _gameManagerScript.currentFace == 7 || _gameManagerScript.currentFace == 8 || _gameManagerScript.currentFace == 9)
        {
            print(normalLD[Random.Range(0, normalLD.Count)]);
        }
        else if (_gameManagerScript.currentFace == 1 || _gameManagerScript.currentFace == 2 || _gameManagerScript.currentFace == 3 || _gameManagerScript.currentFace == 5)
        {
            print(bouncyLD[Random.Range(0, bouncyLD.Count)]);
        }
        else
        {
            print(indestructibleLD[Random.Range(0, indestructibleLD.Count)]);
        }
    }
}
