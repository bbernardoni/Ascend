using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameDataBase
{
    public virtual void Initialize(params object[] parameters)
    {
        
    }
}

[Serializable]
public class CheckPointDataBase : GameDataBase
{

    int latestCheckPointIndex = 0;

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

    public override void Initialize(params object[] parameters)
    {
        checkPointNames = CheckPointsToStrings((GameObject[])parameters[0]);
    }

    public CheckPointDataBase(GameObject[] checkPoints)
    {
        checkPointNames = CheckPointsToStrings(checkPoints);
    }

    public void SetCheckPoints(GameObject[] checkPoints)
    {
        checkPointNames = CheckPointsToStrings(checkPoints);
    }

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

    public void UpdateCheckPoint(string checkPoint)
    {
        UpdateCheckPoint(Array.IndexOf(checkPointNames, checkPoint));
    }

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

[Serializable]
public class EnemyDataBase : GameDataBase, IEnumerable
{
    List<string> enemiesToDisable;

    public EnemyDataBase()
    {
        enemiesToDisable = new List<string>();
    }

    public void AddDeadEnemy(string enemy)
    {
        if (!enemiesToDisable.Contains(enemy)) 
        {
            enemiesToDisable.Add(enemy);
        }

    }

    public void SaveTo(string fileName)
    {

    }

    public IEnumerator GetEnumerator()
    {
        foreach (string name in enemiesToDisable)
        {
            yield return name;
        }
    }

    public void Add(object enemy)
    {
        enemiesToDisable.Add(enemy.ToString());
    }
}

[Serializable]
public class PlayerDataBase : GameDataBase
{
    int health;

    public int Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }

}