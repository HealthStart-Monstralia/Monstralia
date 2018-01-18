using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component {
    private static T instance = null;

    public static T Instance {
        get {
            if (!instance) {
                instance = (T)FindObjectOfType (typeof (T));
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
    }

    protected void SetInstance(T obj) {
        if (!instance) instance = obj;
        else Destroy (obj);
    }

    /*
    public virtual void OnDestroy() {
        instance = null;
    }
    */
}
