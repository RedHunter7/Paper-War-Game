using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	Animator anim;
	private float attackRate = 2f, nextAttackTime = 0f, terrainVelocity, health;
	bool isGrounded, isDead, isAttack, isFoundEnemy;
	string moveSide;
	public Transform groundCheck1, groundCheck2, attackRange, visionRange;
	public float checkRadius, attackRadius, visionRadius;
	public LayerMask whatIsGround, whatIsEnemy;
	Rigidbody2D player;
	ParticleSystem deadEffect;
	public ParticleSystem hitEffect;
	BoxCollider2D bodyCollider;
	CapsuleCollider2D footCollider;
	public AudioSource audio;
	public AudioClip swingSound, hitSound, deathSound;
	GameObject enemyTower;
	
    // Start is called before the first frame update
    void Start()
    {
		isDead = false;
        anim = GetComponent<Animator>();
		player = GetComponent<Rigidbody2D>();
		bodyCollider = GetComponent<BoxCollider2D>();
		footCollider = GetComponent<CapsuleCollider2D>();
		deadEffect = GetComponent<ParticleSystem>();
		terrainVelocity = 0;
		health = 100;
    }

    // Update is called once per frame
    void Update()
    {		
		if (health <= 0 && isDead == false)
		{
			audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
		    isDead = true;
		    player.velocity = new Vector2(0, player.velocity.y);
		    deadEffect.Play();
		    SetActiveAllChildren(transform, false);
		}
    }
	
	void FixedUpdate()
	{
		enemyTower = GameObject.FindWithTag("EnemyTower");
		if(enemyTower)
		{
			if (Vector2.Distance(enemyTower.transform.position, transform.position) >= 0.1) 
			{
				Collider2D detectEnemy = Physics2D.OverlapCircle(visionRange.position, visionRadius, whatIsEnemy);
				if(detectEnemy)
				{
					if(transform.position.x >= detectEnemy.gameObject.transform.position.x) transform.rotation = Quaternion.Euler(0, 180, 0);
				    else transform.rotation = Quaternion.Euler(0,0,0);
			
				    Collider2D enemyOnRange = Physics2D.OverlapCircle(attackRange.position, attackRadius, whatIsEnemy);
				    if(enemyOnRange)
				    {
						anim.ResetTrigger("run");
					    if (Time.time >= nextAttackTime)
					    {
							anim.SetTrigger("attack");
						    nextAttackTime = Time.time + 1f / attackRate;
						    Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackRange.position, attackRadius, whatIsEnemy);
				            audio.PlayOneShot(swingSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
						    foreach(Collider2D enemy in hitEnemies)
						    {
								enemy.GetComponent<Enemy>().TakeDamage(8f);
							} 
					    }
				    }
				    else
					{
						anim.SetTrigger("run");
					    transform.position = Vector2.MoveTowards(transform.position, new Vector2(detectEnemy.gameObject.transform.position.x, transform.position.y), 1.5f * Time.deltaTime);
			            player.MovePosition(transform.position);
				    }			
			    }
			    else 
			    {
					anim.SetTrigger("run");
				    transform.rotation = Quaternion.Euler(0,0,0);
			        transform.position = Vector2.MoveTowards(transform.position, new Vector2(enemyTower.transform.position.x, transform.position.y), 1.5f * Time.deltaTime);
			        player.MovePosition(transform.position);
			    }
		    }
		    else anim.ResetTrigger("run");
		}
		
		isGrounded = Physics2D.OverlapCircle(groundCheck1.position, checkRadius, whatIsGround) ||  Physics2D.OverlapCircle(groundCheck2.position, checkRadius, whatIsGround);	
		if (isGrounded == false)
		{
			anim.SetTrigger("jump");
		}
	}
	
	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(attackRange.position, attackRadius);
		Gizmos.DrawWireSphere(groundCheck1.position, checkRadius);
		Gizmos.DrawWireSphere(groundCheck2.position, checkRadius);
		Gizmos.DrawWireSphere(visionRange.position, visionRadius);
	}
	
	private void OnCollisionEnter2D(Collision2D collision)
	{	
		if (collision.transform.tag.Equals("Enemy"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(),footCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), footCollider, true);
		}
		
		if (collision.transform.tag.Equals("Player Defender"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(),footCollider, true);
		}
		
		if (collision.transform.tag.Equals("Player Shooter"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(),footCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), footCollider, true);
		}
		
		if (collision.transform.tag.Equals("Player"))
		{
			transform.position = transform.position;
			anim.ResetTrigger("run");
			anim.SetTrigger("idle");
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
	 
	public void PlayerTakeDamage(float damage)
	{
		health = health - damage;
		if (health > 0) 
		{
			audio.PlayOneShot(hitSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
			hitEffect.Play();
		}
	}
}