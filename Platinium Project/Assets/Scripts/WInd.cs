using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{

    public GameObject gameManager;
    private GameManager gameManagerScript;
    public List<Rigidbody2D> playersRb;

    public Vector2 direction;
    public float windForce;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();

    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameManagerScript.playerList.Count; i++)
        {
            playersRb.Add(gameManagerScript.playerList[i].gameObject.GetComponent<Rigidbody2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Rigidbody2D playersObjRb in playersRb)
        {
            playersObjRb.velocity -= direction * Time.deltaTime * windForce;
        }
    }
}
