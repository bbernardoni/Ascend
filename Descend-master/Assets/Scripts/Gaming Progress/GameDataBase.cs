using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameDataBase
{
    /// <summary>
    /// Initialize the specified parameters. Serve as a replace ment to constructors for GameDataBase 
    /// class, as they are deserialized, only the empty constructor is invoked;
    /// 
    /// Override this method to perform your own construction
    /// </summary>
    /// <returns>The initialize.</returns>
    /// <param name="parameters">Parameters.</param>
    public virtual void Initialize(params object[] parameters)
    {
        
    }

    public virtual void InitializeForTheFirstTime(params object[] parameters)
    {

    }
}

/// <summary>
/// Check point data base.
/// </summary>
[Serializable]
public class CheckPointDataBase : GameDataBase
{

    int latestCheckPointIndex = 0;

    /// <summary>
    /// Gets or sets the index of the latest check point.
    /// </summary>
    /// <value>The index of the latest check point.</value>
    public int LatestCheckPointIndex
    {
        get
        {
            return latestCheckPointIndex;
        }
        set
        {
            latestCheckPointIndex = value;
        }
    }

    [NonSerialized]
    string[] checkPointNames;

    /// <summary>
    /// Gets the name of the latest check point.
    /// </summary>
    /// <value>The name of the latest check point.</value>
    public string LatestCheckPointName
    {
        get
        {
            return checkPointNames[latestCheckPointIndex];
        }
    }

    public bool IsEmpty
    {
        get 
        {
            if (checkPointNames.Length == 0)
                return true;
            return false;
        }
    }

    public CheckPointDataBase()
    {
    }

    /// <summary>
    /// Initialize the CheckPoint data base with an array of game objects of the check point
    /// </summary>
    /// <returns>Only the array of game object check points.</returns>
    /// <param name="parameters">Parameters.</param>
    public override void Initialize(params object[] parameters)
    {
        base.Initialize(parameters);
        checkPointNames = CheckPointsToStrings((GameObject[])parameters[0]);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CheckPointDataBase"/> class.
    /// </summary>
    /// <param name="checkPoints">Check points.</param>
    public CheckPointDataBase(GameObject[] checkPoints)
    {
        checkPointNames = CheckPointsToStrings(checkPoints);
    }

    /// <summary>
    /// Sets the check points.
    /// </summary>
    /// <param name="checkPoints">Check points.</param>
    public void SetCheckPoints(GameObject[] checkPoints)
    {
        checkPointNames = CheckPointsToStrings(checkPoints);
    }

    /// <summary>
    /// Updates the check point.
    /// </summary>
    /// <param name="checkPoint">Check point.</param>
    public void UpdateCheckPoint(int checkPoint)
    {
        if (checkPoint >= 0 && checkPoint <= checkPointNames.Length - 1)
        {
            latestCheckPointIndex = checkPoint;
        }
        else
        {
            throw new IndexOutOfRangeException("The new check point does not exist in the data base!");
        }
    }

    /// <summary>
    /// Updates the check point.
    /// </summary>
    /// <param name="checkPoint">Check point.</param>
    public void UpdateCheckPoint(string checkPoint)
    {
        UpdateCheckPoint(Array.IndexOf(checkPointNames, checkPoint));
    }

    /// <summary>
    /// Checks the points to strings.
    /// </summary>
    /// <returns>The points to strings.</returns>
    /// <param name="checkPoints">Check points.</param>
    private string[] CheckPointsToStrings(GameObject[] checkPoints)
    {
        string[] output = new string[checkPoints.Length];
        for (int i = 0; i < checkPoints.Length; i++)
        {
            output[i] = checkPoints[i].gameObject.name;
        }
        return output;
    }
}

/// <summary>
/// Enemy data base.
/// </summary>
[Serializable]
public class EnemyDataBase : GameDataBase, IEnumerable
{
    List<string> enemiesToDisable;

    /// <summary>
    /// Initializes a new instance of the <see cref="T:EnemyDataBase"/> class.
    /// </summary>
    public EnemyDataBase()
    {
        enemiesToDisable = new List<string>();
    }

