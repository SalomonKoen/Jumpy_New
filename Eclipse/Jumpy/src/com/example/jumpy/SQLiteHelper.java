package com.example.jumpy;

import java.util.ArrayList;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

public class SQLiteHelper extends SQLiteOpenHelper
{
	private static final int DATABASE_VERSION = 1;
	private static final String DATABASE_NAME = "UserData";
	
	public SQLiteHelper(Context context)
	{
		super(context, DATABASE_NAME, null, DATABASE_VERSION);
	}

	@Override
	public void onCreate(SQLiteDatabase db)
	{
		createPlayerTable(db);
		createHighScoreTable(db);
		createItemTable(db);
		createPowerupTable(db);
		createWeaponTable(db);
		createCharacterTable(db);
		createInventoryTable(db);
	}

	@Override
	public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
	{
		db.execSQL("DROP TABLE IF EXISTS HighScore;");
		db.execSQL("DROP TABLE IF EXISTS Player;");
		db.execSQL("DROP TABLE IF EXISTS Powerup;");
		db.execSQL("DROP TABLE IF EXISTS Weapon;");
		db.execSQL("DROP TABLE IF EXISTS Character;");
		db.execSQL("DROP TABLE IF EXISTS Item;");
		db.execSQL("DROP TABLE IF EXISTS Inventory;");
		
		this.onCreate(db);
	}
	
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	//CREATE TABLES
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	private void createHighScoreTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE HighScore ("
				+ "HighScore_id INTEGER PRIMARY KEY AUTOINCREMENT, "
				+ "player_id INTEGER REFERENCES Player(player_id), "
				+ "kills INTEGER, "
				+ "height INTEGER);";
		
		db.execSQL(sql);
	}
	
	private void createPlayerTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE Player ("
				+ "player_id INTEGER PRIMARY KEY AUTOINCREMENT, "
				+ "name TEXT, "
				+ "coins INTEGER);";
		
		db.execSQL(sql);
	}
	
	private void createItemTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE Item ("
				+ "item_id INTEGER PRIMARY KEY AUTOINCREMENT, "
				+ "name TEXT, "
				+ "description TEXT, "
				+ "price INTEGER, "
				+ "image INTEGER, "
				+ "type INTEGER CONSTRAINT check_type CHECK (type IN (0, 1, 2)), "
				+ "multiple BOOLEAN);";		
		
		db.execSQL(sql);
		
		String[] items = {
				"null, 'Time Slow', 'Slows down time for 10 seconds.', 200, " + R.drawable.item2 + ", 1, 1",
				"null, 'Super Shooter', 'Increases the rate of fire for 10 seconds.', 100, " + R.drawable.item1 + ", 1, 1",
				"null, 'No Enemies', 'Removes all enemies for 30 seconds.', 300, " + R.drawable.item3 + ", 1, 1",
				"null, 'Indestructible', 'Makes you indestructible for 15 seconds', 400, " + R.drawable.item4 + ", 1, 1",
		};
		
		for (String s : items)
		{
			String itemSql = "INSERT INTO Item VALUES (" + s + ");";
			db.execSQL(itemSql);
		}
	}
	
	private void createCharacterTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE Character ("
				+ "item_id INTEGER PRIMARY KEY REFERENCES Item(item_id), "
				+ "mass INTEGER, "
				+ "health INTEGER);";
		
		db.execSQL(sql);
	}
	
	private void createPowerupTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE Powerup ("
				+ "item_id INTEGER PRIMARY KEY REFERENCES Item(item_id), "
				+ "type INTEGER, "
				+ "value REAL);";
		
		db.execSQL(sql);
		
		String[] items = { 
				"1, 1, 0.7",
				"2, 1, 0",
				"3, 2, 0",
				"4, 3, 0"
		};
		
		for (String s : items)
		{
			String itemSql = "INSERT INTO Powerup VALUES (" + s + ");";
			db.execSQL(itemSql);
		}
	}
	
	private void createWeaponTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE Weapon ("
				+ "item_id INTEGER PRIMARY KEY REFERENCES Item(item_id), "
				+ "damage INTEGER);";
		
		db.execSQL(sql);
	}
	
