using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSettings : MonoBehaviour {
	
	public int mapRadius;
	private static GameSettings instance;
	
	public GameSettings(){
		
	}
	
	public static GameSettings Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = GameObject.FindObjectOfType<GameSettings>();;
			}
			return instance;
		}
	}
	
	public int MapRadius{
		get
		{
			return mapRadius;
		}
		set{
			mapRadius = value;
		}
	}
	
}
