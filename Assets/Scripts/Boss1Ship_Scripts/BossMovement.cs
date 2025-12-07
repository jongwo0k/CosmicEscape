using System;
using System.Collections;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    [Header("기본 움직임")]
    public float floatAmplitude = 0.8f;   // 위아래 흔들리는 크기
    public float floatSpeed = 2f;         // 위아래 속도

    public float sideAmplitude = 2f;      // 좌우 이동 폭
    public float sideSpeed = 1.5f;        // 좌우 이동 속도

    [Header("패턴 연출용 흔들림")]
    public float shakeIntensity = 0.3f;
    public float shakeDuration = 0.25f;

    Vector3 _startPos;
    Coroutine _shakeRoutine;

    void Start()
    {
        _startPos = transform.position;
        StartCoroutine(FloatMoveLoop());
    }

    // 기본 둥둥 떠있는 연출
    IEnumerator FloatMoveLoop()
    {
        float t = 0f;

        while (true)
        {
            t += Time.deltaTime;

            float y = Mathf.Sin(t * floatSpeed) * floatAmplitude;
            float x = Mathf.Cos(t * sideSpeed) * sideAmplitude;

            Vector3 targetPos = _startPos + new Vector3(x, y, 0f);
            transform.position = targetPos;

            yield return null;
        }
    }

    // 외부에서 호출 가능 (패턴 중 연출용)
    public void Shake()
    {
        if (_shakeRoutine != null)
            StopCoroutine(_shakeRoutine);

        _shakeRoutine = StartCoroutine(ShakeRoutine());
    }

    IEnumerator ShakeRoutine()
    {
        float t = 0f;
        Vector3 origin = transform.position;

        while (t < shakeDuration)
        {
            t += Time.deltaTime;
            Vector3 offset = UnityEngine.Random.insideUnitSphere * shakeIntensity;
            transform.position = origin + offset;
            yield return null;
        }

        transform.position = origin;
    }
}
