using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class BMazeMonsterMovement : MonoBehaviour {
    /* CREATED BY: Colby Tang
	 * GAME: Brain Maze
	 */
    public Vector2 gotoPos;
    public bool finished = false;
    public static bool isMonsterMovementAllowed = false;

    private Vector3 pointerOffset;
	private Vector3 cursorPos;
	private Rigidbody2D rigBody;

    private void OnDestroy () {
        isMonsterMovementAllowed = false;
    }

    void Start () {
        rigBody = GetComponent<Rigidbody2D> ();
        if (!rigBody)
            rigBody = gameObject.AddComponent<Rigidbody2D> ();
        rigBody.gravityScale = 0.0f;
        rigBody.freezeRotation = true;
        rigBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

	public void OnMouseDown() {
		if (isMonsterMovementAllowed) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			pointerOffset = Camera.main.ScreenToWorldPoint (cursorPos) - transform.position;
		}
	}

	public void OnMouseDrag() {
		if (isMonsterMovementAllowed) {
			cursorPos = Input.mousePosition;
			cursorPos.z -= (Camera.main.transform.position.z + 10f);
			MoveTowards (Camera.main.ScreenToWorldPoint (cursorPos) - pointerOffset);
		}
	}

    public void MoveTowards (Vector2 pos) {
		rigBody.MovePosition (Vector2.MoveTowards (rigBody.position, pos, 0.5f));
	}

    public void MoveToFinishLine (Vector2 pos) {
        StartCoroutine (MoveTo (pos));
    }

    IEnumerator MoveTo (Vector2 pos) {
        while (transform.position.x != pos.x && transform.position.y != pos.y) {
            transform.position = Vector2.MoveTowards (transform.position, pos, 0.01f);
            yield return null;
        }
    }
}