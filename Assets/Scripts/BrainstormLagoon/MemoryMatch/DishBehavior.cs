using UnityEngine;
using System.Collections;

public class DishBehavior : MonoBehaviour {

	private Food myFood;
	private static bool isGuessing;
	private float inverseMoveTime;
	private Rigidbody2D topRigidbody;
	private Vector3 moveUp;

	public GameObject top;
	public GameObject bottom;

	// Use this for initialization
	void Start () {
		isGuessing = false;
		inverseMoveTime = 1/.1f;
		topRigidbody = gameObject.GetComponentInChildren<Rigidbody2D>();
		moveUp = new Vector3(topRigidbody.transform.position.x, topRigidbody.transform.localPosition.y + 13f, topRigidbody.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetFood(Food food) {
		myFood = gameObject.GetComponentsInChildren<Food>()[0];
	}

	IEnumerator OnMouseDown() {
		if(!isGuessing) {
			isGuessing = true;
			top.GetComponent<SpriteRenderer>().enabled = false;
			Vector2 originalPos = topRigidbody.transform.position;
//			StartCoroutine(SmoothMovement(top.gameObject, moveUp));
			if(MemoryMatchGameManager.GetInstance().GetFoodToMatch().name != myFood.name) {
				yield return new WaitForSeconds(1.5f);
				top.GetComponent<SpriteRenderer>().enabled = true;
//				StartCoroutine(SmoothMovement(top.gameObject, originalPos));
			}
			else {
				top.GetComponent<SpriteRenderer>().enabled = false;
				MemoryMatchGameManager.GetInstance().ChooseFoodToMatch();
			}
			isGuessing = false;
			return true;
		}
	}

	IEnumerator SmoothMovement(GameObject obj, Vector3 end) {
		float sqrRemainingDistance = (obj.transform.position - end).sqrMagnitude;
		
		while(sqrRemainingDistance > float.Epsilon) {
			Vector3 newPosition = Vector3.MoveTowards(topRigidbody.position, end, inverseMoveTime * Time.deltaTime);
			topRigidbody.MovePosition(newPosition);
			sqrRemainingDistance = (obj.transform.position - end).sqrMagnitude;
			yield return null;
		}
	}
}
