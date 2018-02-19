using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicFade : MonoBehaviour {

    [SerializeField]
    private AnimationCurve fadeCurve;
    Renderer _renderer;
    Color _color;
    float _timer = 0f;
    int integer = 0;
    public int FrameStart;
    public GameObject Object;
    

    private void Awake()
    {
        _renderer = GetComponent<Renderer>(); // do this in awake, it has an impact on performances in Update
        _color = _renderer.material.color;
    }

    private void Update()
    {
        if (integer >= FrameStart)
        {
            _timer += Time.deltaTime;
            _color.a = fadeCurve.Evaluate(_timer);
            _renderer.material.color = _color;
        }
        integer += 1;
        if (_color.a <= 0)
        {
            Object.SetActive(false);
        }

    }

}
