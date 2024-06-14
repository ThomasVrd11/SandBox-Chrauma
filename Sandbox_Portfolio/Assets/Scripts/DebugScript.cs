using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScript : MonoBehaviour
{
    [SerializeField]GameObject tppos;
    void Start()
    {
        //LogDontDestroyOnLoadObjects();
    }

    void LogDontDestroyOnLoadObjects()
    {
        GameObject temp = null;
        try
        {
            temp = new GameObject();
            DontDestroyOnLoad(temp);
            Scene dontDestroyOnLoad = temp.scene;
            Destroy(temp);

            foreach (GameObject obj in dontDestroyOnLoad.GetRootGameObjects())
            {
                Debug.Log("DontDestroyOnLoad Object: " + obj.name);
            }
        }
        catch
        {
            if (temp != null)
            {
                Destroy(temp);
            }
        }
    }
    public void load()
    {
        DataPersistenceManager.instance.LoadGame();
    }
    public void save()
    {
        DataPersistenceManager.instance.SaveGame();
    }
    public void exit()
    {
        GameManager.instance.ExitGame();
    }
    public void debug()
    {
        DataPersistenceManager.instance.DebugList();
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hey");
        other.transform.SetPositionAndRotation(tppos.transform.position,other.transform.rotation);
    }
}
