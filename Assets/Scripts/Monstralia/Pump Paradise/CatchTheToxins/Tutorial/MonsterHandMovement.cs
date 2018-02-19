using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterHandMovement : MonoBehaviour {

    public GameObject monsterhand;
    public GameObject player;
    public float speed0;
    bool stop;
    bool stop2;
    bool stop3;
	bool stop4;
	bool stop5;
	public AudioClip Tutorial1;
	public AudioClip Tutorial4;
	public AudioClip Tutorial5;

	private AudioSource source;
	private AudioSource source4;
	private AudioSource source5;

	void Awake()
	{
		source = GetComponent<AudioSource>();
		source4 = GetComponent<AudioSource>();
		source5 = GetComponent<AudioSource>();

	}

    // Use this for initialization
    void Start()
    {
        //prefabCopy = Instantiate(monsterHand, new Vector2(0.4f, -12.52f), Quaternion.identity);
        monsterhand.SetActive(true);
        stop = false;
        stop2 = false;
        stop3 = false;
		stop4 = false;
		stop5 = false;
    }


    // Update is called once per frame
    void Update()
    {
//        if (stop == false)
//        {
//            monsterhand.transform.position += Vector3.left * speed0;
//        }
//		if (monsterhand.transform.position == new Vector3(-6.190002f, -15f) && stop == false)
//        {
//            stop = true;
//			//source4.PlayOneShot (Tutorial4);
//        }
//
//        if (stop == true && stop2 == false)
//        {
//            monsterhand.transform.position += Vector3.right * speed0;
//        }
//
//		if (monsterhand.transform.position == new Vector3(9.26f, -15f) && stop2 == false)
//        {
//            stop2 = true; 
//			//source5.PlayOneShot (Tutorial5);
//        }
//
//        if (stop3 == false && stop2 == true)
//        {
//            //monsterhand.SetActive(false);
//            monsterhand.transform.position += Vector3.left * speed0;
//        }
//		if (monsterhand.transform.position == new Vector3(0.2999997f, -15f) && stop2 == true)
//        {
//            stop3 = true;
//        }
//        if (stop3 == true)
//        {
//            monsterhand.SetActive(false);
//        }

    }
}

