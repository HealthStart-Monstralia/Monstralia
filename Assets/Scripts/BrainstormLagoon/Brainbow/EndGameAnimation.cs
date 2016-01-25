using UnityEngine;
using System.Collections;

public class EndGameAnimation : MonoBehaviour {
	
	public void PlayEndGameAnim(AbstractGameManager gameManager, GameObject endGameAnimation) {

		GameObject animation = (GameObject)Instantiate(endGameAnimation);
		//animation.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameManager.instance.getMonster());
		animation.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(GameManager.GetInstance().getMonster());
		// when when monster collides with food, destroy the food

		gameManager.GameOver ();
	}
}
