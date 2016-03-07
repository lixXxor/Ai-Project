using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishSpawner : MonoBehaviour {
	
	public static int FasterSimSpeed = 10;
	
	public int numberOfFish = 30;
	public GameObject fishPrefab;
	public float newGenerationTime;
	public int bestAmountOfFood =0;
	public int bestGeneration =0;
	public float bestSpeed = 0;
	public float bestRotationSpeed =0;
	public float bestDistance = 0;

	public AudioClip newGenerationSound;
	
	public List<GameObject> schoolOfFish = new List<GameObject>();

	private GUIText guiText;
	private int numberOfGeneration;

	// Use this for initialization
	void Start () {
		numberOfGeneration = 0;
		guiText = (GUIText)GameObject.FindWithTag("GUI").GetComponent<GUIText>();
		guiText.text = "Number of generations: " + numberOfGeneration;
		InvokeRepeating("CreateNewGeneration", 0, newGenerationTime);
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (schoolOfFish.ToArray().Length);
		if(Input.GetKeyDown(KeyCode.F))
			simulateFaster();
		if(Input.GetKeyUp(KeyCode.F))
			simulateNormal();
	}
	
	private void simulateFaster(){
		print("F PRESSED DOWN");
		CancelInvoke();
		InvokeRepeating("CreateNewGeneration", 0, newGenerationTime/FasterSimSpeed);
	}
	
	private void simulateNormal(){
		print("F PRESSED UP");
		CancelInvoke();
		InvokeRepeating("CreateNewGeneration", 0, newGenerationTime);
	}

	private void CreateNewGeneration(){
		audio.clip = newGenerationSound;
		audio.Play ();

		numberOfGeneration++;
		Debug.Log ("Starting a new generation");

		guiText.text = "Number of generations: " + numberOfGeneration+"\n";
		guiText.text += "Current best amount of food: " + bestAmountOfFood+"\n";
		guiText.text += "Current best speed: " + bestSpeed+"\n";
		guiText.text += "Current best rotation speed: " + bestRotationSpeed+"\n";
		guiText.text += "Current best smelling distance: " + bestDistance+"\n";
		guiText.text += "Best generation yet: " + bestGeneration+"\n";

		GameObject[] fishArray = GameObject.FindGameObjectsWithTag("Fish");
		GameObject[] bestFishes = getBestFishes(fishArray);

		if(fishArray.Length > 0){
			if(bestFishes[0].GetComponent<FishScript>().getFish().getNumberOfEatenFood() > bestAmountOfFood){
				bestGeneration = numberOfGeneration-1;
				bestAmountOfFood = bestFishes[0].GetComponent<FishScript>().getFish().getNumberOfEatenFood();
				bestSpeed = bestFishes[0].GetComponent<FishScript>().getFish().getSpeed();
				bestRotationSpeed = bestFishes[0].GetComponent<FishScript>().getFish().getRotationSpeed();
				bestDistance = bestFishes[0].GetComponent<FishScript>().getFish().getSmellDistance();
				
				guiText.text = "Number of generations: " + numberOfGeneration+"\n";
				guiText.text += "Current best amount of food: " + bestAmountOfFood+"\n";
				guiText.text += "Current best speed: " + bestSpeed+"\n";
				guiText.text += "Current best rotation speed: " + bestRotationSpeed+"\n";
				guiText.text += "Current best smelling distance: " + bestDistance+"\n";
				guiText.text += "Best generation yet: " + bestGeneration+"\n";
		 	}

			//Remove all old fishes
			for(int i = 0; i < fishArray.Length; i++){
				Destroy(fishArray[i]);
			}
		}

		for (int i = 0; i < numberOfFish; i++) {
			Vector3 randomPos = RandomOnUnitCircle3(GameSettings.Instance.MapRadius);
			GameObject clone;
			if(bestFishes.Length > 0){
				if(fishPrefab){
					clone = (GameObject)Instantiate(fishPrefab, randomPos, Quaternion.Euler( 0 , Random.Range(0, 360) , 0));
					clone.tag = fishPrefab.tag;
					clone.name = fishPrefab.name;

					Fishy f = bestFishes[0].gameObject.GetComponent<FishScript>().getFish().makeChild();
					clone.GetComponent<FishScript>().setFish(f);
					clone.GetComponent<FishScript>().setSmellRangeIndicator(f.getSmellDistance());
					clone.GetComponent<FishScript>().setFishMaterial(f.getFishType());
				}
			}
			else{
				if(fishPrefab){
					clone = (GameObject)Instantiate(fishPrefab, randomPos, Quaternion.Euler( 0 , Random.Range(0, 360) , 0));
					clone.tag = fishPrefab.tag;
					clone.name = fishPrefab.name;
				}
			}
		}
	}

	private GameObject[] getBestFishes(GameObject[] go){

		GameObject[] bestFish = go;
		List<GameObject> listOfFish = new List<GameObject>(go);
		listOfFish.Sort(ByNumberOfFood);

		bestFish = listOfFish.ToArray();

		return bestFish;
	}

	//Sort function for lists
	int ByNumberOfFood ( GameObject a ,   GameObject b  ){
		int bn = b.GetComponent<FishScript>().getFish().getNumberOfEatenFood();
		int an = a.GetComponent<FishScript>().getFish().getNumberOfEatenFood();

		return bn.CompareTo(an);
	}

	public static Vector3 RandomOnUnitCircle3( float radius)
	{
		Vector3 randomPointOnCircle = Random.insideUnitSphere;
		
		randomPointOnCircle.y -= 1;
		
		randomPointOnCircle *= radius;
		if(Physics.Raycast(randomPointOnCircle, Vector3.forward, radius*2) && Physics.Raycast(randomPointOnCircle, Vector3.back, radius*2) 
		   && Physics.Raycast(randomPointOnCircle, Vector3.left, radius*2) && Physics.Raycast(randomPointOnCircle, Vector3.right, radius*2))
		{
			return randomPointOnCircle;
		}
		else{
			return randomPointOnCircle = RandomOnUnitCircle3(radius);
		}
	}
}