    /// <summary>
    /// Adds the dead enemy.
    /// </summary>
    /// <param name="enemy">Enemy.</param>
    public void AddDeadEnemy(string enemy)
    {
        if (!enemiesToDisable.Contains(enemy)) 
        {
            enemiesToDisable.Add(enemy);
        }

    }

    public IEnumerator GetEnumerator()
    {
        foreach (string name in enemiesToDisable)
        {
            yield return name;
        }
    }

    /// <summary>
    /// Add the specified enemy.
    /// </summary>
    /// <returns>The add.</returns>
    /// <param name="enemy">Enemy.</param>
    public void Add(object enemy)
    {
        enemiesToDisable.Add(enemy.ToString());
    }
}

/// <summary>
/// Player data base.
/// </summary>
[Serializable]
public class PlayerDataBase : GameDataBase
{

    const int inventorySize = 2;

    private object[] inventory;

    /// <summary>
    /// Gets or sets the health.
    /// </summary>
    /// <value>The health.</value>
    public int Health { get; set; }

    /// <summary>
    /// Gets or sets the fuel.
    /// </summary>
    /// <value>The fuel.</value>
    public int Fuel { get; set; }

    /// <summary>
    /// Gets or sets the inventory.
    /// </summary>
    /// <value>The inventory.</value>
    public object[] Inventory 
    {
        get 
        {
            return inventory;
        }
        set 
        {
            inventory = value;
        }
    }

    /// <summary>
    /// Sets the inventory item.
    /// </summary>
    /// <param name="index">Index, [0,1] as the inventory only has two slots</param>
    /// <param name="item">Item.</param>
    public void SetInventoryItem(int index, object item) 
    {
        if (index >= 0 && index < inventorySize)
        {
            inventory[index] = item;
            return;
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:PlayerDataBase"/> class.
    /// </summary>
    public PlayerDataBase()
    {
        //inventory = new object[inventorySize];
    }

    /// <summary>
    /// Initialize the specified parameters.
    /// </summary>
    /// <param name="parameters">Parameters: the size of the inventory</param>
    public override void Initialize(params object[] parameters)
    {
        base.Initialize(parameters);

        int size = (int)parameters[0];
        inventory = new object[size];
    }
        

}

[Serializable]
public class EnvironmentItem
{
    public string Name { get; set; }
    
    public SerializableVector3 Position { get; set; }
    public SerializableQuaternion Rotation { get; set; }

    public EnvironmentItem(string name, Vector3 position, Quaternion rotation)
    {
        this.Name = name;
        this.Position = new SerializableVector3(position);
        this.Rotation = new SerializableQuaternion(rotation);
    }

    public EnvironmentItem()
    {
        this.Name = "";
        this.Position = new SerializableVector3(0.0f, 0.0f, 0.0f);
        this.Rotation = new SerializableQuaternion(0.0f, 0.0f, 0.0f, 0.0f);
    }
}

[Serializable]
public class EnvironmentDataBase : GameDataBase
{
    private List<EnvironmentItem> _items;

    ///<summary>
    ///Environment items stored
    ///</summary>
    public List<EnvironmentItem> Items 
    {
        get 
        {
            return _items;
        }
    }

    public void UpdateItem(GameObject item)
    {
        foreach (var i in Items)
        {
            if (i.Name == item.name)
            {
                Debug.Log(item.transform.position);
                i.Position = new SerializableVector3(item.transform.position);
                i.Rotation = new SerializableQuaternion(item.transform.rotation);
                return;
            }
        }

        EnvironmentItem seriralizableItem = new EnvironmentItem(item.name, 
        item.transform.position, item.transform.rotation);

        Items.Add(seriralizableItem);
    }

    public EnvironmentDataBase() 
    {
        _items = new List<EnvironmentItem>();
    }

    /*
    public override void Initialize(params object[] parameters)
    {
        GameObject[] _elevators = (GameObject[])parameters;
        foreach (var _elevator in _elevators)
        {
            _triggerItems.Add(new EnvrionmentTriggerItem(_elevator.name, _elevator.transform.position, _elevator.transform.rotation));
        }
    }
    */
}