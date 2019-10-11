using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouvementPlayer : MonoBehaviour
{
    private float accelerationX;
    private float accelerationY;
    private float prevAccX;
    private float prevAccY;
    //
    private float timerPowerX;
    private float timerPowerY;
    public float powerMax;
    //
    public int speed;
    //
    private Rigidbody2D myRb;

    private float timerDeadPointX;
    private float timerDeadPointY;

    public float rotationSpeed;
    private float joyAngle;

    private float angle;

    public Slider powerSlider;

    [HideInInspector] public int controllerNumber;

    private void Awake()
    {
        timerPowerX = 0;
        timerPowerY = 0;
        powerSlider.value = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        timerDeadPointX = 0;
        timerDeadPointY = 0;
        powerSlider.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        accelerationX = Input.GetAxis("HorizontalJoy" + controllerNumber);
        accelerationY = Input.GetAxis("VerticalJoy" + controllerNumber);

        float inputX = Input.GetAxis("HorizontalJoy" + controllerNumber);
        float inputY = -Input.GetAxis("VerticalJoy" + controllerNumber);

        if (inputX != 0.0f || inputY != 0.0f)
        {
            angle = Mathf.Atan2(inputX, inputY) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        //on définit la puissance du déplacement. Plus le joueur reste incliné, plus le timer et donc la puissance augmente
        if (accelerationX != 0)
        {
            powerSlider.gameObject.SetActive(true);

            prevAccX = accelerationX;
            myRb.drag = 3;
            timerPowerX += Time.deltaTime;
            if (timerPowerX > powerMax)
            {
                timerPowerX = powerMax;
            }
        }
        if (accelerationY != 0)
        {
            powerSlider.gameObject.SetActive(true);

            prevAccY = accelerationY;
            myRb.drag = 2;
            timerPowerY += Time.deltaTime;
            if (timerPowerY > powerMax)
            {
                timerPowerY = powerMax;
            }
        }
        //évite les "flottements" du perso si le joueur change de direction pendant la charge de puissance
        if (accelerationX == 0)
        {
            timerDeadPointX += Time.deltaTime;
        }
        else
        {
            timerDeadPointX = 0;
        }
        if (accelerationY == 0)
        {
            timerDeadPointY += Time.deltaTime;
        }
        else
        {
            timerDeadPointY = 0;
        }
        //détermine la vélocité selon les timer et la direction
        if ((timerDeadPointX >= 0.1 && timerDeadPointY >= 0.1) && ((accelerationX == 0 && prevAccX != 0) || (accelerationY == 0 && prevAccY != 0)))
        {
            myRb.velocity = new Vector2(0, 0);
            myRb.drag = 0;
            if (Mathf.Abs(timerPowerX) > Mathf.Abs(timerPowerY))
            {
                timerPowerY = timerPowerX;
            }
            else if (Mathf.Abs(timerPowerY) > Mathf.Abs(timerPowerX))
            {
                timerPowerX = timerPowerY;
            }
            powerSlider.gameObject.SetActive(false);

            myRb.velocity = new Vector2(prevAccX * (-timerPowerX * speed), prevAccY * (-timerPowerY * speed));
            prevAccX = 0;
            prevAccY = 0;
            timerPowerX = 0;
            timerPowerY = 0;
            timerDeadPointX = 0;
            timerDeadPointY = 0;
        }

        //controle la value du slider en fonction de la puissance engagée
        if (timerPowerX > timerPowerY)
        {
            powerSlider.value = timerPowerX / 5;
        }
        else
        {
            powerSlider.value = timerPowerY / 5;
        }
    }

}
