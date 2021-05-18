using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
	public LayerMask whatIsEnemy;
	public Transform visionRange;
	public float visionRadius;
	ParticleSystem deadEffect, hitEffect;
	CapsuleCollider2D bodyCollider;
	private float health;
	private bool isDead;
	public AudioSource audio;
	public AudioClip hitSound, deathSound;
    // Start is called before the first frame update
    void Start()
    {
        deadEffect = GetComponent<ParticleSystem>();
		hitEffect = GameObject.Find("Defender Hit Effect").GetComponent<ParticleSystem>();
		bodyCollider = GetComponent<CapsuleCollider2D>();
		health = 150;
		isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
		Collider2D detectEnemy = Physics2D.OverlapCircle(visionRange.position, visionRadius, whatIsEnemy);
		if(detectEnemy)
		{		
			if(transform.position.x >= detectEnemy.gameObject.transform.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);
			else transform.rotation = Quaternion.Euler(0,0,0);	
		}
		else 
		{
			transform.rotation = Quaternion.Euler(0,0,0);
		}
        if (health <= 0 && isDead == false)
		{
			audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
		    isDead = true;
		    deadEffect.Play();
		    SetActiveAllChildren(transform, false);
		}
    }
	
	private void OnCollisionEnter2D(Collision2D collision)
	{	
	    if (collision.transform.tag.Equals("Player"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(),bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
		}
		
		if (collision.transform.tag.Equals("Player Shooter"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
		}
	}
	
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(visionRange.position, visionRadius);
	}
	
	public void DefenderTakeDamage(float damage)
	{
		health = health - damage;
		if (health > 0) 
		{
			audio.PlayOneShot(hitSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
			hitEffect.Play();
		}
	}
	
	private void SetActiveAllChildren(Transform transform, bool value)
     {
         foreach (Transform child in transform)
         {
             child.gameObject.SetActive(value);
             SetActiveAllChildren(child, value);
         }
     }
}
