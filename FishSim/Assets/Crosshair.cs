﻿using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairTexture;
	public float crosshairScale = 1;

	void Start(){

	}

	void OnGUI(){

		//if not paused
		if(Time.timeScale != 0)
		{
			if(crosshairTexture!=null)
				GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width) / 2,
				                         (Screen.height - crosshairTexture.height) /2, crosshairTexture.width*crosshairScale, crosshairTexture.height*crosshairScale),crosshairTexture);
			else
				Debug.Log("No crosshair texture set in the Inspector");
		}
	}
}
