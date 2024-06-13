using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour, IDataPersistence
{
	public static PlayerStats instance;
	private int max_health;
	private int max_entropy;
	[SerializeField]  int current_health;
	[SerializeField] int current_entropy;
	[SerializeField] private int buffer_health;
	private int buffer_entropy;
	private GameObject UI;
	private Slider slider_health;
	private Slider slider_entropy;


	private void Awake()
	{
		max_health = 100;
		max_entropy = 100;
	}
    void Start()
    {
		instance = this;
		UI = GameObject.Find("UI");
		slider_health = UI.transform.Find("HealthBar_").GetComponent<Slider>();
		slider_entropy = UI.transform.Find("Entropy").GetComponent<Slider>();

	    /* Setup stats */
        current_health = max_health;
		current_entropy = max_entropy;
		buffer_entropy = max_entropy;
		buffer_health = max_health;
		slider_health.value = current_health;
		slider_entropy.value = current_entropy;
    }

    // Update is called once per frame
    void Update()
    {
        if (buffer_health != current_health)
		{
			current_health = buffer_health;
			slider_health.value = current_health;
		}
		if (buffer_entropy != current_entropy)
		{
			current_entropy = buffer_entropy;
		}
		//Debug.Log("hp = " + current_health);
    }
	public void TakeDamage(int damage)
	{
		buffer_health -= damage;
	}
	public void LoadData(GameData data)
	{
		this.buffer_health = data.health;
		this.buffer_entropy = data.entropy;
	}
	public void SaveData(GameData data)
	{
		if (data == null)
		{
			Debug.LogError("GameData is null in playerstats.SaveData");
			return;
		}
		data.health = this.current_health;
		data.entropy = this.current_entropy;
	}
}
