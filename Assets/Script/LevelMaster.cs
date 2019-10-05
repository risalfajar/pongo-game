using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelMaster : MonoBehaviour {

	public int cageCount;
	[HideInInspector]public int cageLeft;

	public static LevelMaster instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		cageLeft = cageCount;
	}

	public void EndGame()
	{
		if(cageLeft > 0)
		{
			Lose();
		}else
		{
			Win();
		}
	}

	private void Lose()
	{
		GameplayUI.instance.showEndGameScreen(false);
	}

	private void Win()
	{
		GameplayUI.instance.showEndGameScreen(true);
	}
}
