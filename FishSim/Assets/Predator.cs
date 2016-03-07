using UnityEngine;
using System.Collections;

public class Predator
{
	private float speed;
	private float smellDistance;
	private float rotationSpeed;
	//Constructor
	
	public Predator (float s, float sDist, float rotSpd)
	{
		speed = s;
		smellDistance = sDist;
		rotationSpeed = rotSpd;
	}
	//Getters
	
	public float getSpeed(){
		return speed;
	}
	
	public float getSmellDistance(){
		return smellDistance;
	}
	
	public float getRotationSpeed(){
		return rotationSpeed;
	}
	
	//Setters

	
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
