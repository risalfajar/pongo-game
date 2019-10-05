using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

	public GameObject tutorialScreen;
	public GameObject upScreen;
	public GameObject downScreen;
	public GameObject standScreen;
	public GameObject shootScreen;

	private int state = 1;
	private bool isShowing = false;

	/* State:
	 * 1 : Jump
	 * 2 : Duck
	 * 3 : Stand Up
	 * 4 : Shoot
	*/

	void Start()
	{
		PlayerControl.instance.disableControl = true;
	}

	void Update()
	{
		if(isShowing)
		{
			if(state == 1)
			{
				if(PlayerControl.instance.SwipeUp() || Input.GetKeyDown(KeyCode.UpArrow))
				{
					DisableAllScreens();
					PlayerControl.instance.Jump();
				}
			}else if(state == 2)
			{
				if(PlayerControl.instance.SwipeDown() || Input.GetKeyDown(KeyCode.DownArrow))
				{
					DisableAllScreens();
					PlayerControl.instance.isDuck = true;
					Invoke("StandUp", 1f);
				}
			}else if(state == 3)
			{
				EnableScreen(standScreen);

				if(PlayerControl.instance.SwipeUp() || Input.GetKeyDown(KeyCode.UpArrow))
				{
					DisableAllScreens();
					PlayerControl.instance.StandUp();
				}
			}
		}
	}

	void StandUp()
	{
		isShowing = true;
	}

	public void Shoot()
	{
		if(isShowing && state == 4)
		{
			DisableAllScreens();
			TutorialFinished();
		}
	}

	void TutorialFinished()
	{
		PlayerControl.instance.disableControl = false;
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(state == 1)
		{
			EnableScreen(upScreen);
		}else if(state == 2)
		{
			EnableScreen(downScreen);
		}else if(state == 4)
		{
			EnableScreen(shootScreen);
		}
	}

	void EnableScreen(GameObject selectedScreen)
	{
		tutorialScreen.SetActive(true);
		selectedScreen.SetActive(true);

		isShowing = true;
		Time.timeScale = 0;
	}

	void DisableAllScreens()
	{
		tutorialScreen.SetActive(false);
		upScreen.SetActive(false);
		downScreen.SetActive(false);
		standScreen.SetActive(false);
		shootScreen.SetActive(false);

		isShowing = false;
		Time.timeScale = 1;

		state++;
	}
}
