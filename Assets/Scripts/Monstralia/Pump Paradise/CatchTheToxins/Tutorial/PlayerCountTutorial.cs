using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerCountTutorial : MonoBehaviour
{
//    public Text CountText;
//    public Text timerText;
    public AudioClip Gulp;
    public AudioClip Bounce;

    private AudioSource source;
    private int count;
//    private float startTime;

    void Awake()
    {

        source = GetComponent<AudioSource>();


    }

    void Start()
    {
        count = 0;

//        SetCountText();

        //startTime = (int)Time.time;
//		startTime = 0;
    }

    void Update()
    {
        //Timer
//		float t = 60 - Time.timeSinceLevelLoad - startTime;
//
//        string seconds = (t % 60).ToString("f0");
//
//        timerText.text = seconds;

        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // pick up
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            source.PlayOneShot(Gulp);

			count = count + 1;


			if (count > 5)
			{
				//FindObjectOfType<GameManager>().CompleteLevel();
			}
        }

        if (other.gameObject.CompareTag("RBC"))
        {
            source.PlayOneShot(Bounce);
        }
    }

//    void SetCountText()
//    {
//        //counting text update
//        CountText.text = "Count: " + count.ToString();
//
//    }
}
