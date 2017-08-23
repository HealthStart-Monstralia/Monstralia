using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainMazeReviewMonster : MonoBehaviour {

    public float speed;
    public Sprite[] monsterSprites;
    Dictionary <int, Sprite> monsterColor;

    private void Start() {
        monsterColor = new Dictionary<int, Sprite>();
        for (int i = 0; i < monsterSprites.Length; i++) {
            monsterColor.Add(i, monsterSprites[i]);
        }
        GetComponent<Image>().sprite = monsterColor[(int)GameManager.monsterType];
    }

    private void OnMouseDrag() {
        if (Input.GetMouseButton(0)) {
            transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * speed;
        }
    }

}
