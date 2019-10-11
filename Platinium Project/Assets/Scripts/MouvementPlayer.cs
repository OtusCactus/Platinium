using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvementPlayer : MonoBehaviour
{
    public float accelerationX;
    public float accelerationY;
    public float prevAccX;
    public float prevAccY;
    //
    public float timerPowerX;
    public float timerPowerY;
    public float powerMax;
    //
    public int speed;
    //
    private Rigidbody2D myRb;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        accelerationX = Input.GetAxis("Horizontal");
        accelerationY = Input.GetAxis("Vertical");

        if (accelerationX != 0)
        {
            prevAccX = accelerationX;
            myRb.drag = 3;
            if (accelerationX < 0)
            {
                timerPowerX -= Time.deltaTime;

                if (timerPowerX < -powerMax)
                {
                    timerPowerX = -powerMax;
                }
            }
            else
            {
                timerPowerX += Time.deltaTime;
                if (timerPowerX > powerMax)
                {
                    timerPowerX = powerMax;
                }
            }
        }
        if (accelerationY != 0)
        {
            prevAccY = accelerationY;
            myRb.drag = 2;
            if (accelerationY < 0)
            {
                timerPowerY -= Time.deltaTime;

                if (timerPowerY < -powerMax)
                {
                    timerPowerY = -powerMax;
                }
            }
            else
            {
                timerPowerY += Time.deltaTime;
                if (timerPowerY >powerMax)
                {
                    timerPowerY = powerMax;
                }
            }
        }
        
        if ((accelerationX == 0 && prevAccX !=0) || (accelerationY == 0 && prevAccY !=0))
        {
            myRb.velocity = new Vector2(0, 0);
            myRb.drag = 0;
            myRb.velocity = new Vector2((-timerPowerX * speed), (-timerPowerY * speed));
            prevAccX = 0;
            prevAccY = 0;
            timerPowerX = 0;
            timerPowerY = 0;
        }
    }
}
