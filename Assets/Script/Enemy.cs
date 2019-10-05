using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Enemy : MonoBehaviour {

	public bool isSniper;
	public bool hasKey = false;
	public bool canWalk = true;
	[HideInInspector]public GameObject keyPrefab;

	[HideInInspector] public Transform startPoint;
	[HideInInspector] public Transform endPoint;

	[Space]
	public float walkSpeed = 20f;
	public float detectionRange = 5f;

	[HideInInspector]public GameObject bullet;
	[HideInInspector]public GameObject weapon;
	[HideInInspector]public Transform bulletPosition;
	[HideInInspector]public float shootingInterval = 1f;

	private Collider2D[] colliders;
	private Rigidbody2D rigidBody;
	private Vector2 dir = Vector2.right;
	private Transform player;
	private Animator anim;
	private bool isDetectingPlayer = false;
	private int dirToPlayer = 1;
	private float detectingInterval = 0.1f;
	private float shootingCountdown;
	private bool isDead = false;

	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		rigidBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		colliders = GetComponents<Collider2D>();
		InvokeRepeating("DetectPlayer", 0f, detectingInterval);

		if (isSniper)
			shootingCountdown = 0;
	}

	void DetectPlayer()
	{
		if (isDead)
			return;
		
		float distance = player.position.x - transform.position.x;

		if(dir.x == 1) //menghadap ke kanan
		{
			if(distance <= detectionRange && distance >= 0)
			{
				isDetectingPlayer = true;
				dirToPlayer = 1;
				if (isSniper)
					Shot();
			}else{
				isDetectingPlayer = false;
				if (isSniper)
					anim.SetBool("Shoot", false);
			}
		}else if(dir.x == -1)
		{
			if(distance * -1 <= detectionRange && distance * -1 >= 0)
			{
				isDetectingPlayer = true;
				dirToPlayer = -1;
				if (isSniper)
					Shot();
			}else{
				isDetectingPlayer = false;
				if (isSniper)
					anim.SetBool("Shoot", false);
			}
		}
	}

	void Update()
	{
		if (isDead)
			return;

		if(isSniper)
		{
			if(isDetectingPlayer)
			{
				if(dir.x == 1)
					weapon.transform.right = player.position - weapon.transform.position;
				else if(dir.x == -1)
					weapon.transform.right = weapon.transform.position - player.position;
				return; //agar berhenti berjalan
			}
		}else{
			if(isDetectingPlayer) //Jika mendeteksi player
			{
				if(dirToPlayer == 1) //Jika player ada di kanan
				{
					transform.Translate(Vector2.right * Time.deltaTime * walkSpeed);
				}else{
					transform.Translate(Vector2.left * Time.deltaTime * walkSpeed);
				}
				return;
			}
		}
		if(canWalk)
			transform.Translate(dir * Time.deltaTime * walkSpeed); //berjalan ke waypoint
	}

	void Flip()
	{
		transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
		dir *= -1;
	}

	void Shot()
	{
		anim.SetBool("Shoot", true);
		shootingCountdown--;
		if(shootingCountdown <= 0)
		{
			Vector3 difference = player.transform.position - bulletPosition.position;
			float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90f;

			GameObject insBullet = (GameObject)Instantiate(bullet,
				bulletPosition.position,
				Quaternion.Euler(0.0f, 0.0f, rotationZ));
			
			shootingCountdown = shootingInterval / detectingInterval;
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (isDead)
			return;
		
		if(!isDetectingPlayer && canWalk)
		{
			if(col.gameObject.name.Equals(startPoint.name) || col.gameObject.name.Equals(endPoint.name))
			{
				Flip();
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (isDead)
			return;
		
		if(col.gameObject.CompareTag("Ara"))
		{
			Dead();
		}
	}

	void Dead()
	{
		isDead = true;
		anim.SetTrigger("Dead");
		rigidBody.bodyType = RigidbodyType2D.Static;

		foreach(Collider2D collider in colliders)
		{
			collider.enabled = false;
		}

		if(hasKey)
		{
			Vector3 keyOffset = new Vector3(0, -1f, 0);
			GameObject key = (GameObject)Instantiate(keyPrefab, transform.position + keyOffset, Quaternion.identity);
		}
	}

	#if UNITY_EDITOR
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		if(dir.x == 1)
			Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + detectionRange, transform.position.y));
		else if(dir.x == -1)
			Gizmos.DrawLine(transform.position, new Vector2(transform.position.x - detectionRange, transform.position.y));
	}
	#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(Enemy))]
public class Enemy_Editor: Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Enemy script = (Enemy)target;

		if (script.canWalk)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.Space();
			script.startPoint = EditorGUILayout.ObjectField("Start Point", script.startPoint, typeof(Transform), true) as Transform;
			script.endPoint = EditorGUILayout.ObjectField("End Point", script.endPoint, typeof(Transform), true) as Transform;
		}
		if(script.isSniper)
		{
			EditorGUILayout.Separator();
			EditorGUILayout.Space();
			script.bullet = EditorGUILayout.ObjectField("Bullet", script.bullet, typeof(GameObject), true) as GameObject;
			script.weapon = EditorGUILayout.ObjectField("Weapon", script.weapon, typeof(GameObject), true) as GameObject;
			script.bulletPosition = EditorGUILayout.ObjectField("Bullet Position", script.bulletPosition, typeof(Transform), true) as Transform;
			script.shootingInterval = EditorGUILayout.FloatField("Shooting Interval", script.shootingInterval);
		}
		if(script.hasKey)
		{
			EditorGUILayout.Space();
			script.keyPrefab = EditorGUILayout.ObjectField("Key Prefab", script.keyPrefab, typeof(GameObject), true) as GameObject;
		}
	}
}
#endif