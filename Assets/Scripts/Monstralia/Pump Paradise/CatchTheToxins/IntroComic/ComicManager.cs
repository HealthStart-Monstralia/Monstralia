using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour {

    public AudioClip Comic1;
    public AudioClip Comic2;
    public AudioClip Comic3;
    public AudioClip Comic4;
    public GameObject Image;
    public GameObject Image2;
    public GameObject Image3;
    public GameObject Image4;

    private AudioSource source;
    private AudioSource source2;
    private AudioSource source3;
    private AudioSource source4;
    private bool Played;
    private bool Played2;
    private bool Played3;
    private bool Played4;

    void Awake()
    {

        source = GetComponent<AudioSource>();
        source2 = GetComponent<AudioSource>();
        source3 = GetComponent<AudioSource>();
        source4 = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start () {
        Played = false;
        Played2 = false;
        Played3 = false;
        Played4 = false;

    }
	
	// Update is called once per frame
	void Update () {

        if (Image.activeSelf == true && Played == false)
        {
            
            source.PlayOneShot(Comic1);
            Played = true;
        }

        if (!source.isPlaying)
        {
            if(Played2 == false)
            {
                Image2.SetActive(true);
                source2.PlayOneShot(Comic2);
                Played2 = true;
            }
        }

        if (!source2.isPlaying)
        {
            if (Played3 == false)
            {
                Image3.SetActive(true);
                source3.PlayOneShot(Comic3);
                Played3 = true;
            }
        }

        if (!source3.isPlaying)
        {
            if (Played4 == false)
            {
                Image4.SetActive(true);
                source4.PlayOneShot(Comic4);
                Played4 = true;
            }
        }
        if (!source4.isPlaying)
        {
            if (Played4 == true)
            {
                LoadNextLevel();
            }
        }
       

    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
