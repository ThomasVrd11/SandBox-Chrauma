using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    public static DataPersistenceManager instance {get; private set;}
    private List<IDataPersistence> dataPersistencesObjects;

    private void Awake() {
        if(instance != null)
        {
            Debug.LogError("More than one Data Persistence Manager in the scene");
            //Destroy(this);
        }
        instance = this;
    }
    private void Start() {
        this.dataPersistencesObjects = FindAllDataPersistenceObjects();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        if (this.gameData == null)
        {
            NewGame();
        }
        foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }
        Debug.Log("Loaded hp = " + gameData.health);
    }
    public void SaveGame()
    {
        // TODO - gather data and file it into save file
        foreach(IDataPersistence dataPersistenceObj in dataPersistencesObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }
        Debug.Log("Save hp = " + gameData.health);
    }
    public bool CheckIfSave()
    {
        LoadGame();
        if(this.gameData != null)
        {
            return true;
        }
        else return false;
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return new List<IDataPersistence>(dataPersistenceObjects);
    }
}
