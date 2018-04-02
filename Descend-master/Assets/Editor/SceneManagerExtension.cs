using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SceneManager))]
public class SceneManagerExtension : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SceneManager mytarget = (SceneManager)target;



        if (GUILayout.Button("Clear This Scene's Files"))
        {
            mytarget.DeleteFilesOfThisScene();
        }
    }
}
