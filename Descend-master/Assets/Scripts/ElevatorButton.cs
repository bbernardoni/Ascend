using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : Interactable {


    public Transform elevator;
    public float elevatorDistance;
    public float elevatorRate;

    public ElevatorButton()
    {
        InteractableType = TYPE.ENVIRONMENT;
        inUse = false;
        beingHeld = false;
    }

    override public void function(GameObject Player)
    {
        //if (!inUse)
        //{
            StartCoroutine(moveElevator());
        //}
    }

    IEnumerator moveElevator()
    {
        //inUse = true;
        float distance = 0;
        while(distance < elevatorDistance)
        {
            Debug.Log("Moving elevator...");
            elevator.Translate(Vector2.up * elevatorRate);
            distance += Mathf.Abs(elevatorRate);
            yield return new WaitForSeconds(Mathf.Abs(elevatorRate));
        }
        //inUse = false;
    }
}