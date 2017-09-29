using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGauge : MonoBehaviour {
    public Image scoreBar;

    private Coroutine coroutine;
    private bool isTransitioning;

    public void SetProgress(float progress) {
        scoreBar.fillAmount = progress;
    }

    public void SetProgressTransition(float progress) {
        if (isTransitioning)
            StopCoroutine (coroutine);
        coroutine = StartCoroutine (ProgressTransition(scoreBar.fillAmount, progress));
    }

    IEnumerator ProgressTransition(float initial, float desired) {
        isTransitioning = true;
        float t = 0f;
        while (t < 1f) {
            t += Time.deltaTime * 4;
            scoreBar.fillAmount = Mathf.Lerp (initial, desired, t);
            yield return null;
        }
        isTransitioning = false;
    }
}
