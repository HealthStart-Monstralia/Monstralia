using UnityEngine;

public class Fader : MonoBehaviour {
    public float animSpeed;
    private Animator anim;

    private void Awake () {
        anim = GetComponent<Animator> ();
        anim.SetFloat ("Speed", animSpeed);
    }

    public void FadeIn() {
        anim.Play ("FadeIn", -1, 0f);
    }

    public void FadeOut() {
        anim.Play ("FadeOut", -1, 0f);
    }

    public void FadeStayBlack () {
        anim.Play ("FadeStayBlack", -1, 0f);
    }
}
