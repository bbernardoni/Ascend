using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
    public GameObject menu; //reference to menu
    public GameObject settingsMenu; //reference to settings
	
    //resume game function
    public void Resume()
    {
        //bring down menu
        menu.SetActive(false);
        //time back to normal
        Time.timeScale = 1f;
        isPaused = false;
    }

    //pause game
    void Pause()
    {
        //bring up menu
        menu.SetActive(true);
        //freeze time
        Time.timeScale = 0f;
        isPaused = true;
    }

    //quit game
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && !settingsMenu.activeSelf)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
	}

    public void BeginNewGame() 
    {
        // Debug.Log("Delete files");
        // SceneManager manager = null;
        // manager = MonoBehaviour.FindObjectOfType<SceneManager>();
        // if (manager != null) 
        // {
        //     manager.DeleteAllContent();
        // }
        // else 
        // {
        //     Debug.LogWarning("No \"SceneManager\" Found!");
        // }
    }

}
