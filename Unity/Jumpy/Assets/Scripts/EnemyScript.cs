using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour
{ 
	public int health = 1;

	void Start ()
	{
		
	}
	
	void Update ()
    {
		if (transform.parent.position.y + GetComponent<BoxCollider2D>().size.y/2 < 0)
		{
			Destroy (this.transform.parent.gameObject);
		}
	}

	public void Damage(int damage)
	{
		health -= damage;

		if (health <= 0)
		{
			Destroy (this.transform.parent.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject enemySound = GameObject.Find("Sounds").transform.Find("Enemy").gameObject;
		AudioSource ac = enemySound.GetComponent<AudioSource>();
		ac.audio.Play();
	}
}
