using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnTutorial : MonoBehaviour {

	public GameObject RBC;
    public GameObject enemyA;
    public GameObject enemyB;
    public GameObject enemyC;
    public GameObject goodCircle;
    public GameObject badCircle1;
	public GameObject badCircle2;
	public GameObject badCircle3;
    public GameObject monsterhand;
	public GameObject NYTScreen;
    bool stop;
    bool stop2;
    bool stop3;
    bool stop4;
	bool stop5;
	float timer;
	public AudioClip Tutorial1;
    public AudioClip Tutorial2;
    public AudioClip Tutorial3;
	public AudioClip NYTAudio;


	private AudioSource source1;
    private AudioSource source2;
    private AudioSource source3;

    void Awake()
    {
		source1 = GetComponent<AudioSource>();
        source2 = GetComponent<AudioSource>();
        source3 = GetComponent<AudioSource>();
        
    }
    // Use this for initialization
    void Start ()
	{
        stop = false;
        stop2 = false;
        stop3 = false;
        stop4 = false;
		stop5 = false;
        goodCircle.SetActive(false);
        badCircle1.SetActive(false);
		badCircle2.SetActive(false);
		badCircle3.SetActive(false);
		RBC.SetActive(false);
		NYTScreen.SetActive(false);
		source1.PlayOneShot (Tutorial1);
		timer = 0f;
    }

//	public void LoadNextLevel()
//	{
//		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
//	}

	// Update is called once per frame
	void Update () {
//		goodCircle.SetActive(false);
//		badCircle1.SetActive(false);
        if (monsterhand.activeSelf == false && stop == false)
        {
            //Instantiate(RBC, new Vector2(.1f, 15f), Quaternion.identity);
            RBC.SetActive(true);
            stop = true;
        }

		if (RBC.transform.position == new Vector3(0f, 0.099979f))
        {
            goodCircle.SetActive(true);
			//source2.PlayOneShot (Tutorial2);
            //stop2 = true;
        }

		if (RBC.transform.position == new Vector3(0f, 0.099979f) && stop2 == false)
		{
			
			source2.PlayOneShot (Tutorial2);
			stop2 = true;
		}

        if(RBC.activeSelf == false && stop2 == true && stop3 == false)
        {
            enemyA.SetActive(true);
			enemyB.SetActive(true);
			enemyC.SetActive(true);
            stop3 = true;
        }
		if (enemyA.transform.position == new Vector3(0f, 0.099979f))
		{
			badCircle1.SetActive(true);
			badCircle2.SetActive(true);
			badCircle3.SetActive(true);
		}
		if (enemyA.transform.position == new Vector3(0f, 0.099979f) && stop4 == false)
		{
			source3.PlayOneShot (Tutorial3);
			stop4 = true;
		}

		if (enemyC.activeSelf == false && stop4 == true && stop5 == false) 
		{
			//SceneManager.LoadScene ((SceneManager.GetActiveScene ().buildIndex+1));
			timer = 0f;
			NYTScreen.SetActive (true);
			source1.PlayOneShot (NYTAudio);
			stop5 = true;
		}

		if (enemyC.activeSelf == false && stop5 == true && timer == 100 ) 
		{
			NYTScreen.SetActive (false);

		}


		timer += 1;
    }
}
