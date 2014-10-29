package com.example.jumpy;

import java.util.ArrayList;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.AlertDialog.Builder;
import android.content.DialogInterface;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ListView;

public class ProfileActivity extends Activity 
{
	Profile selected;
	ProfileAdapter adapter;
	
	private Button btnNewProfile;
	private Button btnChangeProfile;
	private Button btnDeleteProfile;
	
	private void ShowAlert(boolean cancelable)
	{
		AlertDialog.Builder alert = new AlertDialog.Builder(this);

		alert.setTitle("Create new Profile");
		alert.setMessage("Enter your name:");

		// Set an EditText view to get user input 
		final EditText input = new EditText(this);
		alert.setView(input);

		alert.setPositiveButton("OK", new DialogInterface.OnClickListener() 
		{
			@Override
			public void onClick(DialogInterface dialog, int whichButton)
			{
				JumpyApplication app = (JumpyApplication)ProfileActivity.this.getApplication();
				
				SQLiteHelper helper = app.getHelper();
				
				String name = input.getText().toString();
				
				Player player = helper.addPlayer(name);
				
				app.setPlayer(player);
				
				selected = null;
				
				Profile profile = new Profile(name, player.getId());
				adapter.add(profile);
				
				adapter.changeProfile(profile);
				
				btnChangeProfile.setEnabled(false);
				btnDeleteProfile.setEnabled(false);
			}
		});

		if (cancelable)
		{
			alert.setNegativeButton("Cancel", new DialogInterface.OnClickListener()
			{
				@Override
				public void onClick(DialogInterface dialog, int whichButton)
				{
		
				}
			});
		}
		
		alert.setCancelable(cancelable);

		alert.show();
	}
	
	private void ShowDeleteAlert()
	{
		AlertDialog.Builder alert = new AlertDialog.Builder(this);

		alert.setTitle("Delete Profile");
		alert.setMessage("Are you sure?");

		alert.setPositiveButton("OK", new DialogInterface.OnClickListener() 
		{
			@Override
			public void onClick(DialogInterface dialog, int whichButton)
			{
				JumpyApplication app = (JumpyApplication)ProfileActivity.this.getApplication();
				
				if (selected.isActive())
				{
					AlertDialog.Builder notifDialog = new AlertDialog.Builder(ProfileActivity.this);
					
					notifDialog.setTitle("Cannot Delete");
					notifDialog.setMessage("You cannot delete the active profile. Please change to another profile first.");
					notifDialog.setPositiveButton("OK", new DialogInterface.OnClickListener()
					{
						
						@Override
						public void onClick(DialogInterface dialog, int which)
						{
							
						}
					});
					
					notifDialog.show();
				}
				else
				{
					SQLiteHelper helper = app.getHelper();
					
					helper.removePlayer(selected.getPlayer_id());
					adapter.remove(selected);
					
					selected = null;
					
					btnDeleteProfile.setEnabled(false);
					btnChangeProfile.setEnabled(false);
				}
			}
		});

		alert.setNegativeButton("Cancel", new DialogInterface.OnClickListener()
		{
		  @Override
		public void onClick(DialogInterface dialog, int whichButton)
		  {

		  }
		});

		alert.show();
	}
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_profile);

		ListView listView = (ListView)findViewById(R.id.lvProfiles);
		
		JumpyApplication app = (JumpyApplication)getApplication();
		
		SQLiteHelper helper = app.getHelper();
		
		final ArrayList<Profile> profiles = helper.getProfiles(app.getPlayer().getId());

		// create adapter to transform string items
		adapter = new ProfileAdapter(this, profiles);
		
		// attach adapter to list view
		listView.setAdapter(adapter);
		
		btnNewProfile = (Button)findViewById(R.id.btnNew);
		btnNewProfile.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				ShowAlert(true);
			}
		});
		
		btnDeleteProfile = (Button)findViewById(R.id.btnDelete);
		btnDeleteProfile.setEnabled(false);
		btnDeleteProfile.setOnClickListener(new OnClickListener() {

			@Override
			public void onClick(View v) {
				ShowDeleteAlert();
				
			}
		});
		
		listView.setOnItemClickListener(new OnItemClickListener() {

			@Override
			public void onItemClick(AdapterView<?> parent, View view,
					int position, long id) {
				
				btnChangeProfile.setEnabled(true);
				btnDeleteProfile.setEnabled(true);
				
				if (selected != null)
				{
					selected.setSelected(false);
				}
				
				selected = profiles.get(position);
				selected.setSelected(true);
				
				if (selected.isActive())
				{
					btnChangeProfile.setEnabled(false);
				}
				else 
					btnChangeProfile.setEnabled(true);				
				
				adapter.notifyDataSetChanged();
			}
		});

		btnChangeProfile = (Button)findViewById(R.id.btnChange);
		btnChangeProfile.setEnabled(false);
		btnChangeProfile.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				JumpyApplication application = (JumpyApplication)getApplication();
				application.getHelper().savePlayer(application.getPlayer());
				application.setPlayer(selected);
				adapter.setActive(selected);
				btnChangeProfile.setEnabled(false);
			}
		});

	}
}
