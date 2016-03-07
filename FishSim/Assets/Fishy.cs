using UnityEngine;
using System.Collections;

public class Fishy
{
	private int numberOfEatenFood =0;
	private float speed;
	private float smellDistance;
	private float rotationSpeed;
	private int mutationRisk;
	private string fishType; 
	//Constructor

	public Fishy (float s, float sDist, float rotSpd, int mutRisk)
	{
		speed = s;
		smellDistance = sDist;
		rotationSpeed = rotSpd;
		mutationRisk = mutRisk;
		setFishType (); 
	}

	//Member functions
	public Fishy makeChild(){
		int Mutation = Random.Range (1, 100);
		if (Mutation >= mutationRisk) {
			Fishy fish = new Fishy (speed + Random.Range (-1.0f, 1.0f), smellDistance + Random.Range (-1.0f, 1.0f), rotationSpeed + Random.Range (-0.5f, 0.5f), mutationRisk);
			fish.fishType = fishType;
			return fish;
		} 
		else {
			return new Fishy (Random.Range (2.0f, 10.0f), 10.0f, Random.Range (0.3f, 2.0f), mutationRisk);
		}
	}

	public Fishy makeSickFish(){
		return new Fishy (speed*0.9f, smellDistance*0.9f, rotationSpeed*0.9f, mutationRisk);
	}

	//Getters
	public int getNumberOfEatenFood(){
		return numberOfEatenFood;
	}
	
	public float getSpeed(){
		return speed;
	}

	public float getSmellDistance(){
		return smellDistance;
	}

	public float getRotationSpeed(){
		return rotationSpeed;
	}

	public string getFishType(){
		return fishType;
	}

	//Setters
	public void setFishType(){
		float t = Random.Range (0, 100);
		if (t < 50){
			fishType = "Harald";
			speed*=1.5f;
			rotationSpeed*=0.5f;
		}
		else {
			fishType = "UsainBolt";
			speed*=0.5f;
			rotationSpeed*=1.5f;
		}
	}

	public void increaseNumberOfEatenFood(int nFood){
		numberOfEatenFood += nFood;
	}
	
	public void setSpeed(float s){
		speed = s;
	}
	
	public void setSmellDistance(float sDist){
		smellDistance = sDist;
	}
	
	public void setRotationSpeed(float rotSpd){
		rotationSpeed = rotSpd;
	}
}
