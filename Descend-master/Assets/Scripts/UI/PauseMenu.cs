using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    public static bool isPaused = false;
    public GameObject pauseMenu; //reference to menu
    public GameObject settingsMenu; //reference to settings
    
    [SerializeField]
    private SceneSaver _sceneSaver;
    
    [SerializeField]
    private string _firstLevelName;

    public Slider volSlider;
    public AudioSource bgMusic;

    //resume game function
    public void Resume()
    {
        //bring down menu
        pauseMenu.SetActive(false);
        //time back to normal
        Time.timeScale = 1f;
        isPaused = false;
    }

    //pause game
    void Pause()
    {
        //bring up menu
        pauseMenu.SetActive(true);
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

    public void EnterSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void ExitSettings()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) {
                Pause();
            } else if(settingsMenu.activeSelf) {
                ExitSettings();
            } else {
                Resume();
            }
        }

        //change when slider changed
        bgMusic.volume = volSlider.value;
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
        SceneSaver.DeleteAllData();
        SceneManager.LoadScene(_firstLevelName);
    }

}
