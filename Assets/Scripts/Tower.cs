using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tower : MonoBehaviour
{
	public string towerType;
	public Slider healthBar;
	private float towerHealth = 100;
	ParticleSystem deadEffect;
	SpriteRenderer sprite;
	bool isDead;
    // Start is called before the first frame update
    void Start()
    {
		isDead = false;
        deadEffect = GetComponent<ParticleSystem>();
		sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(towerHealth <= 0 && isDead == false)
		{
			StartCoroutine("DestroyedTower");
		}
    }
	
	private void OnTriggerEnter2D(Collider2D collision)
	{	
        if(towerType == "Enemy")
		{
			if (collision.transform.tag.Equals("Player Shooter") || collision.transform.tag.Equals("Player"))
			{
				Destroy(collision.gameObject);
			    towerHealth -= 10;
				healthBar.value = towerHealth;
			}
		}
        
        else if(towerType == "Player")
		{
			if (collision.transform.tag.Equals("Enemy"))
			{
				Destroy(collision.gameObject);
				towerHealth -= 10;
				healthBar.value = towerHealth;
			}
		}		
	}
	
	IEnumerator DestroyedTower()
	{
		isDead = true;
		sprite.enabled = false;
		deadEffect.Play();
		yield return new WaitForSeconds(2);
		if(towerType == "Enemy")
		{
			SceneManager.LoadScene("GameSuccess");
		}
        
        else if(towerType == "Player")
		{
			SceneManager.LoadScene("GameOver");
		}
	}
}
