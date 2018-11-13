using System.Xml;
using UnityEngine;

public class SavableTest : MonoBehaviour, ISavable
{
    private Transform elev;
    private Transform start;
    private Transform end;
    private bool up;

    public SceneSaver ss;

    public string ContainerElementTag
    {
        get { return "Elevator"; }
    }
    
    public void OnSave(ISavableWriteStore store)
    {
        store.WriteBool("isUp", up);
    }
    
    public void OnLoad(ISavableReadStore store)
    {
        up = store.ReadBool("isUp");
    }

    void Awake()
    {
        elev = transform.Find("Sample Elevator");
        start = transform.Find("Start Position");
        end = transform.Find("End Position");
        up = true;
    }

    void Update()
    {
        // elevator operation
        if(Input.GetKeyDown(KeyCode.Space))
            up = !up;

        if(up)
            elev.position = start.position;
        else
            elev.position = end.position;

        // SceneSaver test code
        if(Input.GetKeyDown(KeyCode.S))
        {
            ss.Save();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            ss.Load();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            SceneSaver.DeleteAllData();
        }
    }
}