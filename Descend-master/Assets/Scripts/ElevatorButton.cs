using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : Interactable {


    public Rigidbody2D elevator;
    public float elevatorDistance;
    public float elevatorRate;

    private Vector2 up;
    private Vector2 down;

    private void Start() {
        up = elevator.position;
        down = elevator.position + Vector2.down * elevatorDistance;
    }

    override public void function() {
        inUse = true;
    }

    void FixedUpdate() {
        if(inUse) {
            if(elevator.position.y > down.y) {
                Vector2 position = elevator.position;
                position.y += elevatorRate * Time.fixedDeltaTime;
                elevator.MovePosition(position);
            }
            else {
                elevator.position = down;
                inUse = false;
            }
        }
    }
}