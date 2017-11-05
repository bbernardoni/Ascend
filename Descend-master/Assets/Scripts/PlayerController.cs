using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public bool grounded = false;
    public Animator anim;
    
    //ladder
    public bool onLadder;
    public float climbSpeed;
    private float climbVelocity;
    private float gravityStore;

    private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        gravityStore = rb2d.gravityScale;
	}
	
	// Update is called once per frame
	void Update () {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //grounded = true;
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if (Other.gameObject.CompareTag("Interactable") && Input.GetKeyDown(KeyCode.E))
        {
            Other.GetComponent<Interactable>().function(gameObject);
        }
    }

    void FixedUpdate()
    {
        //handle movement
        float hMove = Input.GetAxis("Horizontal");

        if (hMove * rb2d.velocity.x < maxSpeed)
            rb2d.AddForce(Vector2.right * hMove * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        if (hMove > 0 && !facingRight)
            Flip();
        else if (hMove < 0 && facingRight)
            Flip();

        if (jump)
        {
            anim.Play("jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        } else if (!grounded)
        {
            anim.Play("jump");
        }
        else if (facingRight)
        {
            anim.Play("right");
        } else if (!facingRight)
        {
            anim.Play("left");
        }
         
        if (onLadder) {
            //set g = 0
            rb2d.gravityScale = 0f;
            climbVelocity = climbSpeed*Input.GetAxisRaw("Vertical");
            rb2d.velocity = new Vector2(rb2d.velocity.x, climbVelocity);

            anim.Play("climb");
        }

        if (!onLadder) {
            //set g back to original value
            rb2d.gravityScale = gravityStore;
        }

    }

    void Flip()
    {
        facingRight = !facingRight;
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }

    
}
