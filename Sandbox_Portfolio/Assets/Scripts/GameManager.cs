using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	private void Awake() {
		if (instance == null)
		{
			instance = this;
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

		//if save exist
		//button continue is white
		// load stat from save
		// elif new game
		// stat default
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
