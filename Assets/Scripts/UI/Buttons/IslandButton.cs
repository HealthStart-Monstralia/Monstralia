﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandButton : MonoBehaviour {
    public DataType.IslandSection section;
    public bool isUnlocked = true;
    public bool lockForPublicBuilds = true;
    public string notificationText;

    [SerializeField] private Color unavailableColor;
    [SerializeField] private Color availableColor;

    [SerializeField] private Button buttonOfIsland;
    [SerializeField] private SwitchScene switchScene;

    private void Awake () {
        if (lockForPublicBuilds) {
            // Unlock section if test build, lock if public
#if (TEST_BUILD)
                isUnlocked = true;
            ColorBlock colorBlock = buttonOfIsland.colors;
            colorBlock.normalColor = availableColor;
            buttonOfIsland.colors = colorBlock;
#else
            isUnlocked = false;
            ColorBlock colorBlock = buttonOfIsland.colors;
            colorBlock.normalColor = unavailableColor;
            buttonOfIsland.colors = colorBlock;
#endif

        }
        else {
            Vector3 originalScale = gameObject.transform.localScale;
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale (gameObject, originalScale, 0.75f).setEaseOutBack ();
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
