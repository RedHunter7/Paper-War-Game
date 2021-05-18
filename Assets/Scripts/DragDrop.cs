using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour
{
	[HideInInspector]
	public Vector3 screenPoint;
	[HideInInspector]
	public Vector3 offset;
	[HideInInspector]
	public bool follow = true;
	[HideInInspector]
	public int cost;
	
	public static bool showError = false;
	
    // Start is called before the first frame update
    void Start()
    {
           
	}

    // Update is called once per frame
    void Update()
    {
		if(follow)
		{
			Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		    Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
		    transform.position = curPosition;
			if(Input.GetMouseButtonUp(0))
			{
				if(transform.position.x < 3 && (transform.position.y <= 0.2f &&  transform.position.y >= -0.5f))
				{
					foreach (Behaviour childComponent in GetComponentsInChildren<Behaviour>()) childComponent.enabled = true;	
					follow = false;
					Data.coin -= cost;
				}
				else
				{
					Destroy(gameObject);
					showError = true;
				}
			}
		}
        
    }
}
