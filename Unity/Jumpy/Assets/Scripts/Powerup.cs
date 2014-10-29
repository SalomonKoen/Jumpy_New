using UnityEngine;
using System.Collections;

public class Powerup
{
	private int quantity;
	private int type;
	private double value;
	
	public Powerup(int quantity, int type, double value)
	{
		this.quantity = quantity;
		this.value = value;
		this.type = type;
	}

	public int getQuantity()
	{
		return quantity;
	}

	public int getType()
	{
		return type;
	}

	public double getValue()
	{
		return value;
	}
}
