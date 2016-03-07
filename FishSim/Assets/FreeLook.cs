using UnityEngine;
using System.Collections;

public class FreeLook : MonoBehaviour
{
	public AudioClip WaterUp;
	public AudioClip WaterDown;
	public AudioClip ambUnderWater;
	public AudioClip ambAboveWater;

	private AudioSource ambAudio;
	private AudioSource fxAudio;
	
	public float cameraSensitivity = 90;
	public float climbSpeed = 4;
	public float normalMoveSpeed = 10;
	public float slowMoveFactor = 0.25f;
	public float fastMoveFactor = 3;
	
	private float rotationX = 0.0f;
	private float rotationY = 0.0f;

	private float lastYPos;
	
	void Start ()
	{
		ambAudio = (AudioSource)gameObject.AddComponent ("AudioSource");
		ambAudio.volume = 0.2f;
		ambAudio.loop = true;
		ambAudio.dopplerLevel = 0;
		ambAudio.minDistance = 1;
		ambAudio.maxDistance = 500;
		ambAudio.clip = ambAboveWater;
		ambAudio.Play();

		fxAudio = (AudioSource)gameObject.AddComponent ("AudioSource");
		fxAudio.volume = 0.6f;
		fxAudio.loop = false;
		fxAudio.dopplerLevel = 0;
		fxAudio.minDistance = 1;
		fxAudio.maxDistance = 500;
		
		

		Screen.lockCursor = true;
	}
	
	void Update ()
	{
		rotationX += Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
		rotationY += Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp (rotationY, -90, 90);
		
		transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(rotationY, Vector3.left);
		
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
		{
			transform.position += transform.forward * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			transform.position += transform.right * (normalMoveSpeed * fastMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
		{
			transform.position += transform.forward * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Vertical") * Time.deltaTime;
			transform.position += transform.right * (normalMoveSpeed * slowMoveFactor) * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		else
		{
			transform.position += transform.forward * normalMoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
			transform.position += transform.right * normalMoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
		}
		
		
		if (Input.GetKey (KeyCode.Q)) {transform.position += transform.up * climbSpeed * Time.deltaTime;}
		if (Input.GetKey (KeyCode.E)) {transform.position -= transform.up * climbSpeed * Time.deltaTime;}
		
		if (Input.GetKeyDown (KeyCode.End))
		{
			Screen.lockCursor = (Screen.lockCursor == false) ? true : false;
		}

		if (transform.position.y <= 0.0f && lastYPos > 0) {
			ambAudio.clip = ambUnderWater;
			ambAudio.Play();

			fxAudio.PlayOneShot(WaterDown);
		}
		else if(transform.position.y >= 0.0f && lastYPos < 0){

			ambAudio.clip = ambAboveWater;
			ambAudio.Play();

			fxAudio.PlayOneShot(WaterUp);
		}

		lastYPos = transform.position.y;
	}
}