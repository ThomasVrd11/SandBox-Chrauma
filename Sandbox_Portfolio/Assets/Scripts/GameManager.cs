using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameManager instance;
	private void Awake() {
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}
    void Start()
    {
        // #if !UNITY_EDITOR
        // Cursor.visible = false;
        // #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
