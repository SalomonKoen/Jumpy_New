package com.example.jumpy;

import java.util.ArrayList;

import android.os.Bundle;

import com.unity3d.player.UnityPlayerActivity;

public class GameActivity extends UnityPlayerActivity
{
	private JumpyApplication application;
	
    @Override
    public void onCreate(Bundle savedInstanceState)
    {
    	application = (JumpyApplication)getApplication();
        super.onCreate(savedInstanceState);
    }
    
    public ArrayList<HighScore> getHighScores()
    {
    	return application.getHelper().getHighScores();
    }
    
    public void setHighScore(int height, int kills)
    {
    	application.getHelper().addHighScore(new HighScore(application.getPlayer().getId(), application.getPlayer().getName(), kills, height));
    }
    
    public HighScore getHighScore(int index)
    {
    	return application.getHelper().getHighScore(index);
    }
    
    public int getHighScoresCount()
    {
    	return application.getHelper().getHighScoresCount();
    }
    
    public Powerup getPowerup(int index)
    {
    	return application.getPlayer().getPowerups().get(index);
    }
    
    public void setPowerups(int[] powerups)
    {
    	application.getPlayer().setPowerups(powerups);
    }
    
    public int getPowerupsCount()
    {
    	return application.getPlayer().getPowerups().size();
    }
    
    public int getCharacter()
    {
    	return application.getPlayer().getCharacter();
    }
}

