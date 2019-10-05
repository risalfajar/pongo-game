using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour {

	public float speed = .1f;

	private Rigidbody2D rigidBody;
	private Vector2 velocity;

	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		velocity = new Vector2(transform.up.x, transform.up.y);
		velocity *= speed;
	}

	void FixedUpdate()
	{
		//rigidBody.velocity = transform.up * speed
		rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime);
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Destroy(gameObject);
	}
}
