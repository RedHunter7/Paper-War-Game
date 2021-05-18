using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	float minute = 0, timer, delayTime= 1, seconds;
	string minutesText, secondsText;
	public Text timerText;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
		if(timer >= delayTime)
		{
			seconds++;
			if(seconds == 60)
			{
				minute++;
				seconds = 0;
			}
			
			if(minute < 10) minutesText = "0" + minute.ToString();
			else minutesText = minute.ToString();
			
			if(seconds < 10) secondsText = "0" + seconds.ToString();
			else secondsText = seconds.ToString();
			
			timerText.text = minutesText + " : " + secondsText;
			timer = 0;
		}
    }
}
