using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeBonesCamera : MonoBehaviour {

    public float dampTime = 0.1f;
    public GameObject target;
    public float xMinClamp = 0f;
    public float xMaxClamp = 0f;

    private Vector3 velocity = Vector3.zero;
    private float zVal = -100f;
    private Camera cam;
    private float offsetX = 0f;
    private float offsetY = 0f;

    void Start () {
        cam = GetComponent<Camera> ();
        if (!target)
            target = BridgeBonesManager.GetInstance ().monster.gameObject;
    }

    void moveCamera (Vector3 pos) {
        Vector3 point = new Vector3 (pos.x, pos.y, zVal);
        Vector3 delta = point - cam.ViewportToWorldPoint (new Vector3 (0.5f, 0.5f, 0f));
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp (transform.position, destination, ref velocity, dampTime);
    }

    private void LateUpdate () {
        moveCamera (target.transform.position);
        transform.position = new Vector3 (
            Mathf.Clamp (transform.position.x, xMinClamp, xMaxClamp),
            0f,
            -20f
        );
    }
}
