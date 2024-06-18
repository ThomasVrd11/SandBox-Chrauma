using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetReaper : MonoBehaviour
{
    [SerializeField] GameObject player;
	[SerializeField] Transform destination;
	[SerializeField] GameObject target;
	[SerializeField] GameObject effects;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Player") 
		{
			player.GetComponent<CharacterController>().enabled = false;
			effects.SetActive(true);
			

		}
	}
}
