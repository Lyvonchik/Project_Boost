using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 moveVector = new Vector3(37f,0f,0f);
    [SerializeField] float moveFactor, period=5f;

    Vector3 startingPos;
    void Start()
    {
        startingPos = transform.position;
    }
    
    void Update()
    {
        if (period <= Mathf.Epsilon)
        {
            return;
        }

        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;//скорость
        float sinWave = Mathf.Sin(cycles * tau);

        moveFactor = sinWave / 2f + 0.5f;
        Vector3 offset = moveVector * moveFactor;
        transform.position = startingPos + offset;
    }
}
 