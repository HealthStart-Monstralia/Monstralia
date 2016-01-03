using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {


	public IEnumerator RunCountdown () {

		GameObject countdown3 = (GameObject)Instantiate(Resources.Load("Countdown3")); 
		countdown3.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		countdown3.SetActive (false);
		
		GameObject countdown2 = (GameObject)Instantiate(Resources.Load("Countdown2"));
		countdown2.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		countdown2.SetActive (false);
		
		GameObject countdown1 = (GameObject)Instantiate(Resources.Load("Countdown1"));
		countdown1.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		countdown1.SetActive (false);
		
		GameObject countdownGo = (GameObject)Instantiate(Resources.Load("CountdownGo"));
		countdownGo.SetActive (true);
		yield return new WaitForSeconds (1.0f);
		countdownGo.SetActive (false);
	}

}
