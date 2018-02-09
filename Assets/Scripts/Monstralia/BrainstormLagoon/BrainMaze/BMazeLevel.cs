using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BMazeLevel : MonoBehaviour {
    [Range (0.05f, 1.0f)]
    public float monsterScale, factoryScale;
    public Door doorObject;
    public BMazeFinishline finishLine;
	public Transform startingLocation;
    public BMazePickup[] pickups;
    public int numOfPickupsToSpawn;

    private bool isDoorOpen = false;
    [SerializeField] private GameObject mazeArea;
    [SerializeField] private List<Transform> pointList = new List<Transform> ();

    private void Awake () {
        if (mazeArea) {
            print (gameObject.name);
            GetPointList ();
            ChooseMonsterSpawn ();
        }
    }

    void GetPointList() {
        for (int i = 0; i < mazeArea.transform.childCount; i++) {
            BMazePoint child = mazeArea.transform.GetChild (i).GetComponent<BMazePoint> ();
            if (child)
                pointList.Add (child.transform);
        }
    }

    void ChooseMonsterSpawn () {
        startingLocation = pointList.RemoveRandom ();
    }

    void SpawnPickups () {
        BMazeManager.Instance.SetFactoryScale (factoryScale);

        List<GameObject> prefabList = new List<GameObject> ();
        prefabList.AddRange (BMazeManager.Instance.GetFactoryList ());
        int listCount = prefabList.Count;

        if (numOfPickupsToSpawn < listCount) {
            listCount = numOfPickupsToSpawn;
        }

        for (int i = 0; i < listCount; i++) {
            BMazeManager.Instance.OrderPickupFromFactory (prefabList.RemoveRandom(), pointList.RemoveRandom ());
            numOfPickupsToSpawn--;
        }

        prefabList.AddRange (BMazeManager.Instance.GetFactoryList ());

        for (int i = 0; i < numOfPickupsToSpawn; i++) {
            BMazeManager.Instance.OrderPickupFromFactory (prefabList.GetRandomItem (), pointList.RemoveRandom ());
        }
    }

    void Start() {
        if (mazeArea) {
            SpawnPickups ();
        }
	}

    public void UnlockDoor () {
        if (!isDoorOpen) {
            AudioClip unlockeddoor = BMazeManager.Instance.voData.FindVO ("unlockeddoor");
            SoundManager.Instance.AddToVOQueue (unlockeddoor);

            doorObject.OpenDoor ();
            finishLine.UnlockFinishline ();
            isDoorOpen = true;
        }
    }
}
