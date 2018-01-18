using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrainMazeReviewMonster : MonoBehaviour {

    public float speed;
    public Sprite[] monsterSprites;
    Dictionary <int, Sprite> monsterColor;

    private Rigidbody2D rigBody;

    private void Start() {
        rigBody = GetComponent<Rigidbody2D> ();
        monsterColor = new Dictionary<int, Sprite>();
        for (int i = 0; i < monsterSprites.Length; i++) {
            monsterColor.Add(i, monsterSprites[i]);
        }
        GetComponent<Image>().sprite = monsterColor[(int)GameManager.Instance.GetMonsterType()];
    }

    private void OnMouseDrag() {
        if (Input.GetMouseButton(0)) {
            //transform.position += new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")) * speed;
            Vector3 screenPoint = Camera.main.ScreenToWorldPoint(new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0f));
            screenPoint.z = transform.position.z;
            //transform.position = screenPoint;
            rigBody.MovePosition (screenPoint);
        }
    }

}
