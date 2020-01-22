using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    private float speed = 2;
    private RectTransform _myTransform;
    public float minSpeed = 0.2f;
    public float maxSpeed = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = gameObject.GetComponent<Image>().rectTransform;
        speed = Random.Range(minSpeed, maxSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        _myTransform.anchoredPosition += Vector2.left * speed;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Destroy(this.gameObject);
    }
}
