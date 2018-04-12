using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneBridgeCamera : MonoBehaviour {

    public float dampTime = 0.1f;
    public GameObject target;
    public float xMinClamp = 0f;
    public float xMaxClamp = 0f;

    private Vector3 velocity = Vector3.zero;
    private float zVal = -100f;
    private Camera cam;

    private void Awake () {
        cam = GetComponent<Camera> ();
    }

    void Start () {
        if (!target)
            target = BoneBridgeManager.Instance.monster.gameObject;
        if (target) transform.position = target.transform.position;
    }

    void MoveCamera (Vector3 pos) {
        Vector3 point = new Vector3 (pos.x, pos.y, zVal);
        Vector3 delta = point - cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0f));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
        transform.position = new Vector3 (
            Mathf.Clamp (transform.position.x, xMinClamp, xMaxClamp),
            0f,
            -20f
        );
    }

    private void LateUpdate () {
        MoveCamera (target.transform.position);
    }
}
