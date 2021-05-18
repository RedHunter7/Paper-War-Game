using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	private bool isPaused = false;
	public GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
		{
			if(isPaused == false)
			{
				pausePanel.transform.localScale = new Vector3(1,1,1);
				Time.timeScale = 0;
				isPaused = true;
			}
			else if(isPaused == true)
			{
				ResumeGame();
			}
		}
    }
	
	public void ResumeGame()
	{
		pausePanel.transform.localScale = new Vector3(0,0,0);
		Time.timeScale = 1;
		isPaused = false;
	}
	
	public void RestartGame()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		Time.timeScale = 1;
	}
	
	public void MainMenu()
	{
		SceneManager.LoadScene("Home");
		Time.timeScale = 1;
	}
	
	public void QuitGame()
	{
		Application.Quit();
	}
}
