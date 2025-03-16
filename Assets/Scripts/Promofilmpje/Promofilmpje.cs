using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PromofilmpjeStartingScene : MonoBehaviour
{
    public static PromofilmpjeStartingScene instance;

    public Camera promoCamera;
    public List<Vector3> controlPoints = new List<Vector3>();
    public List<Vector3> lookAtPoints = new List<Vector3>();
    public float duration;

    private float timeElapsed = 0f;

    void Start()
    {
        CreateSingleton();
    }

    void Update()
    {
        if (timeElapsed < duration && controlPoints.Count >= 2)
        {
            float t = timeElapsed / duration;
            t = EaseInOut(t);

            Vector3 position = GetBezierPosition(t, controlPoints);
            promoCamera.transform.position = position;

            Vector3 lookAtPosition = GetBezierPosition(t, lookAtPoints);
            promoCamera.transform.LookAt(lookAtPosition);

            timeElapsed += Time.deltaTime;
        }

    }

    private Vector3 GetBezierPosition(float t, List<Vector3> points)
    {
        List<Vector3> tempPoints = new List<Vector3>(points);

        while (tempPoints.Count > 1)
        {
            for (int i = 0; i < tempPoints.Count - 1; i++)
            {
                tempPoints[i] = Vector3.Lerp(tempPoints[i], tempPoints[i + 1], t);
            }
            tempPoints.RemoveAt(tempPoints.Count - 1);
        }

        return tempPoints[0];
    }

    private float EaseInOut(float t)
    {
        return t * t * (3f - 2f * t);
    }

    void CreateSingleton()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
