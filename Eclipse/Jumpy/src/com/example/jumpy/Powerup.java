package com.example.jumpy;

public class Powerup extends Item
{
	private int type;
	private double value;
	
	public Powerup(int id, String name, String description, boolean multiple, int price, int image, int quantity, int type, double value)
	{
		super(id, name, description, multiple, price, image, quantity);
		this.value = value;
		this.type = type;
	}
	
	public int getType()
	{
		return type;
	}
	
	public double getValue()
	{
		return value;
	}
	
	@Override
	public int getQuantity()
	{
		return this.quantity;
	}
}
