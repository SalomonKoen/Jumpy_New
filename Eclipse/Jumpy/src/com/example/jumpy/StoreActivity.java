package com.example.jumpy;

import android.app.Activity;
import android.os.Bundle;
import android.widget.ListView;
import android.widget.TextView;

public class StoreActivity extends Activity
{
	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_store);
		
		JumpyApplication application = (JumpyApplication)getApplication();
		
		Player player = application.getPlayer();
		
		TextView txtCoins = (TextView)findViewById(R.id.txtCoins);
		txtCoins.setText(Integer.toString(player.getCoins()));
		
		StoreAdapter adapter = new StoreAdapter(this, player);
		
		ListView listView = (ListView)findViewById(R.id.list);
		listView.setAdapter(adapter);
	}
	
	public void buyItem()
	{
		JumpyApplication application = (JumpyApplication)getApplication();
		
		Player player = application.getPlayer();
		
		TextView txtCoins = (TextView)findViewById(R.id.txtCoins);
		txtCoins.setText(Integer.toString(player.getCoins()));
	}
	
	@Override
	protected void onPause()
	{
		JumpyApplication application = (JumpyApplication)this.getApplication();
		
		if (!this.isFinishing())
		{
			application.pause();
		}
		
		application.getHelper().savePlayer(application.getPlayer());
		
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
