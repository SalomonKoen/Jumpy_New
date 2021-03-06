package com.example.jumpy;

import android.app.Activity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.RadioGroup;
import android.widget.SeekBar;
import android.widget.SeekBar.OnSeekBarChangeListener;

public class SettingsActivity extends Activity
{
	private RadioGroup rgGraphics = null;
	private SeekBar seekMusic = null;
	private SeekBar seekEffects = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_settings);
			
		seekMusic =  (SeekBar)findViewById(R.id.seekMusic);
		seekEffects =  (SeekBar)findViewById(R.id.seekEffects);
		
		seekMusic.setOnSeekBarChangeListener(new OnSeekBarChangeListener()
		{
			
			@Override
			public void onStopTrackingTouch(SeekBar seekBar)
			{
				
			}
			
			@Override
			public void onStartTrackingTouch(SeekBar seekBar)
			{
				
			}
			
			@Override
			public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser)
			{
				
			}
		});
		
		seekEffects.setOnSeekBarChangeListener(new OnSeekBarChangeListener()
		{
			
			@Override
			public void onStopTrackingTouch(SeekBar seekBar)
			{
				
			}
			
			@Override
			public void onStartTrackingTouch(SeekBar seekBar)
			{
				
			}
			
			@Override
			public void onProgressChanged(SeekBar seekBar, int progress, boolean fromUser)
			{
				
			}
		});
		
		if (!Settings.isLoaded())
		{
			Settings.loadSettings(getSharedPreferences("Settings", 0), (JumpyApplication)this.getApplication());
		}
		
		seekMusic.setProgress(Settings.getMusic());
		seekEffects.setProgress(Settings.getEffects());
	}

	@Override
	public boolean onCreateOptionsMenu(Menu menu)
	{
		getMenuInflater().inflate(R.menu.main, menu);
		return true;
	}

	@Override
	public boolean onOptionsItemSelected(MenuItem item)
	{
		int id = item.getItemId();
		if (id == R.id.action_settings)
		{
			return true;
		}
		return super.onOptionsItemSelected(item);
	}
	
	public void onSaveClick(View view)
	{	
		Settings.saveSettings(getSharedPreferences("Settings", 0), seekMusic.getProgress(), seekEffects.getProgress());
		
		finish();
	}
	
	public void onCancelClick(View view)
	{
		finish();
	}
	
	@Override
	protected void onPause()
	{
		if (this.isFinishing())
		{
			JumpyApplication application = (JumpyApplication)this.getApplication();
			application.pause();
		}
		
		super.onPause();
	}
	
	@Override
	protected void onResume()
	{
		JumpyApplication application = (JumpyApplication)this.getApplication();
		application.resume();
		
		super.onResume();
	}
}
