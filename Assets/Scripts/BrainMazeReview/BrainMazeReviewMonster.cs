using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainMazeReviewMonster : MonoBehaviour {

    public float speed;
    Vector3 clickPos;
    Vector3 changeInMousePos;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            clickPos = Input.mousePosition;
        }
        if (Input.GetKey(KeyCode.Mouse0)) {
            changeInMousePos = clickPos - Input.mousePosition;
            transform.position -= changeInMousePos * Time.deltaTime * speed;
        }
    }
    private void FixedUpdate() {
        //transform.position -= changeInMousePos * Time.deltaTime * speed;
    }
    /*
    private void Start() {
        StartDragMonster();
    }
    public void StartDragMonster() {
        print("ppoi");
        StartCoroutine(DragMonster());
 
    }
    public void StopDragMonster() {
        StopCoroutine(DragMonster());
    }
	
    IEnumerator DragMonster() {
        while (true) {

            transform.position = Input.mousePosition;

            yield return null;
        }
    }*/
}
