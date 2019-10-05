using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {

	public Transform player;

	[Range(1, 100)]
	public float percentFollowing = 100;

	private Rigidbody2D rigidBody;
	private float playerMoveSpeed;
	private Vector2 dir;

	void Start()
	{
//		rigidBody = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player").transform;
//		playerMoveSpeed = player.GetComponent<PlayerControl>().moveSpeed;
		dir = transform.position - player.transform.position;
		dir = new Vector2(dir.x, 0);
		dir.Normalize();
	}

	void Update()
	{
		transform.Translate(dir * Time.deltaTime * percentFollowing/100);
	}

}
