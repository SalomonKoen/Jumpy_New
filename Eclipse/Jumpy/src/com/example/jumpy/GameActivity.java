package com.example.jumpy;

import java.util.ArrayList;

import android.content.Intent;
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
    	int i = application.getPlayer().getPowerups().size();
    	return i;
    }
    
    public String getPlayer()
    {
    	return application.getPlayer().getName();
    }
    
    public int getCharacter()
    {
    	return application.getPlayer().getCharacter();
    }
    
    public void finishActivity()
    {
    	finish();
    }

    @Override
    protected void onPause()
    {
    	super.onDestroy();
    	
    	JumpyApplication app = (JumpyApplication)this.getApplication();
		
		Settings.savePlayer(getSharedPreferences("Settings", 0), app.getPlayer().getId());
		
		if (this.isFinishing())
		{
			app.pause();
			
			app.getHelper().savePlayer(app.getPlayer());
			
			app.closeConnection();
		}
    }
    
    public int getEffectsVolume()
    {
    	return Settings.getEffects();
    }
    
    public int getMusicVolume()
    {
    	return Settings.getMusic();
    }
    
    public void addCoins(int coins)
    {
    	application.getPlayer().addCoins(coins);
    	application.getHelper().savePlayer(application.getPlayer());
    }
}

