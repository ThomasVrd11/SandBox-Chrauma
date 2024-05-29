using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : MonoBehaviour
{
	PlayerStats playerStats;
	[SerializeField] int damage;


	private void Start()
	{
		playerStats = GameObject.Find("Player").transform.GetComponent<PlayerStats>();
	}
	private void OnTriggerEnter(Collider other) {
		if (other.name == "Player") {
			Debug.Log("Bim dans tes dents");
			playerStats.TakeDamage(damage);
		}
	}
}
