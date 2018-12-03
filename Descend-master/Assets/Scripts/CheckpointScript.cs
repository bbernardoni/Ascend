using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour, ISavable {

    public SceneSaver sceneSaver;
    private bool activated = false;
    
    void Start (){
    }

    void OnTriggerEnter2D(Collider2D Other)
    {
        if(!activated && Other.gameObject.CompareTag("Player"))
        {
            activated = true;
            Vector3 oldPos = Other.transform.position;
            Other.transform.position = transform.position;
            sceneSaver.Save();
            Debug.Log("Checkpoint Activated");
            Other.transform.position = oldPos;
        }
    }

    public void OnSave(ISavableWriteStore store)
    {
        store.WriteBool("activated", activated);
    }

    public void OnLoad(ISavableReadStore store)
    {
        activated = store.ReadBool("activated");
    }
}
