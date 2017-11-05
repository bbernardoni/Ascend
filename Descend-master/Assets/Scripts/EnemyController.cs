using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public bool facingRight = true;
//<<<<<<< HEAD
    //public float moveForce = 10f;
    //public float maxSpeed = 5f;
    //public float health = 100;
//=======
    public float moveForce = 600f;
    public float maxSpeed = 5f;
    public float health = 100;
    public float sightDistance = 100f;
    public GameObject eye;
    public GameObject player;
//>>>>>>> refs/remotes/origin/Josh

    public Rigidbody2D rb2d;
    public Slider healthSlider;

    private float moveTime = 0;
    private float leftRight = 0;
    private float lastMove = 0;

    private bool idleMoving = false;
//<<<<<<< HEAD
//=======
    public bool alerted = false;
//>>>>>>> refs/remotes/origin/Josh

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
//<<<<<<< HEAD
        if (!idleMoving)
//=======
        if (!idleMoving && !alerted)
//>>>>>>> refs/remotes/origin/Josh
        {
            StartCoroutine(idleMove());
            idleMoving = true;
        }
//<<<<<<< HEAD
        healthSlider.value = health;
	}

    /* IEnumerator idleMove()
    {
        lastMove = Time.time;
        moveTime = Random.Range(1, 3);
        float direction = Random.Range(-1, 1);
        while((Time.time - lastMove) < moveTime)
        {
            rb2d.AddForce(Vector2.right * direction * moveForce);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(3);
//=======

        playerCheck();
        healthSlider.value = health;

        if(rb2d.velocity.x > 0)
        {
            facingRight = true;
            transform.localScale = new Vector3(1, 1, 1);
        }
        
        if(rb2d.velocity.x < 0)
        {
            facingRight = false;
            transform.localScale = new Vector3(-1, 1, 1);
        }
	} */

    void FixedUpdate()
    {
        if (alerted)
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
        rb2d.AddForce(new Vector3(direction, 0, 0) * moveForce * 0.1f);
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
            rb2d.AddForce(new Vector3(direction, 0, 0) * (moveForce / Random.Range(1,3)));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);
//>>>>>>> refs/remotes/origin/Josh

        idleMoving = false;
    }

    void takeDamage(float f)
    {
        if(health - f < 0)
        {
            health = 0;
            die();
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

    void die()
    {

    }
}
