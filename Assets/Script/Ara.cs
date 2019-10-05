using System.Collections;
using UnityEngine;

public class Ara : MonoBehaviour {

	private Rigidbody2D rigidBody;
	private Animator anim;
	private float timeBeforeDisappear = 5f;
	private Vector2 startPos;

	public float throwForce = 5f;
	public float range = 30f;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		startPos = transform.position;
		Vector2 dir = new Vector2(-1, .3f);
		rigidBody.AddForce(dir * throwForce);
	} 

	void Update()
	{
		if(transform.position.x + range <= startPos.x)
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		anim.SetTrigger("Disappear");
		Invoke("Disappear", timeBeforeDisappear);
	}

	void Disappear()
	{
		Destroy(gameObject);
	}
}
