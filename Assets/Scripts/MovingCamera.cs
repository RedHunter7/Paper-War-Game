using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
	public float speed = 15f, minWorldRange, maxWorldRange;
	float borderThickness = 30f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x  <= borderThickness)
		{
			pos.x -= speed * Time.deltaTime; 
		}
		
		if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - borderThickness)
		{
			pos.x += speed * Time.deltaTime;
		}
		
		pos.x = Mathf.Clamp(pos.x, minWorldRange, maxWorldRange);
		transform.position = pos;
	}
}
