using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

	public Texture btnTexture;
	public Texture btnTexture2;
	public Texture btnTexture3;
	public Texture btnTexture4;

	public Texture playTexture;
	public Texture pauseTexture;

	private Texture texture;

	public GameObject player;

	public GameGenerator gameGenerator;

	public bool showGUI = true;

	float timeLeft = 10.0f;

	private bool play = true;

	float timeScale = 1.0f;

	// Use this for initialization
	void Start () {
		texture = pauseTexture;
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
		GUI.contentColor = Color.white;
		GUIStyle style = new GUIStyle();
		style.fontSize = 80;
		style.fontStyle = FontStyle.Bold;
		style.normal.textColor = Color.white;
		GUI.Label(new Rect(13,13,100,90), "Height: " + ScrollingScript.getHeight(), style);

		// Make a background box
		//GUI.Box(new Rect(10,10,100,90), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed

		if(GUI.Button(new Rect(Screen.width-44,4,40,40), texture)) {
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

		if (!btnTexture && showGUI) {
			if (GUI.Button(new Rect(Screen.width/3-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Supershooter();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width/3-50, Screen.height-50, 50, 50), btnTexture))
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Supershooter();
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow1").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
			}
		}		

		if (!btnTexture2 && showGUI) {
			if (GUI.Button(new Rect(Screen.width/2-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Slowdown(0.6f);
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width/2-50, Screen.height-50, 50, 50), btnTexture2))
			{
				timeLeft = 10.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Slowdown(0.6f);
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow2").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
			}
		}	

		if (!btnTexture3 && showGUI) {
			if (GUI.Button(new Rect(Screen.width*2/3-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 30.0f;
				showGUI = false;
				gameGenerator.noenemies();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width*2/3-50, Screen.height-50, 50, 50), btnTexture3))
			{
				timeLeft = 30.0f;
				showGUI = false;
				gameGenerator.noenemies();
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow3").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
			}
		}	

		if (!btnTexture4 && showGUI) {
			if (GUI.Button(new Rect(Screen.width*5/6-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 15.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Indestructible();
			}
		}
		else if (showGUI) 
		{
			if (GUI.Button(new Rect(Screen.width*5/6-50, Screen.height-50, 50, 50), btnTexture4))
			{
				timeLeft = 15.0f;
				showGUI = false;
				player.GetComponent<PlayerScript>().Indestructible();
				GameObject shootSound = GameObject.Find("Sounds").transform.Find("Pow1").gameObject;
				AudioSource ac = shootSound.GetComponent<AudioSource>();
				ac.audio.Play();
			}
		}
	}
}
