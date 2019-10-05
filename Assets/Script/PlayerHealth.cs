using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public float damageTime = 1f;
	public GameObject cinemachine;

	private float damageTimeCountdown;
	private SpriteRenderer playerSprite;
	private Animator anim;

	void Start()
	{
		damageTimeCountdown = 0;
		playerSprite = GetComponent<SpriteRenderer>();
		anim = cinemachine.GetComponent<Animator>();
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if(col.gameObject.CompareTag("Enemy"))
		{
			damageTimeCountdown -= Time.deltaTime;
			if(damageTimeCountdown <= 0)
			{
				Damage();
				damageTimeCountdown = damageTime;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.CompareTag("Bullet"))
		{
			Damage();
		}
	}

	void Damage()
	{
		PlayerInteraction.health--;
		anim.SetTrigger("Camshake");
		GameplayUI.instance.UpdateText();
	}
}
