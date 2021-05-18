using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	Animator anim, spin;
	//public GameObject border1, border2;
	public LayerMask whatIsPlayer, whatIsEnemy;
	public Transform attackRange, visionRange;
	public float attackRadius, visionRadius;
	string moveDirection;
	private float speed = 120, attackRate = 1f, nextAttackTime = 0f, health;
	bool isAttack, deadEffectPlayed;
	Rigidbody2D enemy;
	string mode;
	GameObject playerTower;
	Transform wheel;
	ParticleSystem hitEffect, deadEffect;
	public AudioSource audio;
	public AudioClip hitSound, deathSound;
	
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		wheel = transform.Find("Wheel");
		spin = wheel.gameObject.GetComponent<Animator>();
		hitEffect = transform.Find("Hit Effect").GetComponent<ParticleSystem>();
		deadEffect = GetComponent<ParticleSystem>();
		moveDirection = "right";
		mode = "neutral";
		health = 90f;
		deadEffectPlayed = false;
		transform.rotation = Quaternion.Euler(0,180, 0);
    }
	
	void FixedUpdate()
	{
		playerTower = GameObject.FindWithTag("PlayerTower");
		if(playerTower)
		{
			if (Vector2.Distance(playerTower.transform.position, transform.position) >= 0.1) 
		    {
				Collider2D detectPlayer = Physics2D.OverlapCircle(visionRange.position, visionRadius, whatIsPlayer);
				if(detectPlayer)
				{
					if(transform.position.x >= detectPlayer.gameObject.transform.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);
				    else transform.rotation = Quaternion.Euler(0,0,0);
				
				    Collider2D playerOnRange = Physics2D.OverlapCircle(attackRange.position, attackRadius, whatIsPlayer);
					
				    if(playerOnRange)
				    {
						spin.speed = 0;
			            anim.SetTrigger("hostile");
					    if (Time.time >= nextAttackTime && mode != "dead")
						{
							anim.SetTrigger("attack");
						    nextAttackTime = Time.time + 1f / attackRate;
						    Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackRange.position, attackRadius, whatIsPlayer);
						    foreach(Collider2D player in hitPlayers)
						    {
								if(player.gameObject.tag == "Player") player.GetComponent<Player>().PlayerTakeDamage(10f);
			                    else if(player.gameObject.tag == "Player Shooter") player.GetComponent<PlayerShooter>().PSTakeDamage(10f);
							    else if(player.gameObject.tag == "Player Defender") player.GetComponent<Defender>().DefenderTakeDamage(5f);
						    }
					    }
				    }
				    else
					{
						spin.speed = 1;
		                anim.SetTrigger("idle");
		                transform.position = Vector2.MoveTowards(transform.position, new Vector2(detectPlayer.gameObject.transform.position.x, transform.position.y), 1.5f * Time.deltaTime);
		                enemy.MovePosition(transform.position);		
					}
			    }
			    else
				{
		            spin.speed = 1;
		            anim.SetTrigger("idle");
		            transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTower.gameObject.transform.position.x, transform.position.y), 1.5f * Time.deltaTime);
		            enemy.MovePosition(transform.position);
				}
		    }
		    else if (Vector2.Distance(playerTower.transform.position, transform.position) < 0.1)
			{
				spin.speed = 0;
				anim.SetTrigger("hostile");
			}
		}
		

		if (mode == "dead" && deadEffectPlayed == false)
		{
			audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
			SetActiveAllChildren(transform, false);
		    deadEffect.Play();
			deadEffectPlayed = true;
		}
		
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackRange.position, attackRadius);
		Gizmos.DrawWireSphere(visionRange.position, visionRadius);
	}
	
	
	private void SetActiveAllChildren(Transform transform, bool value)
     {
         foreach (Transform child in transform)
         {
             child.gameObject.SetActive(value);
             SetActiveAllChildren(child, value);
         }
     }
	
	public void TakeDamage(float damage)
	{
		health = health - damage;
		if (health > 0) 
		{
			audio.PlayOneShot(hitSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
			hitEffect.Play();
		}
		else if (health <=0) mode = "dead";
	}
}
