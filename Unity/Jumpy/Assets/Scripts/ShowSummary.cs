using UnityEngine;
using System.Collections;

public class ShowSummary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FB.Init(null);
		GameObject enemySound = GameObject.Find("Sounds").transform.Find("Restart").gameObject;
		AudioSource ac = enemySound.GetComponent<AudioSource>();
		ac.audio.Play();
		GameObject.Find("Main Camera").GetComponent<HUDScript>().hide = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		GUIStyle textStyle = new GUIStyle("label");
		textStyle.alignment = TextAnchor.MiddleCenter;
		textStyle.fontSize = 80;
		textStyle.fontStyle = FontStyle.Bold;
		// Make a background box

		GUIStyle buttonStyle = new GUIStyle("button");
		buttonStyle.fontSize = 50;

		GUI.Box(new Rect(10,10,Screen.width-20,Screen.height-20), "");

		GUI.Label(new Rect(Screen.width/2-400, 50, 800, 150), "Summary", textStyle);

		GUIStyle centeredTextStyle = new GUIStyle("label");
		centeredTextStyle.alignment = TextAnchor.MiddleCenter;
		centeredTextStyle.fontSize = 50;
		centeredTextStyle.fontStyle = FontStyle.Bold;

		GUI.Label(new Rect(Screen.width/2-400,150, 800,150), "Height: " + ScrollingScript.getHeight(),centeredTextStyle);
		GUI.Label(new Rect(Screen.width/2-400,250, 800,150), "Kills: " + PlayerCollisionScript.getKills(), centeredTextStyle);


		if (!FB.IsLoggedIn)                                                                                              
		{                                                          
			//GUI.Label((new Rect(179 , 11, 287, 160)), "Login to Facebook");             
			if (GUI.Button(new Rect(Screen.width/2-300,400,600,130), "Login to Facebook", buttonStyle))                                      
			{                                                                                                            
				//FB.Login("email,publish_actions", LoginCallback);                                                        
			}                                                                                                            
		} 
		
		// Make the second button.
		else if(GUI.Button(new Rect(Screen.width/2-200,400,400,130), "Share", buttonStyle)) {
			if (FB.IsLoggedIn)
			{
				/*FB.Feed(                                                                                                                 
				        linkCaption: "I just reached " + PlayerScript.distance + "! Can you beat it?",               
				        picture: "http://www.friendsmash.com/images/logo_large.jpg",                                                     
				        linkName: "Checkout my Jumpy greatness!",                                                                 
				        link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")       
				        );*/     
			}
		}
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(Screen.width/2-200,550,400,130), "Restart", buttonStyle)) {
			PlayerScript.distance = 0;
			Time.timeScale = 1;

			Application.LoadLevel(0);
		}



		if(GUI.Button(new Rect(Screen.width/2-200,700,400,130), "Main Menu", buttonStyle)) {
			Application.Quit();
		}


	}

	void LoginCallback(FBResult result)                                                        
	{                                                                                                                                                
		
		if (FB.IsLoggedIn)                                                                     
		{                                                                                      
			OnLoggedIn();                                                                      
		}                                                                                      
	}                                                                                          
	
	void OnLoggedIn()                                                                          
	{                                                                                                                                    
	}  
}
