using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
	public Transform groundCheck1, groundCheck2, visionRange, shotPoint;
	public LayerMask whatIsGround, whatIsEnemy;
	public float checkRadius, visionRadius;
	public GameObject bullet;
	public static bool isFacingRight;
	private float attackRate = 2f, nextAttackTime = 0f;
	private bool isGrounded, isAttack, animAttackPlayed, isDead;
	Animator anim;
	Rigidbody2D playerShooter;
	CircleCollider2D bodyCollider;
	CapsuleCollider2D footCollider;
	GameObject enemyTower;
	ParticleSystem deadEffect;
	public ParticleSystem hitEffect;
	private float health;
	public AudioSource audio;
	public AudioClip hitSound, deathSound, shotSound;
	
    // Start is called before the first frame update
    void Start()
    {
		anim = GetComponent<Animator>();
		bodyCollider = GetComponent<CircleCollider2D>();
		footCollider = GetComponent<CapsuleCollider2D>();
		playerShooter = GetComponent<Rigidbody2D>();
		deadEffect = GetComponent<ParticleSystem>();
		isAttack = false;
		isFacingRight = true;
		animAttackPlayed = false;
		health = 60;
		isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && isDead == false)
		{
			audio.PlayOneShot(deathSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
		    isDead = true;
		    deadEffect.Play();
		    SetActiveAllChildren(transform, false);
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
					anim.ResetTrigger("run");
		            anim.SetTrigger("attack");
					//if (isAttack == false) StartCoroutine("DelayAttack");
				
			        if (Time.time >= nextAttackTime)
					{
						audio.PlayOneShot(shotSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
						Instantiate(bullet, shotPoint.position, transform.rotation);
					    nextAttackTime = Time.time + 1f / attackRate;
					} 
				
				    if(transform.position.x >= detectEnemy.gameObject.transform.position.x)
				    {
						transform.rotation = Quaternion.Euler(0, 180, 0);
					    isFacingRight = false;
				    }
				    else
				    {
						transform.rotation = Quaternion.Euler(0,0,0);
					    isFacingRight = true;
				    }		
			    }
			    else 
				{
					transform.rotation = Quaternion.Euler(0,0,0);
				    isFacingRight = true;
				    isAttack = false;
				    animAttackPlayed = false;
				    anim.ResetTrigger("attack");
				    anim.SetTrigger("run");
			        transform.position = Vector2.MoveTowards(transform.position, new Vector2(enemyTower.transform.position.x, transform.position.y), 1.5f * Time.deltaTime);
			        playerShooter.MovePosition(transform.position);
			    }
		    }
		    else
			{
				//if (isAttack == false) StartCoroutine("DelayAttack");
				
				if (Time.time >= nextAttackTime && isAttack == true)
				{
					Instantiate(bullet, shotPoint.position, transform.rotation);
				    nextAttackTime = Time.time + 1f / attackRate;
			    } 
		    }		
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
		/*
		if (collision.transform.tag.Equals("Player Shooter"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CircleCollider2D>(),footCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), footCollider, true);
		}*/
		
		if (collision.transform.tag.Equals("Player"))
		{
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<BoxCollider2D>(),footCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), bodyCollider, true);
			Physics2D.IgnoreCollision(collision.gameObject.GetComponent<CapsuleCollider2D>(), footCollider, true);
		}
	}
	
	IEnumerator DelayAttack()
	{
		if(animAttackPlayed == false)
		{
			anim.ResetTrigger("run");
		    anim.SetTrigger("attack");
			animAttackPlayed = true;
		}
		yield return new WaitForSeconds(1f);
		isAttack = true;
	}
	
	public void PSTakeDamage(float damage)
	{
		health = health - damage;
		if (health > 0) 
		{
			audio.PlayOneShot(hitSound, PlayerPrefs.GetFloat("SFXVolume", 1f));
			hitEffect.Play();
		}
	}
}
