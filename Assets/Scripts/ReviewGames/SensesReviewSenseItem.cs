using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensesReviewSenseItem : MonoBehaviour {

    Image image;
    public Sprite mySprite;
    int senseToPickFromIndex, senseSpriteIndex;
    public Sprite[] touchImages, smellImages, tasteImages, seeImages, hearImages;
    List<Sprite[]> senseArrays;

    // Use this for initialization
    void Awake() {
        image = GetComponent<Image>();
        senseArrays = new List<Sprite[]>();
        senseArrays.Add(touchImages);
        senseArrays.Add(smellImages);
        senseArrays.Add(tasteImages);
        senseArrays.Add(seeImages);
        senseArrays.Add(hearImages);
        senseToPickFromIndex = Random.Range(0, senseArrays.Count);
        mySprite = senseArrays[senseToPickFromIndex][Random.Range(0, senseArrays[senseToPickFromIndex].Length)];
        image.sprite = mySprite;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
