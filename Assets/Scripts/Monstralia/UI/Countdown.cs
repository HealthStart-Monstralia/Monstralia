using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour {

	public AudioClip countdownClip;
	public GameObject countdown1, countdown2, countdown3, countdownGo;

    Animator countdown3Anim, countdown2Anim, countdown1Anim, countdownGoAnim;

	void Awake() {
		countdown3Anim = countdown3.GetComponent<Animator> ();
		countdown2Anim = countdown2.GetComponent<Animator> ();
		countdown1Anim = countdown1.GetComponent<Animator> ();
		countdownGoAnim = countdownGo.GetComponent<Animator> ();
		StartCoroutine (RunCountdown ());
	}

	public IEnumerator RunCountdown() {
		SoundManager.GetInstance().StopPlayingVoiceOver();
		SoundManager.GetInstance().PlayVoiceOverClip(countdownClip);
		yield return new WaitForSeconds (0.25f);

		countdown3.SetActive (true);
		countdown3Anim.Play ("CountdownGoAnimation");
		yield return new WaitForSeconds (1.0f);
		countdown3.SetActive (false);

		countdown2.SetActive (true);
		countdown2Anim.Play ("CountdownGoAnimation");
		yield return new WaitForSeconds (1.0f);
		countdown2.SetActive (false);

		countdown1.SetActive (true);
		countdown1Anim.Play ("CountdownGoAnimation");
		yield return new WaitForSeconds (1.0f);
		countdown1.SetActive (false);

		countdownGo.SetActive (true);
		countdownGoAnim.Play ("CountdownGoAnimation");
		yield return new WaitForSeconds (1.0f);
		countdownGo.SetActive (false);
		Destroy (gameObject);
	}

}
