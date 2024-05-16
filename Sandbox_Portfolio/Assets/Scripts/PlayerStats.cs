using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
	private int max_health;
	private int max_entropy;
	public int current_health;
	public int current_entropy;
	private int buffer_health;
	private int buffer_entropy;


	private void Awake()
	{
		max_health = 100;
		max_entropy = 100;
	}
    void Start()
    {
        current_health = max_health;
		current_entropy = max_entropy;
		buffer_entropy = max_entropy;
		buffer_health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        if (buffer_health != current_health)
		{
			//change l'ui
		}
		current_health = buffer_health;
		if (buffer_entropy != current_entropy)
		{
			//change l'ui
		}
		current_entropy = buffer_entropy;
		Debug.Log("hp = " + current_health);
    }
	public void TakeDamage(int damage)
	{
		buffer_health -= damage;
	}
}
