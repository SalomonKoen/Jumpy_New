using UnityEngine;
using System.Collections;

public class ShowSummary : MonoBehaviour {

	// Use this for initialization
	void Start () {
		FB.Init(null);
		GameObject enemySound = GameObject.Find("Sounds").transform.Find("Restart").gameObject;
		AudioSource ac = enemySound.GetComponent<AudioSource>();
		ac.audio.Play();
		Destroy(GameObject.Find("Main Camera").GetComponent<HUDScript>());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI () {
		// Make a background box
		GUI.Box(new Rect(10,10,Screen.width-20,Screen.height-20), "Summary");

		GUIStyle centeredTextStyle = new GUIStyle("label");
		centeredTextStyle.alignment = TextAnchor.MiddleCenter;
		GUI.Label(new Rect(0,50,Screen.width,100), "Kills: " + PlayerCollisionScript.getKills(), centeredTextStyle);

		GUI.Label(new Rect(0,50,Screen.width,150), "Distance: " + ScrollingScript.getHeight(),centeredTextStyle);

		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,40,Screen.width-40,20), "Restart")) {
			PlayerScript.distance = 0;
			Time.timeScale = 1;

			Application.LoadLevel(0);
		}

		if (!FB.IsLoggedIn)                                                                                              
		{                                                          
			//GUI.Label((new Rect(179 , 11, 287, 160)), "Login to Facebook");             
			if (GUI.Button(new Rect(20,70,Screen.width-40,20), "Login to Facebook"))                                      
			{                                                                                                            
				FB.Login("email,publish_actions", LoginCallback);                                                        
			}                                                                                                            
		} 
		
		// Make the second button.
		else if(GUI.Button(new Rect(20,70,Screen.width-40,20), "Share")) {
			if (FB.IsLoggedIn)
			{
				FB.Feed(                                                                                                                 
				        linkCaption: "I just reached " + PlayerScript.distance + "! Can you beat it?",               
				        picture: "http://www.friendsmash.com/images/logo_large.jpg",                                                     
				        linkName: "Checkout my Jumpy greatness!",                                                                 
				        link: "http://apps.facebook.com/" + FB.AppId + "/?challenge_brag=" + (FB.IsLoggedIn ? FB.UserId : "guest")       
				        );     
			}
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
