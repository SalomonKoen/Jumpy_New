package com.example.jumpy;

import java.util.ArrayList;
import java.util.List;

import com.parse.FindCallback;
import com.parse.ParseException;
import com.parse.ParseObject;
import com.parse.ParseQuery;
import com.parse.ParseUser;

import android.app.Activity;
import android.content.Context;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

public class HighScoreActivity extends Activity 
{
	Context context = this;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);

		setContentView(R.layout.activity_high_score);

		ListView highscores = (ListView)findViewById(R.id.lvHighScores);
		
		JumpyApplication application = (JumpyApplication)getApplication();
		
		List<HighScore> items = application.getHelper().getHighScores(application.getPlayer().getId());

		HighScoreAdapter adapter = new HighScoreAdapter(this, items);
		highscores.setAdapter(adapter);
		
		Button local = (Button)findViewById(R.id.btnLocal);
		local.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				ListView highscores = (ListView)findViewById(R.id.lvHighScores);
				
				JumpyApplication application = (JumpyApplication)getApplication();
				
				List<HighScore> items = application.getHelper().getHighScores(application.getPlayer().getId());

				HighScoreAdapter adapter = new HighScoreAdapter(context, items);
				highscores.setAdapter(adapter);
			}
		});
		
		Button online = (Button)findViewById(R.id.btnOnline);
		online.setOnClickListener(new OnClickListener() {
			
			@Override
			public void onClick(View v) {
				// TODO Auto-generated method stub
				ParseQuery<ParseObject> query = ParseQuery.getQuery("Highscore");
				query.orderByAscending("UserScore");
				query.setLimit(10);
				query.findInBackground(new FindCallback<ParseObject>() {

					@Override
					public void done(List<ParseObject> arg0, ParseException arg1) {
						// TODO Auto-generated method stub
						List<HighScore> onlineitems = new ArrayList<HighScore>();
						
						for (int i = 0; i < arg0.size(); i++)
						{
							ParseObject po = arg0.get(i);
							onlineitems.add(new HighScore(0, po.getString("UserName"), 0, po.getInt("UserScore")));
						}
						
						ListView highscores = (ListView)findViewById(R.id.lvHighScores);
						HighScoreAdapter adapter = new HighScoreAdapter(context, onlineitems);
						highscores.setAdapter(adapter);
					}
				});
				
			}
		});

	}
}
