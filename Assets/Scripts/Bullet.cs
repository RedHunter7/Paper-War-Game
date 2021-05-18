using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed, lifetime;
	
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyBullet", lifetime);
    }

    // Update is called once per frame
    void Update()
    {
		if(PlayerShooter.isFacingRight == true)  transform.Translate(transform.right * speed * Time.deltaTime);
		else transform.Translate(transform.right * speed * Time.deltaTime * -1);
       
    }
	
	void DestroyBullet()
	{
		Destroy(gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.transform.tag.Equals("Enemy"))
		{
			collision.transform.GetComponent<Enemy>().TakeDamage(2f);
            Destroy(gameObject);			
		}
	}
}
