using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPersistent<T> : MonoBehaviour where T : Component {
    private static T instance = null;

    public static T Instance {
        get {
            if (!instance) {
                instance = (T)FindObjectOfType (typeof (T));

                if (!instance) {
                    print ("Instance returned NULL");
                }
            } 

            return instance;
        }
    }

    public void Awake () {
        if (!instance) {
            instance = this as T;
        } else {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }
}
