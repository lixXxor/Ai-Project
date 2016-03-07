using UnityEngine;
using System.Collections;

public class Foody
{
	private Vector2 position;
	
	public Foody (Vector2 pos)
	{
		position = pos;
	}
	
	public Vector2 getPosition(){
		return position;
	}
}