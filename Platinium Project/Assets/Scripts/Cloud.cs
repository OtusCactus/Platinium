using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cloud : MonoBehaviour
{
    public float speed = 2;
    private RectTransform _myTransform;

    // Start is called before the first frame update
    void Start()
    {
        _myTransform = gameObject.GetComponent<Image>().rectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        _myTransform.anchoredPosition += Vector2.left * speed;
    }
}
