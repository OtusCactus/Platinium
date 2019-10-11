using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouvPlayer2 : MonoBehaviour
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

    public float timerDeadPointX;
    public float timerDeadPointY;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();
        timerDeadPointX = 0;
        timerDeadPointY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        accelerationX = Input.GetAxis("Horizontal");
        accelerationY = Input.GetAxis("Vertical");

        //on définit la puissance du déplacement. Plus le joueur reste incliné, plus le timer et donc la puissance augmente
        if (accelerationX != 0)
        {
            prevAccX = accelerationX;
            myRb.drag = 3;
            timerPowerX += Time.deltaTime;
            if(timerPowerX > powerMax)
            {
                timerPowerX = powerMax;
            }
        }
        if (accelerationY != 0)
        {
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
            myRb.velocity = new Vector2(prevAccX * (-timerPowerX * speed), prevAccY * (-timerPowerY * speed));
            prevAccX = 0;
            prevAccY = 0;
            timerPowerX = 0;
            timerPowerY = 0;
            timerDeadPointX = 0;
            timerDeadPointY = 0;
        }
    }
}
