using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandButton : MonoBehaviour {
    public DataType.IslandSection section;
    public bool isUnlocked = true;
    public bool lockForPublicBuilds = true;
    public string notificationText;

    [SerializeField] private Button buttonOfIsland;
    [SerializeField] private SwitchScene switchScene;

    private void Awake () {
        if (lockForPublicBuilds) {
            // Unlock section if test build, lock if public
#if (TEST_BUILD)
                isUnlocked = true;
#else
                isUnlocked = false;
#endif
        }

    }

    public void OnButtonPress () {
        if (isUnlocked) {
            SwitchScene ();
        }
        else {
            Notify ();
        }
    }

    public void SwitchScene () {
        switchScene.LoadIslandSection (section);
    }

    public void Notify () {
        GameManager.Instance.CreateNotification (notificationText);
    }
}
