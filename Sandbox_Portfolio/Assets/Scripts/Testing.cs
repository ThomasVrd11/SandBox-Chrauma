using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
	Transform player;
	[SerializeField] GameObject ball;
	private void Awake()
	{
		player = this.transform;
	}
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		player.LookAt(ball.transform.position);
	}
}