	private void createInventoryTable(SQLiteDatabase db)
	{
		String sql = "CREATE TABLE Inventory ("
				+ "player_id INTEGER REFERENCES Player(player_id), "
				+ "item_id INTEGER REFERENCES Item(item_id), "
				+ "quantity INTEGER, "
				+ "PRIMARY KEY (player_id, item_id));";
		
		db.execSQL(sql);
	}
	
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	//HighScore
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public void addHighScore(HighScore highScore)
	{
		SQLiteDatabase db = this.getWritableDatabase();
		
		ContentValues values = new ContentValues();
		values.put("player_id", highScore.getPlayer_id());
		values.put("kills", highScore.getKills());
		values.put("height", highScore.getHeight());
		
		db.insert("HighScore", null, values);
		
		db.close();
	}
	
	public ArrayList<HighScore> getHighScores()
	{
		ArrayList<HighScore> highScores = new ArrayList<HighScore>();
		
		String sql = "SELECT HighScore.player_id, Player.name, HighScore.kills, HighScore.height FROM HighScore, Player "
				+ "WHERE HighScore.player_id=Player.player_id"
						+ " ORDER BY height DESC, kills;";
		
		SQLiteDatabase db = this.getReadableDatabase();
		Cursor cursor = db.rawQuery(sql, null);
		
		if (cursor.moveToFirst())
		{
			do {
				HighScore highScore = new HighScore(cursor.getInt(0), cursor.getString(1), cursor.getInt(2), cursor.getInt(3));
				highScores.add(highScore);
			} while (cursor.moveToNext());
		}
		
		db.close();
		
		return highScores;
	}
	
	public ArrayList<HighScore> getHighScores(int player_id, String name)
	{
		ArrayList<HighScore> highScores = new ArrayList<HighScore>();
		
		String sql = "SELECT HighScore.player_id, HighScore.kills, HighScore.height FROM HighScore"
				+ "WHERE HighScore.player_id=Player.player_id "
				+ "AND Player.player_id = " + player_id
						+ " ORDER BY height DESC, kills;";
		
		SQLiteDatabase db = this.getReadableDatabase();
		Cursor cursor = db.rawQuery(sql, null);
		
		if (cursor.moveToFirst())
		{
			do {
				HighScore highScore = new HighScore(cursor.getInt(0), name, cursor.getInt(1), cursor.getInt(2));
				highScores.add(highScore);
			} while (cursor.moveToNext());
		}
		
		db.close();
		
		return highScores;
	}
	
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	//INVENTORY
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	public ArrayList<Item> getItems()
	{
		ArrayList<Item> items = new ArrayList<Item>();
		
		String sql = "SELECT * FROM Item;";
		
		SQLiteDatabase db = this.getReadableDatabase();
		Cursor cursor = db.rawQuery(sql, null);
		
		if (cursor.moveToFirst())
		{
			do
			{
				Item item = null;
				
				int id = cursor.getInt(0);
				int type = cursor.getInt(5);
				String name = cursor.getString(1);
				String description = cursor.getString(2);
				int price = cursor.getInt(3);
				int image = cursor.getInt(4);
				boolean multiple = cursor.getInt(6) > 0;
				
				int quantity = 0;
				
				if (type == 0)
				{
					String sql2 = "SELECT * FROM Character "
							+ "WHERE item_id=" + id + ";";
					
					Cursor cursor2 = db.rawQuery(sql2, null);
					
					if (cursor2.moveToFirst())
					{
						item = new Character(id, name, description, multiple, price, image, quantity, cursor2.getInt(1), cursor2.getInt(2));
					}
				}
				else if (type == 1)
				{
					String sql2 = "SELECT * FROM Powerup "
							+ "WHERE item_id=" + id + ";";
					
					Cursor cursor2 = db.rawQuery(sql2, null);
					
					if (cursor2.moveToFirst())
					{
						item = new Powerup(id, name, description, multiple, price, image, quantity, cursor2.getInt(1), cursor2.getDouble(2));
					}
				}
				else if (type == 2)
				{
					String sql2 = "SELECT * FROM Weapon "
							+ "WHERE item_id=" + id + ";";
					
					Cursor cursor2 = db.rawQuery(sql2, null);
					
					if (cursor2.moveToFirst())
					{
						item = new Weapon(id, name, description, multiple, price, image,  quantity, cursor2.getInt(1));
					}
				}
				
				items.add(item);
				
			} while (cursor.moveToNext());
		}
		
		db.close();
		
		return items;
	}
	
	public Inventory getPlayerItems(ArrayList<Item> items, int player_id)
	{
		SQLiteDatabase db = this.getReadableDatabase();
		
		for (Item item : items)
		{
			String sql = "SELECT * FROM Inventory "
					+ "WHERE player_id=" + player_id
					+ " AND item_id=" + item.getId() + ";";
			
			Cursor cursor = db.rawQuery(sql, null);
			
			int quantity = 0;
			
			if (cursor.moveToFirst())
			{
				quantity = cursor.getInt(2);
			}
			
			item.setQuantity(quantity);
		}
		
		db.close();
		
		return new Inventory(items);
	}
	
