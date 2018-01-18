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
                    /*
                    GameObject obj = new GameObject {
                        name = typeof (T).Name,
                        hideFlags = HideFlags.DontSave
                    };
                    instance = obj.AddComponent<T> ();
                    */
                    print ("Instance returned NULL");
                }
            } 

            return instance;
        }
    }

    public virtual void Awake () {
        if (!instance) {
            instance = this as T;
        } else {
            Destroy (gameObject);
        }

        DontDestroyOnLoad (gameObject);
    }
}
