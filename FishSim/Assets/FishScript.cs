using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishScript : MonoBehaviour {

	TextMesh[] myText;


	//FishStats
	private Fishy fish;
	public float minSpeed;
	public float maxSpeed;
	public float minRotationSpeed;
	public float maxRotationSpeed;
	public float minSmellDistance;
	public float maxSmellDistance;
	public int mutationRisk;
	public Material FishMaterial_1;	
	public Material FishMaterial_2;


	// Use this for initialization
	void Start () {
		gameObject.tag = "Fish";

		if(fish == null){
			float s = Random.Range(minSpeed, maxSpeed);
			float sd = Random.Range (minSmellDistance, maxSmellDistance);
			float rs = Random.Range (minSmellDistance, maxSmellDistance);
			fish = new Fishy(s, sd, rs, mutationRisk);
			setSmellRangeIndicator(sd);
			setFishMaterial(fish.getFishType());
		}

		myText = GetComponentsInChildren<TextMesh>();
		myText[0].text = "s: " + fish.getSpeed();
		myText[1].text = "n: " + fish.getNumberOfEatenFood(); 
	}

	public Fishy getFish(){
		return fish;
	}
	
	public void setFish(Fishy f){
		fish = f;
	}

	public void setFishMaterial(string m){
		GameObject fishBody = transform.GetChild(0).GetChild(1).GetChild(2).gameObject;

		if(m == "Harald")
			fishBody.renderer.material = FishMaterial_1;
		else
			fishBody.renderer.material = FishMaterial_2;

	}

	public void setSmellRangeIndicator(float radius){
		int children = transform.childCount;
		for (int i = 0; i < children; ++i){
			if(transform.GetChild(i).tag == "SmellRange"){
				GameObject sr = transform.GetChild(i).gameObject;
				Color c = new Color((1-(radius-minSmellDistance)/(maxSmellDistance-minSmellDistance)),((radius-minSmellDistance)/(maxSmellDistance-minSmellDistance)) , 0.0f, 0.2f);

				sr.transform.localScale = new Vector3( radius*2, radius*2, radius*2 );
				sr.renderer.material.color = c;
			}
		}	
	}
	
	// Update is called once per frame
	void Update () {

		GameObject closestFood = getClosestFood();
		
		float distance = Mathf.Infinity;
		RaycastHit rayHit;
		//Only hit stuff in layer 0
		int layerMask = 1 << 0;		
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, 10, layerMask)){
			if(rayHit.transform.gameObject.tag == "Terrain"){
			Vector3 normal = rayHit.normal;
			Vector3 inVec = transform.forward;
			Vector3 outVec = Vector3.Reflect(inVec, normal);
			Vector3 newDir = Vector3.RotateTowards(transform.forward,outVec,Time.deltaTime * fish.getRotationSpeed(), 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
			}
		}
		layerMask = 1 << 4;
		if(Physics.Raycast(transform.position, transform.forward, out rayHit, 10, layerMask)){
			Vector3 normal = rayHit.normal;
			Vector3 inVec = transform.forward;
			Vector3 outVec = Vector3.Reflect(inVec, normal);
			Vector3 newDir = Vector3.RotateTowards(transform.forward,outVec,Time.deltaTime * fish.getRotationSpeed(), 0.0f);
			transform.rotation = Quaternion.LookRotation(newDir);
		}
		if(closestFood){
			distance = Vector3.Distance( closestFood.transform.position, transform.position);
		}
		if(Input.GetKey(KeyCode.F)){
			//Om Fisken känner lukt av mat (om maten är tillräckligt nära), simma mot den. 
			//Implementera fuzzy logic här kanske....
			if ( distance < fish.getSmellDistance() ){	
				Quaternion rotation = Quaternion.LookRotation(closestFood.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * fish.getRotationSpeed()*FishSpawner.FasterSimSpeed);
				transform.position += transform.forward * fish.getSpeed() * Time.deltaTime*FishSpawner.FasterSimSpeed;
			}
			else
			{
				transform.position += transform.forward * fish.getSpeed() * Time.deltaTime*FishSpawner.FasterSimSpeed;
			} 
		}
		else{
			//Om Fisken känner lukt av mat (om maten är tillräckligt nära), simma mot den. 
			//Implementera fuzzy logic här kanske....
			if ( distance < fish.getSmellDistance() ){	
				Quaternion rotation = Quaternion.LookRotation(closestFood.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * fish.getRotationSpeed());
				transform.position += transform.forward * fish.getSpeed() * Time.deltaTime;
			}
			else
			{
				transform.position += transform.forward * fish.getSpeed() * Time.deltaTime;
			} 
		}
	}

	void  OnCollisionEnter ( Collision collision  ){
		//Fisken åker in i en mat
		if(collision.gameObject.tag == "Food"){
			fish.increaseNumberOfEatenFood(1);
			myText[1].text = "n: " + fish.getNumberOfEatenFood();
			Destroy(collision.gameObject);
		}

		//Fisken åker in i en dålig mat
		if(collision.gameObject.tag == "BadFood"){
			fish.increaseNumberOfEatenFood(-1);
			myText[1].text = "n: " + fish.getNumberOfEatenFood();
			Destroy(collision.gameObject);

			Fishy f = gameObject.GetComponent<FishScript>().getFish().makeSickFish();
			gameObject.GetComponent<FishScript>().setFish(f);
			gameObject.GetComponent<FishScript>().setSmellRangeIndicator(f.getSmellDistance());

		}
	}

	void  OnTriggerExit ( Collider collider  ){

		//Vad händer när en fisk åker utanför kartan?
		//Jo den kommer till motsatt sida av "sjön" (DETTA ÄR DOCK FEL MATEMATIK)
		if(collider.gameObject.tag == "MapBounds"){
			gameObject.transform.forward = new Vector3(-gameObject.transform.forward.x,0,-gameObject.transform.forward.z);

		}
	}
	
	GameObject getClosestFood (){
		
		Collider[] foodInsideRange = Physics.OverlapSphere(transform.position, fish.getSmellDistance());

		if(foodInsideRange.Length > 0){
			List<GameObject> myList = new List<GameObject>();
			//Hämta lista med mat
			for(int i = 0; i<foodInsideRange.Length;i++){
				if(foodInsideRange[i].gameObject.tag == "Food" || foodInsideRange[i].gameObject.tag == "BadFood")
					if(foodInsideRange[i].gameObject.transform.position.y <= 0.0f) 
						myList.Add(foodInsideRange[i].gameObject);
			}


			// Sortera map avstånd från fisk till mat
			myList.Sort(ByDistance);
			if(myList.ToArray().Length > 0)
				return myList[0];
		}
		return null;
	}

	//Sort function for lists
	int ByDistance ( GameObject a ,   GameObject b  ){
		float dstToA= Vector3.Distance(transform.position, a.transform.position);
		float dstToB= Vector3.Distance(transform.position, b.transform.position);
		return dstToA.CompareTo(dstToB);
	}

	public static Vector2 RandomOnUnitCircle2( float radius)
	{
		Vector2 randomPointOnCircle = Random.insideUnitCircle;
		randomPointOnCircle *= radius;
		return randomPointOnCircle;
	}


}