	public void saveItems(Player player)
	{
		ArrayList<Item> items = player.getInventory().getItems();

		SQLiteDatabase db = this.getWritableDatabase();
		
		for (Item item : items)
		{
			String sql = "UPDATE Inventory "
					+ "SET quantity=" + item.getQuantity() 
					+ " WHERE item_id=" + item.getId()
					+ " AND player_id=" + player.getId();
			
			db.execSQL(sql);
		}
		
		db.close();
	}
	
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	//PLAYER
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	
	
	public Player addPlayer(String name)
	{
		SQLiteDatabase db = this.getWritableDatabase();
		
		ContentValues values = new ContentValues();
		values.put("name", name);
		values.put("coins", 100);
		
		db.insert("Player", null, values);
		
		ArrayList<Item> items = getItems();
		
		String sql2 = "SELECT player_id FROM Player;";
		int id = 1;
		
		db = this.getReadableDatabase();
		
		Cursor cursor = db.rawQuery(sql2, null);
		
		if (cursor.moveToLast())
		{
			id = cursor.getInt(0);
		}
		
		for (Item item : items)
		{
			String sql3 = "INSERT INTO Inventory VALUES (" 
					+ id + ", "
					+ item.getId() + ", "
					+ "0);";
			
			db.execSQL(sql3);
		}
		
		db.close();
		
		return new Player(id, name, 100, new Inventory(items));
	}
	
	public void removePlayer(int player_id)
	{
		SQLiteDatabase db = this.getWritableDatabase();
		
		String sql = "DELETE FROM Player "
				+ "WHERE player_id=" + player_id;
		
		db.execSQL(sql);
		
		db.close();
	}
	
	public Player getPlayer(int player_id)
	{
		SQLiteDatabase db = this.getReadableDatabase();
		
		String sql = "SELECT * FROM Player "
				+ "WHERE player_id=" + player_id;
		
		Cursor cursor = db.rawQuery(sql, null);
		
		Player player = null;
		
		if (cursor.moveToFirst())
		{
			String name = cursor.getString(1);
			int coins = cursor.getInt(2);
			
			Inventory inventory = getPlayerItems(getItems(), player_id);
			
			player = new Player(player_id, name, coins, inventory);
		}
		
		db.close();
		
		return player;
	}
	
	public void savePlayer(Player player)
	{
		saveItems(player);
		
		String sql = "UPDATE Player " +
				"SET coins = " + player.getCoins() + 
				" WHERE player_id=" + player.getId();
		
		SQLiteDatabase db = this.getWritableDatabase();
		
		db.execSQL(sql);
		
		db.close();
	}
	
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	//PLAYER
	//-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		
	
	public ArrayList<Profile> getProfiles(int player_id)
	{
		ArrayList<Profile> profiles = new ArrayList<Profile>();
		
		String sql = "SELECT * FROM Player";
		
		SQLiteDatabase db = this.getReadableDatabase();
		
		Cursor cursor = db.rawQuery(sql, null);
		
		if (cursor.moveToFirst())
		{
			do
			{
				boolean active = false;
				
				if (player_id == 0 || cursor.getInt(0) == player_id)
					active = true;
				
				Profile profile = new Profile(cursor.getString(1), cursor.getInt(0), active);
				
				profiles.add(profile);
			} while (cursor.moveToNext());
		}
		
		db.close();
		
		return profiles;
	}

	public int getHighScoresCount()
	{
		SQLiteDatabase db = this.getReadableDatabase();
		
		String sql = "SELECT * FROM HighScore";
		
		Cursor cursor = db.rawQuery(sql, null);
		
		int n = cursor.getCount();
		
		db.close();
		
		return n;
	}
	
	public HighScore getHighScore(int index)
	{
		SQLiteDatabase db = this.getReadableDatabase();
		
		String sql = "SELECT HighScore.player_id, Player.name, HighScore.kills, HighScore.height FROM HighScore, Player "
				+ "WHERE HighScore.player_id=Player.player_id"
				+ " ORDER BY height DESC, kills;";
		
		Cursor cursor = db.rawQuery(sql, null);
		
		cursor.moveToPosition(index);
		
		HighScore highScore = new HighScore(cursor.getInt(0), cursor.getString(1), cursor.getInt(2), cursor.getInt(3));
		
		db.close();
		
		return highScore;
	}
}
