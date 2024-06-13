using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : MonoBehaviour
{

	[SerializeField] int damage;


	private void Start()
	{
	}
	private void OnTriggerEnter(Collider other) {
		if (other.name == "Player") {
			Debug.Log("Bim dans tes dents");
			PlayerStats.instance.TakeDamage(damage);
		}
	}
}
