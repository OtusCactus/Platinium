using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDirection : MonoBehaviour
{
    public float rotationSpeed;
    private float angle;
    private float joyAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("HorizontalJoy" + GetComponentInParent<MouvementPlayer>().controllerNumber);
        float inputY = -Input.GetAxis("VerticalJoy" + GetComponentInParent<MouvementPlayer>().controllerNumber);

        if (inputX != 0.0f || inputY != 0.0f)
        {
            angle = Mathf.Atan2(inputX, inputY) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
