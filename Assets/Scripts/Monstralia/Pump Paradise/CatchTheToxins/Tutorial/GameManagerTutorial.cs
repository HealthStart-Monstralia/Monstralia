using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerTutorial : MonoBehaviour
{
	public GameObject completeLevelUI;
    public AudioClip Congratulation;

    private AudioSource source;

    void Awake()
    {

        source = GetComponent<AudioSource>();

    }
		

	public void CompleteLevel()
	{
		
		completeLevelUI.SetActive (true);
        source.PlayOneShot(Congratulation);
    }

}
