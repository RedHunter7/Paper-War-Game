using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemy : MonoBehaviour
{
	float timer = 0, delayTime;
	public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        delayTime = Random.Range(4f, 12f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
		
		if(timer > delayTime)
		{
			Instantiate(enemy, transform.position, transform.rotation);
			timer = 0;
			delayTime = Random.Range(4f, 8f);
		}
    }
}
