using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, ISavable {

    [HideInInspector] public bool facingRight = true;
    [HideInInspector] public bool jump = false;
    [HideInInspector] public bool holdingBox = false;

    public float moveForce = 365f;
    public float maxSpeed = 5f;
    public float jumpForce = 1000f;
    public Transform groundCheck;
    public bool grounded = false;
    public Animator anim;
    
    //player death
    private bool dying = false;
    public float deathTime = 2.0f;
    public Image fader;
    private float deathTimer;

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
        if(dying)
        {
            deathTimer -= Time.deltaTime;
            fader.color = new Color(0, 0, 0, 1-deathTimer/deathTime);
            if(deathTimer <= 0.0f)
            {
                Destroy(gameObject);
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
        if(Other.gameObject.CompareTag("Interactable"))
        {
            Other.GetComponent<Interactable>().inTrigger = true;
        }
    }

    void OnTriggerExit2D(Collider2D Other)
    {
        if(dying) return;
        if(Other.gameObject.CompareTag("Interactable") && !Other.transform.IsChildOf(transform))
        {
            Other.GetComponent<Interactable>().inTrigger = false;
        }
    }

    void OnTriggerStay2D(Collider2D Other)
    {
        if(dying) return;
        if(Other.gameObject.CompareTag("Enemy") && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("STUN!");
            Other.GetComponent<EnemyController>().stun();
        }
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

    public void Kill()
    {
        dying = true;
        deathTimer = deathTime;
    }

    public string ContainerElementTag
    {
        get { return gameObject.name; }
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

        dying = false;
        onLadder = false;

        rb2d.position = store.ReadVector3("pos");
        rb2d.velocity = Vector2.zero;
        fader.color = new Color(0, 0, 0, 0);
    }
}
