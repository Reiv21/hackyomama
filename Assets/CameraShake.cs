using System;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    Vector2 originalPos; 
    bool shake;

    public float xShake;
    public float yShake;
    float zPos;

    void Start()
    {
        Shake();
    }

    public void Shake()
    {
        shake = true;
        zPos = Camera.main.transform.localPosition.z;
        originalPos = new Vector3(Camera.main.transform.localPosition.x, Camera.main.transform.localPosition.y, Camera.main.transform.localPosition.z);
    }

    void Update()
    {
        if(!shake) return;
        Vector2 sus = originalPos + UnityEngine.Random.insideUnitSphere * new Vector2(xShake, yShake);
        Camera.main.transform.localPosition = new Vector3(sus.x, sus.y, zPos);
    }
}
