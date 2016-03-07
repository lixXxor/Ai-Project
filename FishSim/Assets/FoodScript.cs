using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodScript : MonoBehaviour {	
	private Foody food;
	private bool inWater;
	// Use this for initialization
	void Start () {
		food = new Foody(new Vector2(transform.position.x,transform.position.z));
		if(transform.position.y >0.0f)
			inWater = false;
	}
	
	public Foody getFood(){
		return food;
	}
	
	public void setFood(Foody f){
		food = f;
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.y < 0.0f) {
			
			
			if(inWater == false){
				Physics.gravity = new Vector3(0.0f, -0.1f, 0.0f);
				inWater = true;
			}
		}
	}
}