using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour
{
	public void OpenPanel(GameObject panel)
	{
		panel.transform.localScale = new Vector3(1,1,1);
	}
	
	public void ClosePanel(GameObject panel)
	{
		panel.transform.localScale = new Vector3(0,0,0);
	}
}
