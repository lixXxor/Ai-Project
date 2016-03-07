using UnityEngine;
using System.Collections;
public class GuiButtons : MonoBehaviour
{
	public GameObject predatorPrefab;

	void onStart(){

	}

	void OnGUI(){
		// Make a background box
		GUI.Box(new Rect(0,Screen.height-200,200,100), "Fishy Menu");
		
		// Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
		if(GUI.Button(new Rect(20,Screen.height-160,150,20), "Spawn Predator")) {

			Vector2 randomPos = RandomOnUnitCircle2(GameSettings.Instance.MapRadius);
			GameObject clone = (GameObject)Instantiate(predatorPrefab, new Vector3(randomPos.x,0.0f, randomPos.y ), Quaternion.Euler( 0 , Random.Range(0, 360) , 0));
			clone.tag = predatorPrefab.tag;
			clone.name = predatorPrefab.name;
		}
	}
	
	public static Vector2 RandomOnUnitCircle2( float radius)
	{
		Vector2 randomPointOnCircle = Random.insideUnitCircle;
		randomPointOnCircle *= radius;
		return randomPointOnCircle;
	}
}

