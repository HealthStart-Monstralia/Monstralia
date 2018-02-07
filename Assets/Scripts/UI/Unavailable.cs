using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unavailable : MonoBehaviour {
    public string notificationText;

    public void Notify () {
        GameManager.Instance.CreateNotification (notificationText);
    }
}
