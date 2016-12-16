using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {

	public AudioClip countdownClip;
	public GameObject countdown1;
	public AudioClip count1Clip;
	public GameObject countdown2;
	public AudioClip count2Clip;
	public GameObject countdown3;
	public AudioClip count3Clip;
	public GameObject countdownGo;
	public AudioClip countGoClip;

	public IEnumerator RunCountdown() {
		SoundManager.GetInstance().PlayVoiceOverClip(countdownClip);
		yield return new WaitForSeconds (1.0f);

		GameObject count3 = (GameObject)Instantiate(countdown3);
		count3.SetActive (true);
		//SoundManager.GetInstance().PlayVoiceOverClip(count3Clip);
		yield return new WaitForSeconds (1.0f);
		Destroy(count3);
		
		GameObject count2 = (GameObject)Instantiate(countdown2);
		count2.SetActive (true);
		//SoundManager.GetInstance().PlayVoiceOverClip(count2Clip);
		yield return new WaitForSeconds (1.0f);
		Destroy(count2);
		
		GameObject count1 = (GameObject)Instantiate(countdown1);
		count1.SetActive (true);
		//SoundManager.GetInstance().PlayVoiceOverClip(count1Clip);
		yield return new WaitForSeconds (1.0f);
		Destroy(count1);
		
		GameObject countGo = (GameObject)Instantiate(countdownGo);
		countGo.SetActive (true);
		//SoundManager.GetInstance().PlayVoiceOverClip(countGoClip);
		yield return new WaitForSeconds (1.0f);
		Destroy(countGo);
	}

}
