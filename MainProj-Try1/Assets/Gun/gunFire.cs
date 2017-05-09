using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunFire : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1")){
			AudioSource gunSound = GetComponent<AudioSource>();
			gunSound.Play();
			GetComponent<Animation> ().Play ("GunShot");
	}
}
}
