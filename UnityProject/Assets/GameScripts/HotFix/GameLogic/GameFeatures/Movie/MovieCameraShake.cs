using System.Collections;
using UnityEngine;

public class MovieCameraShake : MonoBehaviour {
    private Transform camTransform = null;
    public float shakeTime = 2.0f;
    public float shakeRange = 3.0f;
    public float shakeSpeed = 2.0f;

    public void StartShake( Transform camTransform, float shakeTime, float shakeRange, float shakeSpeed)
    {
        this.camTransform = camTransform;
        this.shakeTime = shakeTime;
        this.shakeRange = shakeRange;
        this.shakeSpeed = shakeSpeed;

        StartCoroutine(Shake());
    }
    



    public IEnumerator Shake() 
    {
        Vector3 OrigPosition = camTransform.localPosition;
        float ElapsedTime = 0.0f;
        while (ElapsedTime < shakeTime) 
        {
            Vector3 RandomPoint = OrigPosition + Random.insideUnitSphere * shakeRange;
            camTransform.localPosition = Vector3.Lerp(camTransform.localPosition,RandomPoint,Time.deltaTime*shakeSpeed);
            yield return null;
            ElapsedTime += Time.deltaTime;
        }
        camTransform.localPosition = OrigPosition;
    }
}