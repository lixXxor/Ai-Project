using UnityEngine;
using System.Collections;

public class PlaceFood : MonoBehaviour {

	public Camera cam;
	public float throwForce = 800.0f;
	public GameObject GoodFoodPrefab;
	public GameObject BadFoodPrefab;

	public AudioClip placeSound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		//Kasta iväg objektet om nusknapp 0 trycks ner
		if(Input.GetMouseButtonDown(0)){
			GameObject clone = (GameObject)Instantiate(GoodFoodPrefab, cam.transform.position + cam.transform.forward*3, Quaternion.Euler( 0 , 0 , 0));

			clone.tag = GoodFoodPrefab.tag;
			clone.name = GoodFoodPrefab.name;

			clone.rigidbody.AddForce(cam.transform.forward*throwForce);	
		}

		//Kasta iväg objektet om nusknapp 0 trycks ner
		if(Input.GetMouseButtonDown(1)){
			GameObject clone = (GameObject)Instantiate(BadFoodPrefab, cam.transform.position + cam.transform.forward*3, Quaternion.Euler( 0 , 0 , 0));
			
			clone.tag = BadFoodPrefab.tag;
			clone.name = BadFoodPrefab.name;
			
			clone.rigidbody.AddForce(cam.transform.forward*throwForce);	
		}
	}
}
