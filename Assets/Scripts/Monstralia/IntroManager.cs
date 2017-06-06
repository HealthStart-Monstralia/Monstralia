using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour {
	public GameObject blockSlide;
	public GameObject[] aboutMonstraliaSlides;
	private static IntroManager instance = null;

	private int slideNum = 0;

	void Awake() {
		if(instance == null) {
			instance = this;
		}
		else if(instance != this) {
			Destroy(gameObject);
		}

		for (int i = 0; i < aboutMonstraliaSlides.Length; i++) {
			if (aboutMonstraliaSlides [i].activeSelf)
				aboutMonstraliaSlides [i].SetActive (false);
		}
		blockSlide.SetActive (true);
		aboutMonstraliaSlides [0].SetActive (true);
	}
}
