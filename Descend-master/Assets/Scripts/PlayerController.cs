using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, ISavable {
    // movement 
    public float moveForce;
    public float maxSpeed;
    public float jumpForce;
    public Transform groundCheck;
    public Animator anim;

    private bool grounded = false;
    private bool facingRight = true;
    private bool jump = false;

    // intactables
    private bool holdingBox = false;
    private bool overBarrel = false;
    private int overInteractables = 0;

    //player death
    private bool dying = false;
    public float deathTime;
    public Image fader;
    private float deathTimer;
    public SceneSaver sceneSaver;

    //ladder
    private bool onLadder;
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
        if(dying)
        {
            deathTimer -= Time.deltaTime;
            fader.color = new Color(0, 0, 0, 1-deathTimer/deathTime);
            if(deathTimer <= 0.0f)
            {
                sceneSaver.Load();
                fader.color = new Color(0, 0, 0, 0);
            }
            return;
        }

        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        //grounded = true;
        if (Input.GetButtonDown("Jump") && grounded && !holdingBox)
        {
            jump = true;
        }
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(dying) return;

        if(Other.CompareTag("Oil Barrel")) {
            overBarrel = true;
        }
        else if(Other.CompareTag("Interactable"))
        {
            //Debug.Log("Player Trigger Enter: "+Other.name);
            Other.GetComponent<Interactable>().inTrigger = true;
            overInteractables++;
        }
        else if(Other.CompareTag("Ladder")) {
            onLadder = true;
            //set g = 0
            rb2d.gravityScale = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if(dying) return;

        if(Other.CompareTag("Oil Barrel")) {
            overBarrel = false;
        }
        else if(Other.CompareTag("Interactable") && !Other.transform.IsChildOf(transform))
        {
            //Debug.Log("Player Trigger Exit: "+Other.name);
            Other.GetComponent<Interactable>().inTrigger = false;
            overInteractables--;
        }
        else if(Other.CompareTag("Ladder")) {
            onLadder = false;
            //set g back to original value
            rb2d.gravityScale = gravityStore;
        }
    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if(dying) return;
        if(Other.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("STUN!");
            Other.GetComponent<EnemyController>().SetStunned(true);
        }
    }

    void OnCollisionEnter2D(Collision2D coll) {
        //Debug.Log("Collided with " + coll.gameObject.ToString());
    }

    void FixedUpdate()
    {
        if(dying)
        {
            rb2d.velocity = Vector2.zero;
            return;
        }

        //handle movement
        float hMove = Input.GetAxis("Horizontal");
        float curMaxSpeed = maxSpeed;

        if(holdingBox)
            curMaxSpeed /= 2.0f;

        if (hMove * rb2d.velocity.x < curMaxSpeed)
            rb2d.AddForce(Vector2.right * hMove * moveForce);

        if (Mathf.Abs(rb2d.velocity.x) > curMaxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * curMaxSpeed, rb2d.velocity.y);

        if (hMove > 0 && !facingRight)
            Flip();
        else if (hMove < 0 && facingRight)
            Flip();

        if(onLadder) {
            climbVelocity = climbSpeed*Input.GetAxisRaw("Vertical");
            rb2d.velocity = new Vector2(rb2d.velocity.x, climbVelocity);

            anim.Play("climb");
        }
        else if (jump)
        {
            anim.Play("jump");
            rb2d.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
        else if (!grounded){
            anim.Play("jump");
        }
        else if (facingRight){
            anim.Play("right");
        }
        else if (!facingRight){
            anim.Play("left");
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        //Vector3 theScale = transform.localScale;
        //theScale.x *= -1;
        //transform.localScale = theScale;
    }

    public void Kill()
    {
        dying = true;
        deathTimer = deathTime;
    }

    public void SetHoldingBox(bool holdingBox) {
        this.holdingBox = holdingBox;
    }

    public bool GetOverBarrel() {
        return overBarrel;
    }

    public int GetOverInteractables() {
        return overInteractables;
    }

    public bool GetFacingRight() {
        return facingRight;
    }

    public void OnSave(ISavableWriteStore store)
    {
        store.WriteVector3("pos", rb2d.position);
    }

    public void OnLoad(ISavableReadStore store)
    {
        facingRight = true;
        jump = false;
        holdingBox = false;
        overBarrel = false;
        // overInteractables

        dying = false;
        onLadder = false;

        rb2d.position = store.ReadVector3("pos");
        rb2d.velocity = Vector2.zero;
    }
}
