﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameGenerator : MonoBehaviour
{
    public List<GameObject> leftBackgrounds = new List<GameObject>();
    public List<GameObject> rightBackgrounds = new List<GameObject>();
    public List<GameObject> backgrounds = new List<GameObject>();
    public List<GameObject> platforms = new List<GameObject>();
	public List<GameObject> enemies = new List<GameObject>();

	public GameObject lBackground;
	public GameObject rBackground;
	public GameObject background;

    private GameObject curPlatform;
	private GameObject nextPlatform;

	private bool started = false;

	private bool increase = true;

	private float enemyProb = 0.05f;

    private float area = 1f;
	private float lowerArea = 0f;

	private int cur = 0;

	private bool combine = false;

	private int newHeight = 40;
	//100
	private bool inbetween = true;

    private Vector2 size;
    private GameObject maxObj = null;

	public Transform scorePrefab;

	private List<HighScore> highscores = new List<HighScore>();

	void Start()
    {
        curPlatform = platforms[0];
        size = Camera.main.ScreenToWorldPoint(new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight));

		GameObject b = (GameObject)Instantiate(backgrounds[0]);
		b.transform.parent = background.transform;

		GameObject lb = (GameObject)Instantiate(leftBackgrounds[0]);
		lb.transform.parent = lBackground.transform;

		GameObject rb = (GameObject)Instantiate(rightBackgrounds[0]);
		rb.transform.parent = rBackground.transform;

		background.GetComponent<ScrollingScript>().Restart();
		lBackground.GetComponent<ScrollingScript>().Restart();
		rBackground.GetComponent<ScrollingScript>().Restart();

		this.cur++;

        float fourth = size.x / 2;
        float cur = -fourth;
        for (int i = 1; i <= 3; i++)
        {
			Instantiate(curPlatform, new Vector3(cur, curPlatform.renderer.bounds.size.y), Quaternion.identity);
            cur += fourth;
        }

		float curPoint = curPlatform.renderer.bounds.size.y;

        while (true)
        {
			curPoint += Random.Range(0, area) + curPlatform.renderer.bounds.size.y;

            if (curPoint > size.y)
                break;

			maxObj = (GameObject)Instantiate(curPlatform, new Vector3(Random.Range(-size.x+curPlatform.renderer.bounds.size.x/2, size.x-curPlatform.renderer.bounds.size.x/2), curPoint), Quaternion.identity);
        }

		highscores = new List<HighScore>();
		highscores.Add(new HighScore("Salomon", 210));
		highscores.Add(new HighScore("Salomon", 200));
		highscores.Add(new HighScore("Salomon", 190));
		highscores.Add(new HighScore("Salomon", 180));
		highscores.Add(new HighScore("Salomon", 170));
		highscores.Add(new HighScore("Salomon", 160));
		highscores.Add(new HighScore("Salomon", 150));
		highscores.Add(new HighScore("Salomon", 140));
		highscores.Add(new HighScore("Salomon", 130));
		highscores.Add(new HighScore("Salomon", 120));
		highscores.Add(new HighScore("Salomon", 110));
		highscores.Add(new HighScore("Salomon", 100));
		highscores.Add(new HighScore("Salomon", 90));
		highscores.Add(new HighScore("Salomon", 80));
		highscores.Add(new HighScore("Salomon", 70));
		highscores.Add(new HighScore("Salomon", 60));
		highscores.Add(new HighScore("Salomon", 50));
		highscores.Add(new HighScore("Salomon", 40));
		highscores.Add(new HighScore("Salomon", 30));
		highscores.Add(new HighScore("Salomon", 20));
		highscores.Add(new HighScore("Salomon", 10));

		for(int i = 0; i < highscores.Count; i++)
		{
			HighScore h = highscores[i];
			Transform score = (Transform)Instantiate(scorePrefab, new Vector3(0, h.getHeight() + Camera.main.ScreenToWorldPoint(new Vector2(0, Camera.main.pixelHeight/2)).y, scorePrefab.position.z), Quaternion.identity);
			TextMesh text = score.transform.GetChild(0).GetComponent<TextMesh>();
			text.text = (i+1).ToString() + " - " + h.getName();
		}

		spawnObjects();
	}

	public void setHighscores(List<HighScore> highscores)
	{
		this.highscores = highscores;
	}
	
	void Update ()
    {
		started = true;
        if (maxObj.transform.position.y < size.y)
		{
			spawnObjects();
		}
	}

	public void setHeight(int newHeight)
	{
		this.newHeight = newHeight;
	}

    public void spawnObjects()
    {
        float curPoint = size.y;

		if (ScrollingScript.getHeight() >= newHeight)
		{
			if (inbetween && !ScrollingScript.isTransition)
			{
					nextPlatform = null;

					while (nextPlatform == null)
					{
						if (cur == platforms.Capacity*2)
							cur = 0;

						nextPlatform = platforms[(cur+1)/2];
					}

				if (started )
				{
					background.GetComponent<ScrollingScript>().Transition((GameObject)Instantiate(backgrounds[cur]), (GameObject)Instantiate(backgrounds[cur+1]));
					lBackground.GetComponent<ScrollingScript>().Transition((GameObject)Instantiate(leftBackgrounds[cur]), (GameObject)Instantiate(leftBackgrounds[cur+1]));
					rBackground.GetComponent<ScrollingScript>().Transition((GameObject)Instantiate(rightBackgrounds[cur]), (GameObject)Instantiate(rightBackgrounds[cur+1]));

					cur++;
					cur++;
				}

				combine = true;

				inbetween = !inbetween;
			}
			else if (!inbetween && !ScrollingScript.isTransition)
			{
				combine = false;
				newHeight = ScrollingScript.getHeight() + 10;

				curPlatform = nextPlatform;
				enemyProb *= 1.3f;

				PlayerScript.speed *= 1.1f;
				if (area < 8 && increase)
				{
					area *= 1.2f;

					if (area > 8)
					{
						area /= 1.2f;
						increase = false;
					}
					lowerArea += 0.1f;
				}

				inbetween = !inbetween;

				if (!increase)
				{
					enemyProb *= 1.3f;
				}
			}
		}

		GameObject platform = curPlatform;

        while (true)
        {
            curPoint += Random.Range(lowerArea, area) + platform.renderer.bounds.size.y;
            
            if (curPoint > size.y*2)
                break;

			if (combine)
			{
				int n = Random.Range (0, 2);

				if (n == 0)
					platform = curPlatform;
				else
					platform = nextPlatform;
			}

			maxObj = (GameObject)Instantiate(platform, new Vector3(Random.Range(-size.x + platform.renderer.bounds.size.x / 2, size.x - platform.renderer.bounds.size.x / 2), curPoint), Quaternion.identity);
        	
			if (Random.Range(0f, 1f) < enemyProb)
			{
				GameObject enemy = enemies[Random.Range(0, enemies.Count)];
				Transform enemyChild = enemy.transform.GetChild(0);
				Instantiate(enemy, new Vector3(maxObj.transform.position.x, maxObj.transform.position.y + enemyChild.renderer.bounds.size.y/2, enemy.transform.position.z), Quaternion.identity);		
			}
		}
    }
}
