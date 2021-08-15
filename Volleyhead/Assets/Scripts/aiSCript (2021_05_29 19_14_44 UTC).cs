using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiSCript : MonoBehaviour
{
    
    private float groundY = -2.77f;
    public float speed = 10.0f;
    public float jumpSpeed = 10.0f;
    public float netLevel = -2.92f;
    public float leftWall = -7.5f;
    private float prevVelX = -1f;
    private bool prevLandOnMySide = false;
    private float landingX = -4.7f;

    public GameObject ball;

    public string leftKey = "a";
    public string rightKey = "d";
    public string jumpKey = "w";

    public Rigidbody2D rb;
    public bool canJump;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
        Vector2 siz = transform.localScale;
        if(LandOnMySide()) {
            Vector2 loc = transform.position;
            loc.x = landingX;
            transform.position = loc;
            //siz.x += 0.01f;
            //transform.localScale = siz;
            Debug.Log(landingX);
        }else {
            //siz.x -= 0.01f;
            //transform.localScale = siz;
        }
        
    }

    bool LandOnMySide() {
        /*solve for quadratic.
        first, the acceleration eqn is: 
        a(t) = -1;
        find v(t) by doing derivitive but in reverse.
        v(t) = -1t + c (+c because derivitive if a constant is 0)
        now find s(t) by doing derivitive but in reverse.
        s(t) = -(1 / 2)(t ^ 2) + ct + d (+d because serivitive of a constant is 0)

        now if you find the deritive if s(t) it is v(t) and same from v(t) to a(t)...

        in s(t), c will be initial velocity (i got this info from v(t))
        in s(t), d will be initial height

        now teh equation is targetY = s(t)
        which becomes 0 = s(t) - targetY, solve using quad formula, take the higher of the two times. because that one will more likely on your side.
        
        whne you find t such that the ball will be at targetY height (on top of the head of the player),
        use t, current X pos, and X velocity (xVleloity does not change),
        this wil find the X value of landing spot given targetY.
        */
        Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
        float velX = ballRB.velocity.x;
        float velY = ballRB.velocity.y;

        if(prevVelX == velX) return prevLandOnMySide;

        float g = -1.0f;
        float currX = ball.transform.position.x;
        float currY = ball.transform.position.y;
        float targetY = netLevel;
        float targetX; // variable.

        //float t = 

        float t = Math.Max(((-1 * velY) + Mathf.Sqrt((velY * velY) - 4 * (g / 2) * (currY - targetY))) / (2 * (g / 2)), ((-1 * velY) - Mathf.Sqrt((velY * velY) - 4 * (g / 2) * (currY - targetY))) / (2 * (g / 2)));
        targetX = currX + (t * currX);
        if(targetX < 0) prevLandOnMySide = true;
        else prevLandOnMySide = false;
        
        if(prevLandOnMySide) landingX = targetX;
        if(landingX < leftWall) landingX += Math.Abs(leftWall - landingX);
        return prevLandOnMySide;
    }

    bool canReachBall() {
        return false;
    }
    
}
