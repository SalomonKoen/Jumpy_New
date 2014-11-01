using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HUDScript : MonoBehaviour {

	public Texture btnTexture;
	public Texture btnTexture2;
	public Texture btnTexture3;
	public Texture btnTexture4;

	public Texture playTexture;
	public Texture pauseTexture;

	public GameObject TopFrame;

	private Texture texture;

	public GameObject player;

	public GameGenerator gameGenerator;

	public bool showGUI = true;

	public bool hide = false;

	float timeLeft = 10.0f;

	private bool play = true;

	float timeScale = 1.0f;

	// Use this for initialization
	void Start () {
		texture = pauseTexture;
		play = true;
		showGUI = true;
	}
	
	// Update is called once per frame
	void Update () {
		timeLeft -= Time.deltaTime;
		if(timeLeft < 0)
		{
			//btnTexture.enabled = true;
			showGUI = true;
		}
	}

	void OnGUI () {
		if (hide)
		{
			Color c = TopFrame.renderer.material.color;
			c.a = 0;

			TopFrame.renderer.material.color = c;
		}
		else
		{
			Color c = TopFrame.renderer.material.color;
			c.a = 0.58f;
			
			TopFrame.renderer.material.color = c;
		GUI.contentColor = Color.white;
		GUIStyle style = new GUIStyle();
		style.fontSize = 75;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		GUI.Label(new Rect(39,20,300,280), "Height: " + ScrollingScript.getHeight(), style);

		GUIStyle newStyle = new GUIStyle();
		newStyle.fontSize = 30;
		newStyle.fontStyle = FontStyle.Bold;
		newStyle.normal.textColor = Color.white;

		// Make a background box
		//GUI.Box(new Rect(10,10,100,90), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(Screen.width-110,10,100,100), texture)) {
			if (play)
			{
				timeScale = Time.timeScale;
				Time.timeScale = 0;
				play = false;
				texture = playTexture;
			}
			else
			{
				Time.timeScale = timeScale;
				play = true;
				texture = pauseTexture;
			}
		}

		List<Powerup> powerups = player.GetComponent<PlayerScript>().getPowerups();

		if (powerups[1].getQuantity() == 0)
			GUI.color = Color.grey;
		else
			GUI.color = Color.white;

		if (!btnTexture && showGUI) {
			if (GUI.Button(new Rect(Screen.width/2-300, Screen.height-150, 150, 150), "Click") && powerups[1].getQuantity() > 0)
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Supershooter();
				powerups[1].use();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width/2-300, Screen.height-150, 150, 150), btnTexture) && powerups[1].getQuantity() > 0)
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Supershooter();
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow1").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
				powerups[1].use();
			}
		}

		if (powerups[0].getQuantity() == 0)
			GUI.color = Color.grey;
		else
			GUI.color = Color.white;

		if (!btnTexture2 && showGUI) {

			if (GUI.Button(new Rect(Screen.width/2-150, Screen.height-150, 150, 150), "Click") && powerups[0].getQuantity() > 0)
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Slowdown(0.6f);
				powerups[0].use();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width/2-150, Screen.height-150, 150, 150), btnTexture2) && powerups[0].getQuantity() > 0)
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Slowdown(0.6f);
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow2").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
				powerups[0].use();
			}
		}	

		if (powerups[2].getQuantity() == 0)
			GUI.color = Color.grey;
		else
			GUI.color = Color.white;

		if (!btnTexture3 && showGUI) {
			if (GUI.Button(new Rect(Screen.width/2, Screen.height-150, 150, 150), "Click") && powerups[2].getQuantity() > 0)
			{
				timeLeft = 30.0f;
				showGUI = false;
				gameGenerator.noenemies();
				powerups[2].use();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width/2, Screen.height-150, 150, 150), btnTexture3) && powerups[2].getQuantity() > 0)
			{
				timeLeft = 30.0f;
				showGUI = false;
				gameGenerator.noenemies();
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow3").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
				powerups[2].use();
			}
		}	

		if (powerups[3].getQuantity() == 0)
			GUI.color = Color.grey;
		else
			GUI.color = Color.white;

		if (!btnTexture4 && showGUI) {
			if (GUI.Button(new Rect(Screen.width/2+150, Screen.height-150, 150, 150), "Click") && powerups[3].getQuantity() > 0)
			{
				timeLeft = 15.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Indestructible();
				powerups[3].use();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width/2+150, Screen.height-150, 150, 150), btnTexture4) && powerups[3].getQuantity() > 0)
			{
				timeLeft = 15.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Indestructible();
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow1").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
				powerups[3].use();
			}
		}
		}
	}
}
