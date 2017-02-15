using UnityEngine;
using System;

public class SenseObjectBehavior : MonoBehaviour {

	[Serializable]
	public enum Sense {
		See,
		Smell,
		Touch,
		Hear,
		Taste
	};

	public Sense sense;
}
