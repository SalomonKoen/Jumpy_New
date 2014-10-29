using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetupScript : MonoBehaviour {

	private GameObject player;
	public GameObject gameGenerator;

	public List<GameObject> players = new List<GameObject>();

	void Start ()
    {
		//TEMP
		player = (GameObject)Instantiate(players[0]);

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		int c = activity.Call<int>("getCharacter");

		//player = (GameObject)Instantiate(players[c]);

		int n = activity.Call<int>("getPowerupsCount");

		Powerup[] powerups = new Powerup[n];

		for (int i = 0; i < n; i++)
		{
			AndroidJavaObject p = activity.Call<AndroidJavaObject>("getPowerup", i);

			Powerup powerup = new Powerup(p.Call<int>("getQuantity"), p.Call<int>("getType"), p.Call<double>("getValue"));
			powerups[i] = powerup;
		}

		player.GetComponent<PlayerScript>().setPowerups(powerups);

		int m = activity.Call<int>("getHighScoresCount");
		
		List<HighScore> highscores = new List<HighScore>();
		
		for (int i = 0; i < m; i++)
		{
			AndroidJavaObject h = activity.Call<AndroidJavaObject>("getHighScore", i);
			
			HighScore highscore = new HighScore(h.Call<string>("getName"), h.Call<int>("getHeight"));
			highscores.Add(highscore);

		}

		gameGenerator.GetComponent<GameGenerator>().setHighscores(highscores);
	}
}
