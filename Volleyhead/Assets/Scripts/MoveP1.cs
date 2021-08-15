using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveP1 : MonoBehaviour
{
    [SerializeField] GameObject _dust;
    [SerializeField] GameObject _harmSlow;
    public GameObject clone;
    public GameObject powerClone; //clone for the powerup sprite

    public float speed = 5f;
    private float destroyTime = 0.5f;
    public float jumpSpeed = 5f;
    private float timeToPowerup = 0;

    public string leftKey = "a";
    public string rightKey = "d";
    public string jumpKey = "w";

    public Rigidbody2D rb;
    public bool canJump;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    //void Update()
    //{ //make a new script for power ups but for now leaf here
    //    if(timeToPowerup != 0)
    //    {
    //        timeToPowerup -= Time.deltaTime; //cooldown between each powerup
    //    }
    //    else
    //    {
    //        if(GameObject.Find("powerClone") != null) //if the powerup wasn't taken, it will be destroyed
    //        {
    //            Destroy(powerClone);
    //        }
    //        Vector2 _position = new Vector2(Random.Range(-6.7f, 6.5f), Random.Range(-1.8f, 3.7f)); //get a random position to spawn
    //        timeToPowerup = Random.Range(5f, 15f); //random wait time between spawns
    //        powerClone = Instantiate(_harmSlow, _position, _harmSlow.transform.rotation); //add it to game
    //    }
    //}

    void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (Input.GetKey(leftKey))
        {
            pos.x -= speed * Time.deltaTime;
            //clone = Instantiate(_dust, transform.position, 90.0f <-- idk wtf);
            //Destroy(clone, destroyTime);
        }
        if (Input.GetKey(rightKey))
        {
            pos.x += speed * Time.deltaTime;/*
            clone = Instantiate(_dust, transform.position, _dust.transform.rotation);
            Destroy(clone, destroyTime);*/
        }

        //jump
        if (canJump && Input.GetKey(jumpKey))
        {
            canJump = false;
            rb.velocity = Vector2.up * jumpSpeed;
        }

        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor") canJump = true;

    }
}