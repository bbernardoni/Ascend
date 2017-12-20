using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//For serialization
using System.Xml.Serialization;

public interface IDataBase
{
    void SaveTo();
    void LoadFrom();
}

public class SceneManager : MonoBehaviour
{
    private CheckPointDataBase checkPointsManager;
    private EnemyDataBase enemiesManager;
    private PlayerDataBase playerManager;
    private EnvironmentDataBase environmentManager;
    private GameBundle mainBundle;
    private GameFolder sceneFolder;
    private GameFolder playerFolder;

    public string SceneNameOnDisk;
    public string CheckPointFileNameOnDisk;
    public string EnemyFileNameOnDisk;
    public string PlayerFileNameOnDisk;
    public string EnvironmentFileOnDisk;

    public GameObject Player;
    public GameObject[] CheckPoints;


	// Use this for initialization
    void Awake() 
    {
        enemiesManager = new EnemyDataBase();

        //Create folder for the scene on disk
        mainBundle = GameBundle.MainBundle;
        sceneFolder = mainBundle.OpenSpecificSceneFolder(SceneNameOnDisk);
        playerFolder = mainBundle.PlayerFolder;

        checkPointsManager = (CheckPointDataBase)sceneFolder.DeserializeDataBase<CheckPointDataBase>(CheckPointFileNameOnDisk);
        checkPointsManager.SetCheckPoints(CheckPoints);
        enemiesManager = (EnemyDataBase)sceneFolder.DeserializeDataBase<EnemyDataBase>(EnemyFileNameOnDisk);
        playerManager = (PlayerDataBase)playerFolder.DeserializeDataBase<PlayerDataBase>(PlayerFileNameOnDisk, 2); //2 is the inventory size
        environmentManager = (EnvironmentDataBase)sceneFolder.DeserializeDataBase<EnvironmentDataBase>(EnvironmentFileOnDisk);

    }
	void Start()
	{
        Reposition();
        foreach (string enemy in enemiesManager)
        {
            GameObject.Find(enemy).SetActive(false);
        }

        foreach (EnvironmentItem item in environmentManager.Items) 
        {
            GameObject obj = GameObject.Find(item.Name);
            obj.transform.position = item.Position.ToVector3();
            obj.transform.rotation = item.Rotation.ToQuaternion();

        }

        //checkPointsManager.UpdateCheckPoint(1);
        //Reposition();

	}
    void Reposition()
    {
        Player.transform.position = this.LatestCheckPoint;
        //Debug.Log(this.LatestCheckPoint.ToString());
    }

	// Update is called once per frame
	void Update()
	{
			
	}

    void OnDestroy() 
    {
        Save();
    }

    ///<summary>
    ///Save current scene
    ///</summary>
    public void Save() 
    {
        sceneFolder.SerializeDataBase(checkPointsManager, CheckPointFileNameOnDisk);
        sceneFolder.SerializeDataBase(enemiesManager, EnemyFileNameOnDisk);
        playerFolder.SerializeDataBase(playerManager, PlayerFileNameOnDisk);
        sceneFolder.SerializeDataBase(environmentManager, EnvironmentFileOnDisk);
    }

    /// <summary>
    /// Registers an enemy to be dead
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void RegisterDeadEnemy(string enemy) 
    {
        enemiesManager.AddDeadEnemy(enemy);
    }

    /// <summary>
    /// Registers the dead enemy.
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void RegisterDeadEnemy(GameObject enemy)
    {
        enemiesManager.AddDeadEnemy(enemy.name);
    }

    /// <summary>
    /// Registers the dead enemy.
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void RegisterDeadEnemy(MonoBehaviour enemy)
    {
        enemiesManager.AddDeadEnemy(enemy.gameObject.name);
    }

    /// <summary>
    /// Registers the latest check point
    /// </summary>
    /// <param name="checkPoint">Check point.</param>
    public void UpdateCheckPoint(int checkPoint)
    {
        checkPointsManager.UpdateCheckPoint(checkPoint);
        Save();
    }

    public void UpdateCheckPoint(GameObject checkPoint)
    {
        checkPointsManager.UpdateCheckPoint(checkPoint.name);
        Save();
    }

    public void UpdateCheckPoint(MonoBehaviour checkPoint)
    {
        checkPointsManager.UpdateCheckPoint(checkPoint.gameObject.name);
        Save();
    }

    /// <summary>
    /// Gets the latest check point.
    /// </summary>
    /// <value>The latest check point.</value>
    public Vector3 LatestCheckPoint
    {
        get
        {
            string objName = checkPointsManager.LatestCheckPointName;
            GameObject checkPoint = GameObject.Find(objName);
            //Debug.Log(checkPoint.transform.position.ToString());
            return checkPoint.transform.position; 
        }
    }

    /// <summary>
    /// Deletes the files of this scene.
    /// </summary>
    public void DeleteFilesOfThisScene()
    {
        sceneFolder.DeleteAllContent();
        Debug.Log("Files Cleared");
    }

}
