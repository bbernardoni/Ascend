using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Serializable vector3, used for serialization of the position of Unity postions,
/// which cannot be serialied
/// </summary>
[Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// Gets or sets the x.
    /// </summary>
    /// <value>The x.</value>
    public float X { get; set; }

    /// <summary>
    /// Gets or sets the y.
    /// </summary>
    /// <value>The y.</value>
    public float Y { get; set; }

    /// <summary>
    /// Gets or sets the z.
    /// </summary>
    /// <value>The z.</value>
    public float Z { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SerializableVector3"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <param name="z">The z coordinate.</param>
    public SerializableVector3(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SerializableVector3"/> struct.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    public SerializableVector3(float x, float y) : this(x, y, 0)
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:SerializableVector3"/> struct.
    /// </summary>
    /// <param name="unityVector3">Unity vector3.</param>
    public SerializableVector3(Vector3 unityVector3) : this(unityVector3.x, unityVector3.y, unityVector3.z)
    {
        
    }

    public override string ToString()
    {
        return string.Format("[SerializableVector3: X={0}, Y={1}, Z={2}]", X, Y, Z);
    }

    /// <summary>
    /// Tos the vector3.
    /// </summary>
    /// <returns>The vector3.</returns>
    public Vector3 ToVector3() 
    {
        return new Vector3(this.X, this.Y, this.Z);
    }

}

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
    public bool ActionPerformed { get; set; }
    public SerializableVector3 Position { get; set; }
    public SerializableVector3 Rotation { get; set; }
}

public class EnvironmentDataBase : GameDataBase 
{
    public EnvironmentItem[] items;


}