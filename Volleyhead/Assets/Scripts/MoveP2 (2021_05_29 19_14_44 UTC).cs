using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveP2 : MonoBehaviour
{

    [SerializeField] GameObject _dust;
    public GameObject clone;
    public float destroyTime = 0.5f;

    private float groundY = -4.15f;
    public float speed = 5f;
    public float jumpSpeed = 5f;

    public string leftKey = "left";
    public string rightKey = "right";
    public string jumpKey = "up";

    public Rigidbody2D rb;
    public bool canJump = true;

    private void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 pos = transform.position;

        if (Input.GetKey(leftKey))
        {
            pos.x -= speed * Time.deltaTime;

            //dust trail code.
            //clone = Instantiate(_dust, transform.position, 90.0f);
            //Destroy(clone, destroyTime);
        }
        if (Input.GetKey(rightKey))
        {
            pos.x += speed * Time.deltaTime;

            //dust trail code.
            //clone = Instantiate(_dust, transform.position, _dust.transform.rotation);
            //Destroy(clone, destroyTime);
        }

        //enable jump
        //if (pos.y < groundY) canJump = true;

        //jump
        if (canJump && Input.GetKey(jumpKey))
        {
            canJump = false;
            rb.velocity = Vector2.up * jumpSpeed;

            //small trail under the player, or something like very faint little explosion effect?
        }

        //apply changes
        transform.position = pos;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "floor") canJump = true;
    }
}
