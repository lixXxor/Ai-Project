using UnityEngine;
using System.Collections;

public class FoodSpawner : MonoBehaviour {

	public int numberOfFood = 30;
	public GameObject foodPrefab; 
	public int badFoodChance = 0;
	public float newFoodTime;

	// Use this for initialization
	void Start () {
			InvokeRepeating("CreateNewFood", 0, newFoodTime);
			
		for (int i = 0;i < numberOfFood; i++) {
			Vector2 randomPos = RandomOnUnitCircle2(GameSettings.Instance.MapRadius);

			GameObject clone = (GameObject)Instantiate(foodPrefab, new Vector3(randomPos.x,0.0f, randomPos.y ), Quaternion.Euler( 0 , Random.Range(0, 360) , 0));
			clone.tag = foodPrefab.tag;
			clone.name = foodPrefab.name;
			//if(	Random.Range(0.0f,100.0f) < badFoodChance ){
			//	clone.renderer.material.color = new Color(0.0f,0,0f,1.0f);
			//}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.F))
			simulateFaster();
		if(Input.GetKeyUp(KeyCode.F))
			simulateNormal();
	}
	
	private void simulateFaster(){
		print("F PRESSED DOWN");
		CancelInvoke();
		InvokeRepeating("CreateNewFood", 0, newFoodTime/FishSpawner.FasterSimSpeed);
	}
	
	private void simulateNormal(){
		print("F PRESSED UP");
		CancelInvoke();
		InvokeRepeating("CreateNewFood", 0, newFoodTime);
	}
	
	private void CreateNewFood(){
		Vector2 randomPos = RandomOnUnitCircle2(GameSettings.Instance.MapRadius);
		GameObject clone = (GameObject)Instantiate(foodPrefab, new Vector3(randomPos.x,0.0f, randomPos.y ), Quaternion.Euler( 0 , Random.Range(0, 360) , 0));
		clone.tag = foodPrefab.tag;
		clone.name = foodPrefab.name;
	}

	public static Vector2 RandomOnUnitCircle2( float radius)
	{
		Vector2 randomPointOnCircle = Random.insideUnitCircle;
		randomPointOnCircle *= radius;
		return randomPointOnCircle;
	}
}
