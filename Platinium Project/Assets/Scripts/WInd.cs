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

    private GameObject _particule;
    private float _angle;
    private Vector3 startParticuleRotation;

    private void Awake()
    {
        gameManagerScript = gameManager.GetComponent<GameManager>();
        _particule = transform.GetChild(0).gameObject;
        startParticuleRotation = _particule.transform.eulerAngles;
    }


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < gameManagerScript.playerList.Count; i++)
        {
            playersRb.Add(gameManagerScript.playerList[i].gameObject.GetComponent<Rigidbody2D>());
        }
        Quaternion rotation = Quaternion.LookRotation(-direction, Vector3.up);
        _particule.transform.rotation = rotation;
        _particule.transform.eulerAngles = new Vector3(_particule.transform.eulerAngles.x, _particule.transform.eulerAngles.y, 90);
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
