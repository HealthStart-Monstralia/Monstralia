using UnityEngine;

public class Fader : MonoBehaviour {
    private Animator anim;

    private void Awake () {
        anim = GetComponent<Animator> ();
    }

    public void FadeIn() {
        anim.Play ("FadeIn", -1, 0f);
    }

    public void FadeOut() {
        anim.Play ("FadeOut", -1, 0f);
    }
}
