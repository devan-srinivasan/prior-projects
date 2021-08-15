using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public int p1Score, p2Score, turn;
    public int scoreLim = 7;
    private float p1DefPosX, p1DefPosY, p2DefPosX , p2DefPosY ;
    private float ballDefPosY = 2f;
    //public float groundY = -4.21f;
    private Rigidbody2D rb;
    public GameObject p1;
    public GameObject p2;
    public GameObject net;

    public bool BallOnGround;

    void Start()
    {
        Debug.Log("start");
        p1Score = 0;
        p2Score = 0;
        turn = 1;

        p1DefPosX = p1.transform.position.x;
        p1DefPosY = p1.transform.position.y;
        p2DefPosX = p2.transform.position.x;
        p2DefPosY = p2.transform.position.y;

        Vector2 pos = transform.position; //set ball pos to player 1 side (default)
        pos.x = p1DefPosX;
        transform.position = pos;
    }

    void FixedUpdate()
    {
        //case: 0: not on floor,      1: on player 1's ground      2: on player 2's ground
        int casee = BallGrounded(transform.position);
        if(casee == 1) {         //ball landed on p1 side.
            p2Score++;          //p2 gets the point
            turn = 2;           //p1 (loser)'s turn
        }else if(casee == 2) {
            p1Score++;
            turn = 1;
        }

        //a player lost, so reset turn!
        if(casee != 0) {
            ResetTurn();
        }

        ///to be continued.
        if (p1Score == scoreLim || p2Score == scoreLim)
        {
            Time.timeScale = 0; //this just stops the game or pauses it rather
            //add function to load winner screen, parameters: an int of 1 or 2 to say who won, and the score.
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        if (collision.gameObject.tag == "floor") {
            Debug.Log("END");
            Debug.Log(rb.position.x);
            BallOnGround = true;
        }
        else
        {
            Debug.Log("START");
            Debug.Log(rb.position.x);
            Debug.Log(rb.velocity.x);
            Debug.Log(rb.velocity.y);
        }
    }

    private int BallGrounded(Vector2 ballPos) {
        // if(y value at ground && x position on player1 or 2's side)...
        Vector2 netPos = net.transform.position;
        if (BallOnGround && ballPos.x < netPos.x ) return 1;
        else if(BallOnGround && ballPos.x > netPos.x ) return 2;
        else return 0;
    }

    private void ResetTurn()
    {
        //get vectors of the objects.
        Vector2 p1Position = p1.transform.position;
        Vector2 p2Position = p2.transform.position;
        Vector2 ballPosition = transform.position;
        rb = this.GetComponent<Rigidbody2D>();
        Rigidbody2D p1rb = p1.GetComponent<Rigidbody2D>();
        Rigidbody2D p2rb = p2.GetComponent<Rigidbody2D>();

        //reset player velocity
        p1rb.velocity = Vector2.zero;
        p2rb.velocity = Vector2.zero;

        //reset player position
        p1Position.x = p1DefPosX;
        p1Position.y = p1DefPosY;
        p2Position.x = p2DefPosX;
        p2Position.y = p2DefPosY;

        //reset ball position
        if(turn == 1) ballPosition.x = p1DefPosX;       //place ball on p1 side.
        else if(turn == 2) ballPosition.x = p2DefPosX;  //place ball on p2 side.
        ballPosition.y = ballDefPosY;

        //apply vectors to the actualy objects.
        p1.transform.position = p1Position;
        p2.transform.position = p2Position;
        transform.position = ballPosition;

        //reset ball velocity.
        rb.velocity = Vector2.zero;

        //reset ball
        BallOnGround = false;
    }
}
