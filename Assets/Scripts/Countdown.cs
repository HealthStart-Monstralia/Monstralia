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

	Animator countdown3Anim;
	Animator countdown2Anim;
	Animator countdown1Anim;
	Animator countdownGoAnim;

	void Awake() {
		countdown3Anim = countdown3.GetComponent<Animator> ();
		countdown2Anim = countdown2.GetComponent<Animator> ();
		countdown1Anim = countdown1.GetComponent<Animator> ();
		countdownGoAnim = countdownGo.GetComponent<Animator> ();
		StartCoroutine (RunCountdown ());
	}

	public IEnumerator RunCountdown() {
		//if (!SoundManager.GetInstance ())
			//Instantiate (SoundManager);
		SoundManager.GetInstance().StopPlayingVoiceOver();
		SoundManager.GetInstance().PlayVoiceOverClip(countdownClip);
		yield return new WaitForSeconds (1.0f);

		countdown3.SetActive (true);
		countdown3Anim.Play ("CountdownGoAnimation");
		yield return new WaitForSeconds (1.0f);
		countdown3.SetActive (false);

		countdown2.SetActive (true);
		countdown2Anim.Play ("CountdownGoAnimation");
		//SoundManager.GetInstance().PlayVoiceOverClip(count2Clip);
		yield return new WaitForSeconds (1.0f);
		countdown2.SetActive (false);

		countdown1.SetActive (true);
		countdown1Anim.Play ("CountdownGoAnimation");
		//SoundManager.GetInstance().PlayVoiceOverClip(count1Clip);
		yield return new WaitForSeconds (1.0f);
		countdown1.SetActive (false);

		countdownGo.SetActive (true);
		countdownGoAnim.Play ("CountdownGoAnimation");
		//SoundManager.GetInstance().PlayVoiceOverClip(countGoClip);
		yield return new WaitForSeconds (1.0f);
		countdownGo.SetActive (false);
		Destroy (gameObject);
	}

}
