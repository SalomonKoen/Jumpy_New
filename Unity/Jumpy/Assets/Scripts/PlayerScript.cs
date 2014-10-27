using UnityEngine;
using System.Collections;
using Parse;

public class PlayerScript : MonoBehaviour {

	public static float speed = 1f;

    public float jumpForce = 10f;

	public static bool Pause = false;

    public float damp = 20;

    private bool jump = true;
	private bool inverted = false;
	
	public GameObject bullet;

	public Animator animator;

	public Transform gun;

	public float fireRate = 0.2f;
	private float nextFire = 0.0F;

	private bool move = true;

	private Powerup[] powerups;

	private static Transform curTransform;

	public static Transform getTransform()
	{
		return curTransform;
	}

	private BoxCollider2D box;
	private float boxWidth;

	public static float distance = 0;

    void Start()
    {
		curTransform = transform;
		box = transform.GetChild(0).GetComponent<BoxCollider2D>();
		boxWidth = box.size.x*transform.localScale.x;
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
			if (Input.touchCount > 0 && Time.time > nextFire)
			{
				nextFire = Time.time + fireRate;
				Vector2 direction = (Input.GetTouch(0).position - new Vector2(Screen.width / 2, 0)).normalized;
				direction = new Vector2(Mathf.Clamp(direction.x, -5, 5), Mathf.Clamp (direction.y, 1, 5));

				var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
				gun.rotation = Quaternion.AngleAxis(angle - 32, Vector3.forward);
				
				GameObject obj = (GameObject)Instantiate(bullet, new Vector2(gun.position.x, gun.position.y), Quaternion.identity);
				BulletScript script = obj.GetComponent<BulletScript>();
				script.setDirection(direction);
				script.Move();
			}

			if (Application.platform != RuntimePlatform.Android)
			{
				if (Input.GetButtonDown("Fire1"))
				{
					nextFire = Time.time + fireRate;
					Vector3 direction = (Input.mousePosition - new Vector3(Screen.width / 2, 0)).normalized;
					direction = new Vector2(Mathf.Clamp(direction.x, -5, 5), Mathf.Clamp (direction.y, 1, 5));

					var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

					Quaternion rot = Quaternion.AngleAxis(angle - 32, Vector3.forward);

					Debug.Log(gun.rotation.z);

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

	        if (jump)
	        {
	            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, jumpForce*speed);
				animator.SetBool("Fall", false);
				animator.SetBool ("Jump", true);
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
			//sendData();
			GameObject c = GameObject.Find("Main Camera");
			c.AddComponent("ShowSummary");
            Time.timeScale = 0;
        }
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

	public void setPowerups(Powerup[] powerups)
	{
		this.powerups = powerups;
	}

	private void sendData()
	{
		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
		
		activity.Call("setHighScore", ScrollingScript.getHeight(), PlayerCollisionScript.getKills());
		activity.Call("setPowerups", convertPowerups());
    }

	private int[] convertPowerups()
	{
		int[] p = new int[powerups.Length];

		for (int i = 0; i < powerups.Length;i++)
		{
			p[i] = powerups[i].getQuantity();

			if (transform.position.y < 0)
			{
				GameObject c = GameObject.Find("Main Camera");
				c.AddComponent("ShowSummary");

				Time.timeScale = 0;

					
				ParseObject scoreObject = new ParseObject("Highscore");
				scoreObject["UserName"] = "Martin";
				scoreObject["UserScore"] = Mathf.Ceil(distance/10);
				scoreObject.SaveAsync();
			}
		}

		return p;
    }
}
