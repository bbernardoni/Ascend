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
    private GameBundle mainBundle;
    private GameFolder sceneFolder;

    public string SceneNameOnDisk;
    public string CheckPointFileNameOnDisk;
    public string EnemyFileNameOnDisk;
    public string PlayerFileNameOnDisk;

    public GameObject Player;
    public GameObject[] CheckPoints;

	// Use this for initialization
    void Awake() 
    {
        enemiesManager = new EnemyDataBase();

        //Create folder for the scene on disk
        mainBundle = GameBundle.MainBundle;
        sceneFolder = mainBundle.OpenSpecificSceneFolder(SceneNameOnDisk);

        checkPointsManager = (CheckPointDataBase)sceneFolder.DeserializeDataBase<CheckPointDataBase>(CheckPointFileNameOnDisk);
        checkPointsManager.SetCheckPoints(CheckPoints);
        enemiesManager = (EnemyDataBase)sceneFolder.DeserializeDataBase<EnemyDataBase>(EnemyFileNameOnDisk);
        playerManager = (PlayerDataBase)sceneFolder.DeserializeDataBase<PlayerDataBase>(PlayerFileNameOnDisk, 2);

    }
	void Start()
	{
        Reposition();
        foreach (string enemy in enemiesManager)
        {
            GameObject.Find(enemy).SetActive(false);
        }

        //checkPointsManager.UpdateCheckPoint(1);
        //Reposition();

	}
    void Reposition()
    {
        Player.transform.position = this.LatestCheckPoint;
        Debug.Log(this.LatestCheckPoint.ToString());
    }

	// Update is called once per frame
	void Update()
	{
			
	}

    void OnDestroy() 
    {
        sceneFolder.SerializeDataBase(checkPointsManager, CheckPointFileNameOnDisk);
        sceneFolder.SerializeDataBase(enemiesManager, EnemyFileNameOnDisk);
        sceneFolder.SerializeDataBase(playerManager, PlayerFileNameOnDisk);
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
    }

    public void UpdateCheckPoint(GameObject checkPoint)
    {
        checkPointsManager.UpdateCheckPoint(checkPoint.name);
    }

    public void UpdateCheckPoint(MonoBehaviour checkPoint)
    {
        checkPointsManager.UpdateCheckPoint(checkPoint.gameObject.name);
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
}
