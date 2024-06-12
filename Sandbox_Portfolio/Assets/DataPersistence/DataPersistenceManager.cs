using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{
    private GameData gameData;
    public static DataPersistenceManager instance {get; private set;}
    private void Awake() {
        if(instance != null)
        {
            Debug.LogError("More than one Data Persistence Manager in the scene");
            //Destroy(this);
        }
        instance = this;
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        // TODO - load data from save file
    }
    public void SaveGame()
    {
        // TODO - gather data and file it into save file
    }
}
