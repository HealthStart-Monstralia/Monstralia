using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {

	public GameObject countdown1;
	public GameObject countdown2;
	public GameObject countdown3;
	public GameObject countdownGo;

	public IEnumerator RunCountdown() {

		GameObject count3 = (GameObject)Instantiate(countdown3);
		count3.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		Destroy(count3);
		
		GameObject count2 = (GameObject)Instantiate(countdown2);
		count2.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		Destroy(count2);
		
		GameObject count1 = (GameObject)Instantiate(countdown1);
		count1.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		Destroy(count1);
		
		GameObject countGo = (GameObject)Instantiate(countdownGo);
		countGo.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		Destroy(countGo);
	}

}
