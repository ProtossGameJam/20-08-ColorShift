using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;

    private float shakeTime, shakePower, fadeTime, shakeRotation;
    public float rotationMultiplier;

    void Awake() {
        instance = this;
    }
    void LateUpdate()
    {
        if(shakeTime > 0)
        {
            shakeTime -= Time.deltaTime;

            float xAmount = Random.Range(-1f,1f) * shakePower;
            float yAmount = Random.Range(-1f,1f) * shakePower;

            transform.position += new Vector3 (xAmount,yAmount,0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, fadeTime * Time.deltaTime);

            shakeRotation = Mathf.MoveTowards(shakeRotation, 0f, fadeTime * rotationMultiplier * Time.deltaTime);
        }

        transform.rotation = Quaternion.Euler(0f,0f, shakeRotation * Random.Range(-1f,1f));
    }

    public void ShakeCamera(float length, float Power)
    {
        shakeTime = length;
        shakePower = Power;

        fadeTime = Power / length;

        shakeRotation = Power * rotationMultiplier;
    }
}
