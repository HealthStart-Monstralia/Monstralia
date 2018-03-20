using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensesClickInput : MonoBehaviour {

    private void Start () {
        if (!GetComponent<Collider2D>()) {
            gameObject.AddComponent<CircleCollider2D> ();
        }
    }
    private void OnMouseDown () {
        SensesGameManager.Instance.OnItemSense (GetComponent<SensesItem> ());
    }
}
