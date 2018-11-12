using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public bool facingRight = true;
    public float moveForce = 600f;
    public float maxSpeed = 5f;
    public float health = 100;
    public float sightDistance = 100f;
    public float stunDuration = 3f;
    public GameObject eye;
    public GameObject player;
    public GameObject stunAnimation;
    public bool stunned = false;

    public Rigidbody2D rb2d;
    public Slider healthSlider;

    private float moveTime = 0;
    private float leftRight = 0;
    private float lastMove = 0;

    private float stunTime = 0;

    private bool idleMoving = false;
    public bool alerted = false;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        stunCheck();
        if (!stunned)
        {
            playerCheck();
            healthSlider.value = health;
            if (!idleMoving && !alerted)
            {
                StartCoroutine(idleMove());
                idleMoving = true;
            }
            if(rb2d.velocity.x > 0)
            {
                facingRight = true;
                transform.localScale = new Vector3(1, 1, 1);
            } else if(rb2d.velocity.x < 0)
            {
                facingRight = false;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }
	}

    void FixedUpdate()
    {
        if (alerted && !stunned)
        {
            Chase(player);
        }
    }

    void playerCheck()
    {
        if (facingRight)
        {
            Debug.DrawRay(eye.transform.position, Vector3.right * sightDistance);
            RaycastHit2D hit = Physics2D.Raycast(eye.transform.position, Vector3.right, sightDistance, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                Debug.Log("Ray hit!");
                alerted = true;
                player = hit.collider.gameObject;
            } else
            {
                Debug.Log("Ray miss.");
            }
        }
        else
        {
            Debug.DrawRay(eye.transform.position, Vector3.left * sightDistance);
            RaycastHit2D hit = Physics2D.Raycast(eye.transform.position, Vector3.left, sightDistance, 1 << LayerMask.NameToLayer("Player"));
            if (hit)
            {
                Debug.Log("Ray hit!");
                alerted = true;
                player = hit.collider.gameObject;
            }
            else
            {
                Debug.Log("Ray miss.");
            }
        }
    }

    void Chase(GameObject player)
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        float direction = 0;
        if(Mathf.Abs(toPlayer.x) < 0.1)
        {
            return;
        } else if(Mathf.Abs(toPlayer.x) > sightDistance){
            alerted = false;
            return;
        }
        if(toPlayer.x < 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        rb2d.velocity = (new Vector2(direction, rb2d.velocity.y) * moveForce / 200);
        Debug.Log("Chasing... Force = " + (direction*moveForce*0.1f));
    }

    IEnumerator idleMove()
    {
        Debug.Log("idleMoving");
        lastMove = Time.time;
        moveTime = Random.Range(1,2);
        int direction = Random.Range(-1, 2);
        while(direction == 0)
        {
            direction = Random.Range(-1, 2);
        }
        Debug.Log(direction);
        while((Time.time - lastMove) < moveTime)
        {
            rb2d.velocity = (new Vector2(direction, rb2d.velocity.y) * (moveForce / Random.Range(100,300)));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
        idleMoving = false;
    }

    void takeDamage(float f)
    {
        if(health - f < 0)
        {
            health = 0;
            Kill();
        }
        else
        {
            health -= f;
        }
    }

    void moveRandom()
    {
        if(moveTime <= 0)
        {
            moveTime = Random.Range(1,3);
            leftRight = Random.Range(0,2) - 1;
            lastMove = Time.time;
        }
        if(leftRight > 0)
        {
            rb2d.AddForce(Vector2.right * moveForce);
        }
        else
        {
            rb2d.AddForce(Vector2.left * moveForce);
        }

        moveTime -= Time.time - lastMove;
        lastMove = Time.time;
    }

    public void stun()
    {
        stunTime = Time.time;
        stunned = true;
    }

    void stunCheck()
    {
        if (stunned && stunTime + stunDuration < Time.time)
        {
            stunned = false;
        }
        if (stunned)
        {
            stunAnimation.SetActive(true);
        }
        else
        {
            stunAnimation.SetActive(false);
        }
    }

    public void Kill()
    {
        Destroy(gameObject); //Add animation later
    }
}
