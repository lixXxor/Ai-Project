using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class FoodMediator
{

//DELA UPP KARTAN I GRID
//OCH LÄGG TILL MATEN OCH FISKARNA I RESPEKTIVE GRID
	private List<Foody> Foodlist;
	private List<Fishy> Fishlist;
	private List<Vector2> Gridlist;
	private static FoodMediator instance;
	private int numberOfGridPoints = 20;
	
	private FoodMediator(){
		Foodlist = new List<Foody>();
		Fishlist = new List<Fishy>();
		Gridlist = new List<Vector2>();
		setGridlist(100);
	}
	
	public static FoodMediator getInstance
	{
		get 
		{
			if (instance == null)
			{
				instance = new FoodMediator();
			}
			return instance;
		}
	}
	
	public void setGridlist(int mapRadius){
	
		for(int gridpoints = 0;gridpoints<numberOfGridPoints; gridpoints++)
		{
			Gridlist.Add(new Vector2((mapRadius/2)*Mathf.Cos(gridpoints*(Mathf.PI/numberOfGridPoints)),(mapRadius/2)*Mathf.Sin(gridpoints*(Mathf.PI/numberOfGridPoints))));
		}
	}
	
	public Foody getClosestFood(Fishy fish){
		return Foodlist[0];
	}
	
	public void addFishy(Fishy fish)
	{
		Fishlist.Add(fish);
	}
	
	public void addFoody(Foody food)
	{
		Foodlist.Add(food);
		Foodlist.Sort();
	}
	
	public void emptyFishes(){
		Fishlist.Clear();
	}
	
	public void deleteFood(Foody food){
		Foodlist.Remove(food);
	}
	
	public List<Fishy> getFishlist()
	{
		return Fishlist;
	}
	public List<Foody> getFoodlist()
	{
		return Foodlist;
	}
}
