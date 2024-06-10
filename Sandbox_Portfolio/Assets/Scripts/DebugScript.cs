using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugScript : MonoBehaviour
{
    [SerializeField]GameObject tppos;
    void Start()
    {
        LogDontDestroyOnLoadObjects();
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
    private void OnTriggerEnter(Collider other) {
        Debug.Log("Hey");
        other.transform.SetPositionAndRotation(tppos.transform.position,other.transform.rotation);
    }
}
