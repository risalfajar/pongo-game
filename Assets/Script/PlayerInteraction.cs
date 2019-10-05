using System.Collections;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

	public GameObject araPrefab;
	public Transform shotPoint;
	public float shootingForce;
	public float throwAngle = -60f;
	public int startingAra = 3;

	public static int health = 3;
	public static int key;
	public static int ara;

	#if UNITY_EDITOR
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.S))
			ThrowBomb();
	}
	#endif

	void Start()
	{
		health = 3;
		key = 0;
		ara = startingAra;
	}

	public void ThrowBomb()
	{
		if(ara > 0)
		{
			GameObject araGenerated = (GameObject)Instantiate(araPrefab,
				shotPoint.position,
				Quaternion.Euler(0, araPrefab.transform.rotation.y, throwAngle));
			ara--;
			GameplayUI.instance.UpdateText();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.CompareTag("Key"))
		{
			key++;
			GameplayUI.instance.UpdateText();
			GameObject.Destroy(col.gameObject);
		}
		else if(col.gameObject.CompareTag("Tent"))
		{
			health++;
			GameplayUI.instance.UpdateText();
			col.GetComponent<BoxCollider2D>().enabled = false;
		}
		else if(col.gameObject.CompareTag("Root"))
		{
			PlayerControl.instance.isHanging = true;
			PlayerControl.instance.isJump = false;
		}
		else if(col.gameObject.CompareTag("Ara Pickup"))
		{
			ara++;
			GameplayUI.instance.UpdateText();
			GameObject.Destroy(col.gameObject);
		}
		else if(col.gameObject.CompareTag("Cage"))
		{
			if(key > 0)
			{
				col.gameObject.GetComponent<Animator>().SetTrigger("Door Open");
				key--;
				LevelMaster.instance.cageLeft--;
			}
		}
		else if(col.gameObject.CompareTag("End Game"))
		{
			LevelMaster.instance.EndGame();
		}
	}
}
