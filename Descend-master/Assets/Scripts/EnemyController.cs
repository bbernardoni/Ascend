using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    private float currentMoveForce = 0;
    private bool facingRight = true;
    public float moveForce;
    public float maxSpeed;
    public float sightDistance;
    public GameObject eye;
    private GameObject player;

    public Rigidbody2D rb2d;

    private float moveTime = -1;
    
    private bool alerted = false;

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void FixedUpdate() {
        // Check for player
        Vector2 direction = (facingRight ? Vector2.right : Vector2.left);
        Debug.DrawRay(eye.transform.position, direction * sightDistance);
        RaycastHit2D hit = Physics2D.Raycast(eye.transform.position, direction, sightDistance, 1 << LayerMask.NameToLayer("Player"));
        if(hit) {
            alerted = true;
            player = hit.collider.gameObject;
        }

        if(alerted) {
            // Chase player
            Vector3 toPlayer = player.transform.position - transform.position;

            if(Mathf.Abs(toPlayer.x) > sightDistance)
                alerted = false;

            if(Mathf.Abs(toPlayer.x) < 0.1)
                currentMoveForce = 0;
            else if(toPlayer.x < 0)
                currentMoveForce = -moveForce;
            else
                currentMoveForce = moveForce;
        }
        else {
            // Idle movement
            if(moveTime < 0) {
                // new movement
                moveTime = Random.Range(1.0f, 2.0f);
                currentMoveForce = Random.Range(-1, 2) * moveForce;
            }

            moveTime -= Time.deltaTime;
        }

        // move
        rb2d.AddForce(Vector2.right * currentMoveForce);

        // clamp to max speed
        if(Mathf.Abs(rb2d.velocity.x) > maxSpeed)
            rb2d.velocity = new Vector2(Mathf.Sign(rb2d.velocity.x) * maxSpeed, rb2d.velocity.y);

        // set facing right
        if(rb2d.velocity.x > 0.01f) {
            facingRight = true;
            //transform.localScale = new Vector3(1, 1, 1);
        }
        else if(rb2d.velocity.x < -0.01f) {
            facingRight = false;
            //transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Kill()
    {
        Destroy(gameObject); //Add death animation later
    }
}
