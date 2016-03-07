using UnityEngine;
using System.Collections;

public class underwaterEffect : MonoBehaviour {

	public float waterLevel;
	public ParticleSystem myParticles;

	private bool  isUnderwater;
	private Color normalColor;
	private Color underwaterColor;
	
	void  Start (){
		normalColor = new Color (0.5f, 0.5f, 0.5f, 0.5f);
		underwaterColor = new Color (0.22f, 0.65f, 0.77f, 0.5f);

		RenderSettings.fog = true;
		RenderSettings.fogColor = normalColor;
		RenderSettings.fogDensity = 0.004f;
		//myParticles.Stop();
	}
	
	void  Update (){
		if ((transform.position.y < waterLevel) != isUnderwater) 
		{
			isUnderwater = transform.position.y < waterLevel;
			if (isUnderwater) SetUnderwater ();
			if (!isUnderwater) SetNormal ();
		}
	}
	
	void  SetNormal (){
		RenderSettings.fogColor = normalColor;
		RenderSettings.fogDensity = 0.004f;
		//myParticles.Stop();
	}
	
	void  SetUnderwater (){

		RenderSettings.fogColor = underwaterColor;
		RenderSettings.fogDensity = 0.005f;
		//myParticles.Play();
	}
}