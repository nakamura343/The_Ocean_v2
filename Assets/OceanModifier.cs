﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class OceanModifier : MonoBehaviour {

	float eegReading;
	int touchingForehead = 0;
	public bool controllingWaves = true;

    //ui
    public GameObject levelBar;
    public GameObject sensorConnectionStatus;

	//normal
	float minOceanScale = 4.0f;
	float minWaveSpeed = 0.3f;
	float minVol = 0.05f;

	float defaultOceanScale = 5.3f;
	float defaultWaveSpeed = 0.7f;

	//max
	float maxWaveSpeed = 1.65f;
	float maxOceanScale = 12.5f;
	float maxVol = 1.0f;

	float incrementArg = 0.01f;
	float waveSpeedIncrementArg = 0.0001f;

	//current
	float oceanScale;//1-10
	float waveSpeed; //0-3

	public float vol = 1.0f;
	public Ocean myOcean;
	oscReceive oscReceiver;

	// Use this for initialization
	void Start () {
		oceanScale = minOceanScale;
		waveSpeed = minWaveSpeed;
		oscReceiver = GameObject.Find ("osc").GetComponent<oscReceive> ();
	}
	
	// Update is called once per frame
	void Update () {

		eegReading = oscReceiver.mellowValue;

        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(eegReading*100, 10);

		touchingForehead = oscReceiver.touchingForehead;



		if (touchingForehead == 1) {
			oceanScale = (CustomFunc.Map (eegReading, 1, 0, minOceanScale, maxOceanScale) - oceanScale) * incrementArg + oceanScale;
			waveSpeed = (CustomFunc.Map (eegReading, 1, 0, minWaveSpeed, maxWaveSpeed) - waveSpeed) * waveSpeedIncrementArg + waveSpeed;
			sensorConnectionStatus.GetComponent<Text>().text = "Sensor Wearing: YES";

		} else {
			oceanScale = (defaultOceanScale - oceanScale) * incrementArg + oceanScale;
			waveSpeed = (defaultWaveSpeed - waveSpeed) * waveSpeedIncrementArg + waveSpeed;
			sensorConnectionStatus.GetComponent<Text>().text = "Sensor Wearing: NO";

		}

		oceanScale = Mathf.Clamp (oceanScale, minOceanScale, maxOceanScale);
		waveSpeed = Mathf.Clamp (waveSpeed, minWaveSpeed, maxWaveSpeed);
		vol = CustomFunc.Map (oceanScale, minOceanScale, maxOceanScale, minVol, maxVol);
		vol = Mathf.Clamp (vol, minVol, maxVol);

		AudioListener.volume = vol;

		if(controllingWaves){
			myOcean.scale = oceanScale;
			myOcean.speed = waveSpeed;
		}


	}




}