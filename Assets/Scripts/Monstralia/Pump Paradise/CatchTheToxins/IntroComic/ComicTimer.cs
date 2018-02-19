using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicTimer : MonoBehaviour {

	int integer = 0;
	public GameObject Image;
	public GameObject Image2;
	public GameObject Image3;
	public GameObject Image4;

	// Update is called once per frame
	void Update () {

		if (integer == 600)
		{
			Image2.gameObject.SetActive(true);
			//Image.gameObject.SetActive(false);
		}
		if (integer == 800)
		{
			Image3.gameObject.SetActive(true);
			//Image2.gameObject.SetActive(false);
		}
		if (integer == 950)
		{
			Image4.gameObject.SetActive(true);
			//Image3.gameObject.SetActive(false);
		}
		if (integer == 1650) 
		{
			LoadNextLevel ();
		}
		integer += 1;

	}

	public void LoadNextLevel()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
	}
}
