using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

	public static PlayerControl instance;

	public float moveSpeed = 5f;
	public float jumpForce = 150f;
	public float jumpForwardForce = 150f;
	public bool isJump = false, isHanging = false, isDuck = false;
	[HideInInspector]public bool disableControl = false;

	private Rigidbody2D rigidBody;
	private Animator anim;
	private Vector2 jumpDir;
	private bool isGround = false;
	private float startY, endY;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		rigidBody = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		//Jalan ke Kiri
		moveSpeed *= -1;

		jumpDir = new Vector2(jumpForwardForce, jumpForce);
	}

	void Update()
	{
		if (disableControl)
			return;

		if (isHanging){
			anim.SetBool("Hanging", true);

			if(Input.GetKeyDown(KeyCode.DownArrow) || SwipeDown())
			{
				anim.SetBool("Hanging", false);
				rigidBody.isKinematic = false;
				isHanging = false;
			}
				
			return;
		}
		if (isGround && !isDuck)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow) || SwipeUp())
			{
				isJump = true;
				return;
			}
			if(Input.GetKeyDown(KeyCode.DownArrow) || SwipeDown())
			{
				isDuck = true;
				return;
			}
		}
		else if(isDuck)
		{
			if(Input.GetKeyDown(KeyCode.UpArrow) || SwipeUp())
			{
				StandUp();
			}
		}
	}

	void FixedUpdate()
	{
		if(isHanging)
		{
			Hanging();
			return;
		}

		if(isJump)
		{
			Jump();
		}else if(isDuck)
		{
			Duck();
		}else
		{
			MoveForward();
		}
	}

	public bool SwipeDown()
	{
		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Began){
				startY = touch.position.y;
				endY = touch.position.y;
			}
			if(touch.phase == TouchPhase.Moved){
				endY = touch.position.y;
			}
			if(touch.phase == TouchPhase.Ended){
				if ((startY - endY) > 80)
				{
					return true;
				}
			}
		}
		return false;
	}
		
	public bool SwipeUp()
	{
		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Began){
				startY = touch.position.y;
				endY = touch.position.y;
			}
			if(touch.phase == TouchPhase.Moved){
				endY = touch.position.y;
			}
			if(touch.phase == TouchPhase.Ended){
				if((startY - endY) < -80){
					return true;
				}
			}
		}
		return false;
	}

	public void StandUp()
	{
		anim.SetBool("Duck", false);
		isDuck = false;
	}

	public void Jump()
	{
		rigidBody.AddForce(jumpDir);
		anim.SetBool("Ground", false);
		isGround = false;
		isJump = false;
	}

	public void Duck()
	{
		rigidBody.velocity = Vector2.zero;
		anim.SetBool("Duck", true);
	}

	void MoveForward()
	{
		rigidBody.velocity = new Vector2(moveSpeed, rigidBody.velocity.y);
	}

	void Hanging()
	{
		rigidBody.velocity = Vector2.zero;
		rigidBody.isKinematic = true;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.CompareTag("Ground"))
		{
			isGround = true;
			anim.SetBool("Ground", true);
		}
	}
}
