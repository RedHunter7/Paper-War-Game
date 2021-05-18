using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
	public Text coin;
	int delayAmount = 1;
	float timer;
    // Start is called before the first frame update
    void Start()
    {
        Data.coin = 0;
    }

    // Update is called once per frame
    void Update()
    {
		timer += Time.deltaTime;
        if(Data.coin <= 500 && timer >= delayAmount)
		{
			Data.coin += 10;
			coin.text = Data.coin.ToString();
			timer = 0;
		}
    }
}
