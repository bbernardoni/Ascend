using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneSaver))]
public class SceneSaverEditor: Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        SceneSaver saver = (SceneSaver)this.target;
        
        if (GUILayout.Button("Clear Data")) 
        {
            saver.DeleteSceneData();
        }
    }
}