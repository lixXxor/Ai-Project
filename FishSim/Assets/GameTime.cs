using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour {

	public enum TimeOfDay{
		Idle,
		SunRise,
		SunSet,
	}

	public int numberOfPredatorsAtNight = 1;

	public GameObject predatorPrefab;
	private bool predatorsSpawned = false;
	
	private Sun[] _sunScript;
	public Transform[] sun;
	private float _degreeRotation;
	private float _timeOfDay;
	
	public float sunRise;
	public float sunSet;
	public float SkyBoxBlendModifier;

	public Color ambLightMax;
	public Color ambLightMin;

	public float dayCycleInMinutes = 1;

	private float _dayCycleInSeconds;

	private const float SECOND = 1;
	private const float MINUTE = 60*SECOND;
	private const float HOUR = 60*MINUTE;
	private const float DAY = 24*HOUR;
	private const float DEGREES_PER_SECOND = 360 / DAY;

	private TimeOfDay _tod;
	private float _noonTime;
	private float _morningLength;
	private float _eveningLength;
	

	// Use this for initialization
	void Start () {

		_tod = TimeOfDay.Idle;

		_dayCycleInSeconds = dayCycleInMinutes * MINUTE;

		RenderSettings.skybox.SetFloat ("_Blend", 0);

		_sunScript = new Sun[sun.Length];

		for (int cnt = 0; cnt < sun.Length; cnt++) {
			Sun temp = sun[cnt].GetComponent<Sun>();
			if(temp == null){
				Debug.LogWarning("Sun script not found");
				sun[cnt].gameObject.AddComponent<Sun>();
				temp = sun[cnt].GetComponent<Sun>();
			}
			_sunScript[cnt] = temp;
		}

		_timeOfDay = 0;
		_degreeRotation = DEGREES_PER_SECOND * DAY / (_dayCycleInSeconds);

		sunRise *= _dayCycleInSeconds;
		sunSet *= _dayCycleInSeconds;
		_noonTime = _dayCycleInSeconds / 2;

		_morningLength = _noonTime - sunRise;			//The lenght of a morning in seconds
		_eveningLength = sunSet - _noonTime;			//The length of the evening in seconds

		SetupLightning();
	}
	
	// Update is called once per frame
	void Update () {

		_timeOfDay += Time.deltaTime;

		if (_timeOfDay > _dayCycleInSeconds)
			_timeOfDay -= _dayCycleInSeconds;

		for (int cnt = 0; cnt < sun.Length; cnt++) {
			sun [cnt].Rotate (new Vector3(_degreeRotation, 0, 0)*Time.deltaTime);	
		}

		if (_timeOfDay > sunRise && _timeOfDay < _noonTime) {
			AdjustLightning(true);
		} 
		else if(_timeOfDay > _noonTime && _timeOfDay < sunSet){
			AdjustLightning(false);
		}

		if (_timeOfDay > sunRise && _timeOfDay < sunSet && RenderSettings.skybox.GetFloat("_Blend") < 1) {
			_tod = GameTime.TimeOfDay.SunRise;
			BlendSkybox ();
		}
		else if(_timeOfDay > sunSet && RenderSettings.skybox.GetFloat("_Blend")> 0 ){
			_tod = GameTime.TimeOfDay.SunSet;
			BlendSkybox ();
		}
		else{
			_tod = GameTime.TimeOfDay.Idle;
		}
	}

	private void BlendSkybox(){
		float temp = 0;

		switch (_tod) {
		case TimeOfDay.SunRise:
			temp = (_timeOfDay - sunRise) / _dayCycleInSeconds * SkyBoxBlendModifier;
			break;
		case TimeOfDay.SunSet:
			temp = (_timeOfDay - sunSet) / _dayCycleInSeconds * SkyBoxBlendModifier;
			temp = 1 - temp;
			break;
		}
		//Debug.Log(temp);
		RenderSettings.skybox.SetFloat ("_Blend", temp);
	}

	private void SetupLightning(){
		RenderSettings.ambientLight = ambLightMin;

		for (int i = 0; i < _sunScript.Length; i++) {
			if(_sunScript[i].givesLight){
				sun[i].GetComponent<Light>().intensity = _sunScript[i]._minLightBrightness;	
			}
		}
	}

	private void AdjustLightning(bool brighten){
		float pos = 0;

		if (brighten){
			if(predatorsSpawned){
				removePredators();
				predatorsSpawned = false;
			}
			pos = (_timeOfDay - sunRise)/_morningLength;				//Get the position of the sun in the morning sky		
		}
		else{
			if(!predatorsSpawned){
				spawnPredators();
				predatorsSpawned = true;
			}
			pos = (sunSet - _timeOfDay)/_eveningLength;					//Get the position of the sun in the night sky
		}

		RenderSettings.ambientLight = new Color(ambLightMin.r + ambLightMax.r * pos,
		                                        ambLightMin.g + ambLightMax.g * pos,
		                                        ambLightMin.b + ambLightMax.b * pos);

		for (int i = 0; i < _sunScript.Length; i++) {
			if(_sunScript[i].givesLight){
				_sunScript[i].GetComponent<Light>().intensity = _sunScript[i]._maxLightBrightness*pos;	
			}
		}	
	}	

	public void spawnPredators(){

		for (int i = 0; i < numberOfPredatorsAtNight; i++) {
			Vector2 randomPos = RandomOnUnitCircle3(GameSettings.Instance.MapRadius);
			GameObject clone = (GameObject)Instantiate(predatorPrefab, randomPos, Quaternion.Euler( 0 , Random.Range(0, 360) , 0));
			clone.tag = predatorPrefab.tag;
			clone.name = predatorPrefab.name;	
		}
	}
	
	public void removePredators(){
		GameObject[] predatorArray = GameObject.FindGameObjectsWithTag("Predator");
		
		for(int i = 0; i < predatorArray.Length; i++){
			Destroy(predatorArray[i]);
		}
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
