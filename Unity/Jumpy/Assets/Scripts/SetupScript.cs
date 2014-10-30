using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetupScript : MonoBehaviour {

	private GameObject player;
	public GameObject gameGenerator;

	public int effects = 100;
	public int music = 100;

	public List<GameObject> players = new List<GameObject>();

	void Start ()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
		AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

		effects = activity.Call<int>("getEffectsVolume");
		music = activity.Call<int>("getMusicVolume");

		int c = activity.Call<int>("getCharacter");

		player = (GameObject)Instantiate(players[c]);

		string name = activity.Call<string>("getPlayer");
		player.GetComponent<PlayerScript>().name = name;

		GameObject cam = GameObject.Find("Main Camera");
		cam.GetComponent<HUDScript>().player = player;

		int n = activity.Call<int>("getPowerupsCount");

		List<Powerup> powerups = new List<Powerup>();

		for (int i = 0; i < n; i++)
		{
			AndroidJavaObject p = activity.Call<AndroidJavaObject>("getPowerup", i);

			Powerup powerup = new Powerup(p.Call<int>("getQuantity"), p.Call<int>("getType"), p.Call<double>("getValue"));
			powerups.Add(powerup);
		}

		player.GetComponent<PlayerScript>().setPowerups(powerups);

		int m = activity.Call<int>("getHighScoresCount");

		List<HighScore> highscores = new List<HighScore>();

		for (int j = 0; j < m; j++)
		{
			AndroidJavaObject h = activity.Call<AndroidJavaObject>("getHighScore", j);
			
			HighScore highscore = new HighScore(h.Call<string>("getName"), h.Call<int>("getHeight"));
			highscores.Add(highscore);

		}

		gameGenerator.GetComponent<GameGenerator>().setHighscores(highscores);
	}
}
