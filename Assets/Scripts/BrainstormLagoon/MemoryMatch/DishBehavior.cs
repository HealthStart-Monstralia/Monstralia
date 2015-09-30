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
//		moveUp = new Vector3(topRigidbody.transform.position.x, topRigidbody.transform.localPosition.y + 13f, topRigidbody.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetFood(Food food) {
		myFood = gameObject.GetComponentsInChildren<Food>()[0];
	}

	IEnumerator OnMouseDown() {
		if(!isGuessing && MemoryMatchGameManager.GetInstance().isGameStarted()) {
			isGuessing = true;
			top.GetComponent<SpriteRenderer>().enabled = false;
			Vector2 originalPos = topRigidbody.transform.position;
			if(MemoryMatchGameManager.GetInstance().GetFoodToMatch().name != myFood.name) {
				yield return new WaitForSeconds(1.5f);
				top.GetComponent<SpriteRenderer>().enabled = true;
			}
			else {
				top.GetComponent<SpriteRenderer>().enabled = false;
				print(gameObject.name);
				MemoryMatchGameManager.GetInstance().ChooseFoodToMatch();
			}
			isGuessing = false;
			return true;
		}
	}
}
