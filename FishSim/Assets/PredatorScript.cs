using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PredatorScript : MonoBehaviour {

	public float SmellDistance;
	public AudioClip SpawnSound;

	private Predator _predator;

	// Use this for initialization
	void Start () {

		audio.clip = SpawnSound;
		audio.Play ();
		gameObject.tag = "Predator";


		if(_predator == null){
			_predator = new Predator(Random.Range(8,15), SmellDistance, 3);
		}
	}	
	// Update is called once per frame
	void Update () {
		GameObject closestFish = getClosestFish();
		
		float distance = Mathf.Infinity;
		
		if(closestFish){
			distance = Vector3.Distance( closestFish.transform.position, transform.position);
		}
		if(Input.GetKey(KeyCode.F)){
			if ( distance < _predator.getSmellDistance() ){	
				Quaternion rotation = Quaternion.LookRotation(closestFish.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _predator.getRotationSpeed()*FishSpawner.FasterSimSpeed);
				transform.position += transform.forward * _predator.getSpeed() * Time.deltaTime*FishSpawner.FasterSimSpeed;
			}
			else
			{
				transform.position += transform.forward * _predator.getSpeed() * Time.deltaTime*FishSpawner.FasterSimSpeed;
			} 
		}
		else{
			if ( distance < _predator.getSmellDistance() ){	
				Quaternion rotation = Quaternion.LookRotation(closestFish.transform.position - transform.position);
				transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * _predator.getRotationSpeed());
				transform.position += transform.forward * _predator.getSpeed() * Time.deltaTime;
			}
			else
			{
				transform.position += transform.forward * _predator.getSpeed() * Time.deltaTime;
			} 
		}
	}
	
	void  OnCollisionEnter ( Collision collision  ){
		
		//PREDATOR EATS FISHY
		if(collision.gameObject.tag == "Fish"){
			Destroy(collision.gameObject);
		}
		
		if(collision.gameObject.tag == "Food"){
			Destroy(collision.gameObject);
		}
	}
	
	void  OnTriggerExit ( Collider collider  ){
		
		//Vad händer när en fisk åker utanför kartan?
		//Jo den kommer till motsatt sida av "sjön" (DETTA ÄR DOCK FEL MATEMATIK)
		if(collider.gameObject.tag == "MapBounds"){
			gameObject.transform.forward = new Vector3(-gameObject.transform.forward.x,0,-gameObject.transform.forward.z);
			
		}
	}
	
	
	GameObject getClosestFish (){		
		Collider[] fishInsideRange = Physics.OverlapSphere(gameObject.transform.position, _predator.getSmellDistance());
		if(fishInsideRange.Length > 0){
			List<GameObject> myList = new List<GameObject>();
			//Hämta lista med mat
			for(int i = 0; i<fishInsideRange.Length;i++){
				if(fishInsideRange[i].gameObject.tag == "Fish")
					myList.Add(fishInsideRange[i].gameObject);
			}
			
			
			// Sortera map avstånd från predator till fisk
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
