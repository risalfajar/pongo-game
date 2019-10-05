using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class GameplayUI : MonoBehaviour {

	public TextMeshProUGUI healthText;
	public TextMeshProUGUI keyText;
	public TextMeshProUGUI araText;

	public GameObject winScreen;
	public GameObject loseScreen;

	public static GameplayUI instance;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		Invoke("UpdateText", 0.1f);
	}

	public void UpdateText()
	{
		healthText.text = PlayerInteraction.health.ToString();
		keyText.text = PlayerInteraction.key.ToString();
		araText.text = PlayerInteraction.ara.ToString();
	}

	public void showEndGameScreen(bool win)
	{
		Time.timeScale = 0;

		if(win)
		{
			winScreen.SetActive(true);
		}else
		{
			loseScreen.SetActive(true);
		}
	}

	public void Pause()
	{
		Time.timeScale = 0;
	}

	public void Resume()
	{
		Time.timeScale = 1;
	}

	public void Restart()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void Exit()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene("Main Menu");
	}

}