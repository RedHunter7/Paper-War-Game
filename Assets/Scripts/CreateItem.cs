using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItem : MonoBehaviour
{
	public GameObject item;
	private Vector3 screenPoint;
	private Vector3 offset;
	public GameObject dragArea;
	public int cost;
	public GameObject errorMessage;
	SpriteRenderer sprite;
	
	// Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
    void Update()
    {
        if(Data.coin >= cost)
		{
			sprite.color = new Color(1, 1, 1);
		}
		else 
		{
			sprite.color = new Color(0.4f, 0.4f, 0.4f);
		}
		
		if(DragDrop.showError == true)
		{
			StartCoroutine("ShowError");
			DragDrop.showError = false;
		}
    }
	
	private void OnMouseDown()
	{
		if(Data.coin >= cost)
		{
			dragArea.SetActive(true);
		    Debug.Log("Hello");
		    GameObject _item = (GameObject)Instantiate(item, transform.position, Quaternion.identity);
		    foreach (Behaviour childComponent in _item.GetComponentsInChildren<Behaviour>()) childComponent.enabled = false;
		    screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		    offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
		    DragDrop dd = _item.GetComponent<DragDrop>();
		    dd.enabled = true;
		    dd.screenPoint = screenPoint;
		    dd.offset = offset;
			dd.cost = cost;
		}	
	}
	
	private void OnMouseUp()
	{
		dragArea.SetActive(false);
	}
	
	IEnumerator ShowError()
	{
		errorMessage.SetActive(true);
		yield return new WaitForSeconds(2);
		errorMessage.SetActive(false);
	}
}
