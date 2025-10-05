using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    private readonly float _interval = 2f;
    private float _timer;
    private Renderer _renderer;

    private void Awake()
    {
        Debug.Log("ChangeColor Awake");
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer < _interval) return;
        _timer = 0f;

        if (_renderer)
        {
            Debug.Log("Changing the color");
            _renderer.material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}