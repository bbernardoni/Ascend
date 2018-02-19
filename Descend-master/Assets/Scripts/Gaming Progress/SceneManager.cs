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
    public GameObject[] EnvironmentTriggerItems;

    bool haveInitializedScene = false;


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
        environmentManager = (EnvironmentDataBase)sceneFolder.DeserializeDataBase<EnvironmentDataBase>(EnvironmentFileOnDisk, EnvironmentTriggerItems);

    }
    void Start()
    {


        //checkPointsManager.UpdateCheckPoint(1);
        //Reposition();

    }
    void DisableEnemies()
    {
        foreach (string enemy in enemiesManager)
        {
            GameObject.Find(enemy).SetActive(false);
        }

    }
    ///<summary>
    /// Move the player to its last saved position
    ///</summary>
    void RepositionPlayer()
    {
        Player.transform.position = this.LatestCheckPoint;
        //Debug.Log(this.LatestCheckPoint.ToString());
    }

    ///<summary>
    /// Move the environment items to its last saved position
    ///</summary>
    void RepositionEnvironmentItems()
    {

        //Debug.Log("Reposition Environment Item");

        foreach (EnvironmentItem item in environmentManager.Items)
        {
            //Debug.Log(String.Format("Reposition: {0}", item.Name));
            GameObject obj = GameObject.Find(item.Name);
            obj.transform.position = item.Position.ToVector3();
            obj.transform.rotation = item.Rotation.ToQuaternion();
            Debug.Log(String.Format("Reposition {0} to {1}", obj.name, obj.transform.position));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!haveInitializedScene)
        {
            RepositionPlayer();
            RepositionEnvironmentItems();
            DisableEnemies();
            haveInitializedScene = true;
        }
    }

    void OnDestroy()
    {
        Save();
    }

    ///<summary>
    /// Save current game to the disk.
    /// This method is automatically called in OnDestroy()
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
    /// <param name="enemy">The dead enemy</param>
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
    /// Gets the latest check point. If check point array is empty, return (0,0,0)
    /// </summary>
    /// <value>The latest check point.</value>
    public Vector3 LatestCheckPoint
    {
        get
        {
            if (!checkPointsManager.IsEmpty)
            {
                string objName = checkPointsManager.LatestCheckPointName;
                GameObject checkPoint = GameObject.Find(objName);
                //Debug.Log(checkPoint.transform.position.ToString());
                return checkPoint.transform.position;
            }
            else
            {
                return new Vector3(0.0f, 0.0f, 0.0f);
            }

        }
    }

    ///<summary>
    /// Update the location of an environment item, if it has been recorded before.
    /// If the item has not been recorded, a new one one will be inserted into the xml file.
    ///</summary>
    ///<param name="item">The environment item to be recorded</param>
    public void UpdateEnvironmentItem(GameObject item)
    {
        environmentManager.UpdateItem(item);
    }

    ///<summary>
    /// Update the location of an environment item, if it has been recorded before.
    /// If the item has not been recorded, a new one one will be inserted into the xml file.
    ///</summary>
    ///<param name="item">The environment item to be recorded</param>
    public void UpdateEnvironmentItem(MonoBehaviour item)
    {
        environmentManager.UpdateItem(item.gameObject);
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
