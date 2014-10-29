using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

	public Texture btnTexture;
	public Texture btnTexture2;
	public Texture btnTexture3;
	public Texture btnTexture4;

	public bool showGUI = true;

	float timeLeft = 10.0f;

	// Use this for initialization
	void Start () {
	
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
		GUI.Label(new Rect(10,10,100,90), "Distance: " + Mathf.Ceil(PlayerScript.distance/10));

		// Make a background box
		//GUI.Box(new Rect(10,10,100,90), "Loader Menu");

		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed

		string text;
		if (PlayerScript.Pause)
			text = "RESUME";
		else
			text = "PAUSE";

		if(GUI.Button(new Rect(20,40,80,20), text)) {
			PlayerScript.Pause = !PlayerScript.Pause;
		}

		if (!btnTexture && showGUI) {
			if (GUI.Button(new Rect(Screen.width/3-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 10.0f;
				showGUI = false;
			}
		}
		else if (showGUI) 
		{
			GUI.Button(new Rect(Screen.width/3-50, Screen.height-50, 50, 50), btnTexture);
		}		

		if (!btnTexture2 && showGUI) {
			if (GUI.Button(new Rect(Screen.width/2-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 10.0f;
				showGUI = false;
			}
		}
		else if (showGUI) 
		{
			GUI.Button(new Rect(Screen.width/2-50, Screen.height-50, 50, 50), btnTexture2);
		}	

		if (!btnTexture3 && showGUI) {
			if (GUI.Button(new Rect(Screen.width*2/3-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 10.0f;
				showGUI = false;
			}
		}
		else if (showGUI) 
		{
			GUI.Button(new Rect(Screen.width*2/3-50, Screen.height-50, 50, 50), btnTexture3);
		}	

		if (!btnTexture4 && showGUI) {
			if (GUI.Button(new Rect(Screen.width*5/6-50, Screen.height-50, 50, 30), "Click"))
			{
				timeLeft = 10.0f;
				showGUI = false;
			}
		}
		else if (showGUI) 
		{
			GUI.Button(new Rect(Screen.width*5/6-50, Screen.height-50, 50, 50), btnTexture4);
		}

	}
}
