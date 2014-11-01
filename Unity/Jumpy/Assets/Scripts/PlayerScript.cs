using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Parse;

public class PlayerScript : MonoBehaviour {

	public static float speed = 1f;

	public HUDScript hud;

    public float jumpForce = 10f;

	public string name = "";

	public static bool Pause = false;

    public float damp = 20;

	private int enemyLayer = 8;
	private int noEnemyLayer = 13;

    private bool jump = true;
	private bool inverted = false;
	
	public GameObject bullet;

	public Animator animator;

	public Transform gun;
	private float gunRotReset = 0f;

	public float fireRate = 0.5f;
	private float nextFire = 0.0F;

	private bool move = true;

	private bool indestructible = false;
	private bool supershooter = false;
	private bool slowdown = false;

	private float powerupTimer;

	private List<Powerup> powerups = new List<Powerup>();

	private static Transform curTransform;

	public static Transform getTransform()
	{
		return curTransform;
	}

	public static float distance = 0;

	public bool isIndestructible()
	{
		return indestructible;
	}

	public List<Powerup> getPowerups()
	{
		return powerups;
	}

    void Start()
    {
		Pause = false;
		distance = 0;
		curTransform = transform;
    }

	void Update()
	{
		if (Pause)
		{
			rigidbody2D.Sleep();
			return;
		}
		else if (!Pause && rigidbody2D.IsSleeping())
			rigidbody2D.WakeUp();

		if (move)
		{
			if (indestructible && Time.time - powerupTimer > 15f)
			{
				indestructible = false;
				transform.GetChild(0).gameObject.layer = enemyLayer;
			}
			else if (supershooter && Time.time - powerupTimer > 10f)
			{
				supershooter = false;
				fireRate = 0.5f;
			}
			else if (slowdown && Time.time - powerupTimer > 10f)
			{
				slowdown = false;
				Time.timeScale = 1.0f;
			}

			if (gunRotReset != 0)
				if (Time.time - gunRotReset > 0.5f)
					resetGun();

			if (Input.touchCount > 0 && Time.time > nextFire)
			{
				Rect rect = new Rect(Screen.width/2-100, Screen.height-50, 200, 60);
				Rect rect2 = new Rect(Screen.width-44,4, 40,40);
				
				if ((hud.showGUI && rect.Contains(new Vector2(Input.GetTouch(0).position.x, Screen.height - Input.GetTouch(0).position.y))) || rect2.Contains(new Vector2(Input.GetTouch(0).position.x, Screen.height - Input.GetTouch(0).position.y)))
					return;

				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Shoot").gameObject;
                AudioSource ac = shootSound.GetComponent<AudioSource>();
                ac.audio.Play();

				gunRotReset = Time.time;
				nextFire = Time.time + fireRate;
				Vector2 direction = (Input.GetTouch(0).position - new Vector2(Screen.width / 2, 0)).normalized;
				direction = new Vector2(Mathf.Clamp(direction.x, -5, 5), Mathf.Clamp (direction.y, 1, 5));

				var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

				Quaternion rot = Quaternion.AngleAxis(angle - 32, Vector3.forward);
				
				if (rot.z > 0.45 && !inverted)
				{
					inverted = true;
					gun.localScale = new Vector3(-gun.localScale.x, gun.localScale.y, gun.localScale.z);
				}
				else if (rot.z < 0.45 && inverted)
				{
					inverted = false;
					gun.localScale = new Vector3(-gun.localScale.x, gun.localScale.y, gun.localScale.z);
				}
				
				if (inverted)
				{
					rot = Quaternion.AngleAxis(angle + 32 + 180, Vector3.forward);
				}
				
				gun.rotation = rot;
				
				GameObject obj = (GameObject)Instantiate(bullet, new Vector2(gun.position.x, gun.position.y), Quaternion.identity);
				BulletScript script = obj.GetComponent<BulletScript>();
				script.setDirection(direction);
				script.Move();
			}

			if (Application.platform != RuntimePlatform.Android)
			{
				if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
				{
					Rect rect = new Rect(Screen.width/2-100, Screen.height-50, 200, 60);
					Rect rect2 = new Rect(Screen.width-44,4, 40,40);

					if ((hud.showGUI && rect.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y))) || rect2.Contains(new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y)))
						return;

					GameObject shootSound = GameObject.Find("Sounds").transform.Find("Shoot").gameObject;
					AudioSource ac = shootSound.GetComponent<AudioSource>();
					ac.audio.Play();

					gunRotReset = Time.time;
					nextFire = Time.time + fireRate;
					Vector3 direction = (Input.mousePosition - new Vector3(Screen.width / 2, 0)).normalized;
					direction = new Vector2(Mathf.Clamp(direction.x, -5, 5), Mathf.Clamp (direction.y, 1, 5));

					var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

					Quaternion rot = Quaternion.AngleAxis(angle - 32, Vector3.forward);

					if (rot.z > 0.45 && !inverted)
					{
						inverted = true;
						gun.localScale = new Vector3(-gun.localScale.x, gun.localScale.y, gun.localScale.z);
					}
					else if (rot.z < 0.45 && inverted)
					{
						inverted = false;
						gun.localScale = new Vector3(-gun.localScale.x, gun.localScale.y, gun.localScale.z);
					}

					if (inverted)
					{
						rot = Quaternion.AngleAxis(angle + 32 + 180, Vector3.forward);
					}

					gun.rotation = rot;
					
					GameObject obj = (GameObject)Instantiate(bullet, new Vector2(gun.position.x, gun.position.y), Quaternion.identity);
					BulletScript script = obj.GetComponent<BulletScript>();
					script.setDirection(direction);
					script.Move();
				}
			}
		}
	}

    void FixedUpdate()
    {
		if (Pause)
			return;
		
		if (move)
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				if (Input.GetKey (KeyCode.LeftArrow))
				{
					rigidbody2D.velocity = new Vector2(-4, rigidbody2D.velocity.y);
				}
				else if (Input.GetKey (KeyCode.RightArrow))
				{
					rigidbody2D.velocity = new Vector2(4, rigidbody2D.velocity.y);
				}
				else
				{
					rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
				}
			}

	        float x = Input.acceleration.x;

	        transform.position = new Vector3(Mathf.Lerp(transform.position.x, transform.position.x + x, Time.deltaTime * damp), transform.position.y, -1);

			if (indestructible && transform.position.y <= transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.size.y/2)
				jump = true;

			if (jump)
	        {
	            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce*speed);
				animator.SetBool("Fall", false);
				animator.SetBool ("Jump", true);
				GameObject jumpSound = GameObject.Find("Sounds").transform.Find("Jump").gameObject;
				AudioSource ac = jumpSound.GetComponent<AudioSource>();
				ac.audio.Play();
	            gameObject.layer = 8;
				transform.GetChild(1).gameObject.layer = 8;
	            jump = false;
	        }

	        if (rigidbody2D.velocity.y <= 0)
	        {
				animator.SetBool ("Jump", false);
				animator.SetBool("Fall", true);

	            gameObject.layer = 9;
				transform.GetChild(1).gameObject.layer = 9;
	        }

			float width = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth,0)).x;
			
			if (transform.position.x < -width)
			{
				transform.position = new Vector2(width, transform.position.y);
			}
			else if (transform.position.x > width)
			{
				transform.position = new Vector2(-width, transform.position.y);
			}
		}

		if (transform.position.y + transform.GetChild(0).GetComponent<BoxCollider2D>().bounds.size.y < 0)
		{
			if (!indestructible)
			{
	            Time.timeScale = 0;
				GameObject c = GameObject.Find("Main Camera");
				c.AddComponent("ShowSummary");
				sendData();
			}
			else
			{
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce*speed);
				animator.SetBool("Fall", false);
				animator.SetBool ("Jump", true);
				gameObject.layer = 8;
				transform.GetChild(1).gameObject.layer = 8;
				jump = false;
			}
        }
    }

	private void resetGun()
	{
		gunRotReset = Time.time;
		gun.rotation = Quaternion.AngleAxis(0, Vector3.forward);
	}

	public void Indestructible()
	{
		indestructible = true;
		transform.GetChild(0).gameObject.layer = noEnemyLayer;

		powerupTimer = Time.time;
	}

	public void Slowdown(float time)
	{
		slowdown = true;
		Time.timeScale = time;
		powerupTimer = Time.time;
	}

	public void Supershooter()
	{
		supershooter = true;
		fireRate = 0.05f;
		powerupTimer = Time.time;
	}

	public void setJump(bool jump)
	{
		this.jump = jump;
	}

	public void setMove(bool move)
	{
		this.move = move;
	}
    
	public bool isMoving()
	{
		return move;
	}

	public void setPowerups(List<Powerup> powerups)
	{
		this.powerups = powerups;
	}

	private void sendData()
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		
		activity.Call("setHighScore", ScrollingScript.getHeight(), PlayerCollisionScript.getKills());
		activity.Call("setPowerups", convertPowerups());
		activity.Call ("addCoins", (PlayerCollisionScript.getKills() * 10) + ScrollingScript.getHeight());
			
		ParseObject scoreObject = new ParseObject("Highscore");
		scoreObject["UserName"] = name;
		scoreObject["UserScore"] = ScrollingScript.getHeight();
		scoreObject.SaveAsync();
    }

	private int[] convertPowerups()
	{
		int[] p = new int[powerups.Capacity];

		for (int i = 0; i < powerups.Capacity;i++)
		{
			p[i] = powerups[i].getQuantity();
		}

		return p;
    }
}
