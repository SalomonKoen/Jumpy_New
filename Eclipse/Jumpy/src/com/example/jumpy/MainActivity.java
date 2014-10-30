package com.example.jumpy;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

public class MainActivity extends Activity
{
	private Intent musicService;
	private boolean resumed = false;
	
	private Button btnFrog;
	private Button btnRabbit;
	private Button btnKangaroo;
	
	private TextView txtPlayerName;
	
	@Override
	protected void onStart()
	{
		super.onStart();
	}

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main_menu);
		
		txtPlayerName = (TextView)findViewById(R.id.txtPlayerName);
		
		btnFrog = (Button)findViewById(R.id.btnFrog);
		btnRabbit = (Button)findViewById(R.id.btnRabbit);
		btnKangaroo = (Button)findViewById(R.id.btnKangaroo);
		
		JumpyApplication app = (JumpyApplication)this.getApplication();
		
		SQLiteHelper helper = new SQLiteHelper(this);
		
		app.setHelper(helper);
		
		JumpyApplication application = (JumpyApplication)getApplication();
		application.setVolume(Settings.getMusic());
		application.resume();
	}
	
	private void ShowAlert()
	{
		AlertDialog.Builder alert = new AlertDialog.Builder(this);

		alert.setTitle("Create new Profile");
		alert.setMessage("Enter your name:");
		alert.setCancelable(false);

		// Set an EditText view to get user input 
		final EditText input = new EditText(this);
		alert.setView(input);

		alert.setPositiveButton("Ok", new DialogInterface.OnClickListener()
		{
			@Override
			public void onClick(DialogInterface dialog, int whichButton)
			{
				JumpyApplication app = (JumpyApplication)MainActivity.this.getApplication();
				
				SQLiteHelper helper = app.getHelper();
				
				String name = input.getText().toString();
				
				app.setPlayer(helper.addPlayer(name));
				
				txtPlayerName.setText(app.getPlayer().getName());
			 }
		});

		alert.show();
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
	
	public void onQuitClick(View view)
	{
		new AlertDialog.Builder(this)
        .setIcon(android.R.drawable.ic_delete)
        .setTitle("Quit")
        .setMessage("Are you sure you want to quit?")
        .setPositiveButton("Yes", new DialogInterface.OnClickListener() {

            @Override
            public void onClick(DialogInterface dialog, int which) {
                MainActivity.this.finish();
            }

        })
        .setNegativeButton("No", null)
        .show();
	}
	
	public void onPlayClick(View view)
	{
		Intent intent = new Intent(MainActivity.this, GameActivity.class);
		startActivity(intent);
	}
	
	public void onSettingsClick(View view)
	{
		Intent intent = new Intent(MainActivity.this, SettingsActivity.class);
		startActivity(intent);
	}
	
	public void onStoreClick(View view)
	{
		Intent intent = new Intent(MainActivity.this, StoreActivity.class);
		startActivity(intent);
	}
	
	public void onHighScoreClick(View view)
	{
		Intent intent = new Intent(MainActivity.this, HighScoreActivity.class);
		startActivity(intent);
	}
	
	public void onProfileClick(View view)
	{
		Intent intent = new Intent(MainActivity.this, ProfileActivity.class);
		startActivity(intent);
	}
	
	@Override
	protected void onPause()
	{
		JumpyApplication app = (JumpyApplication)this.getApplication();
		
		Settings.savePlayer(getSharedPreferences("Settings", 0), app.getPlayer().getId());
		
		if (this.isFinishing())
		{
			app.pause();
			
			app.getHelper().savePlayer(app.getPlayer());
			
			app.closeConnection();
		}
		
		super.onPause();
	} 	
	
	@Override
	protected void onResume()
	{
		JumpyApplication app = (JumpyApplication)this.getApplication();
		app.resume();
		
		if (!resumed)
		{
			resumed = true;
			
			if (!Settings.loadSettings(getSharedPreferences("Settings", 0), app))
				ShowAlert();
		}
		else
		{
			txtPlayerName.setText(app.getPlayer().getName());
		}
		
		super.onResume();
	}
	
	public void onKangarooClick(View view)
	{
		JumpyApplication app = (JumpyApplication)this.getApplication();
		app.getPlayer().setCharacter(1);
		view.setBackgroundResource(R.drawable.kangarooblue);
		btnFrog.setBackgroundResource(R.drawable.frog);
		btnRabbit.setBackgroundResource(R.drawable.rabbit);
	}
	
	public void onFrogClick(View view)
	{
		JumpyApplication app = (JumpyApplication)this.getApplication();
		app.getPlayer().setCharacter(0);
		view.setBackgroundResource(R.drawable.frogblue);
		btnRabbit.setBackgroundResource(R.drawable.rabbit);
		btnKangaroo.setBackgroundResource(R.drawable.kangaroo);
	}
	
	public void onRabbitClick(View view)
	{
		JumpyApplication app = (JumpyApplication)this.getApplication();
		app.getPlayer().setCharacter(2);
		view.setBackgroundResource(R.drawable.rabbitblue);
		btnFrog.setBackgroundResource(R.drawable.frog);
		btnKangaroo.setBackgroundResource(R.drawable.kangaroo);
	}
}
