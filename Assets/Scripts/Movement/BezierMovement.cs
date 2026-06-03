using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 2f; 
    public float fixedYPosition = 0f; 
    public float moveRange = 25f; 
    public int curveResolution = 20; 

    private Vector3[] curvePoints; 
    private int currentSegmentIndex = 0; 
    private float t = 0f; 

    void Start()
    {
        GenerateRandomCurve();
    }

    void Update()
    {
        MoveAlongCurve();
        if (currentSegmentIndex >= curvePoints.Length - 1)
        {
            GenerateRandomCurve();
        }
    }

    void GenerateRandomCurve()
    {
        Vector3 start = transform.position;
        Vector3 control1 = GetRandomPoint();
        Vector3 control2 = GetRandomPoint();
        Vector3 end = GetRandomPoint();

        curvePoints = GenerateBezierCurve(start, control1, control2, end, curveResolution);
        currentSegmentIndex = 0;
        t = 0f;
    }

    void MoveAlongCurve()
    {
        if (currentSegmentIndex < curvePoints.Length - 1)
        {
            t += speed * Time.deltaTime / Vector3.Distance(curvePoints[currentSegmentIndex], curvePoints[currentSegmentIndex + 1]);

            transform.position = Vector3.Lerp(curvePoints[currentSegmentIndex], curvePoints[currentSegmentIndex + 1], t);
            transform.position = new Vector3(transform.position.x, fixedYPosition, transform.position.z); 

            if (t >= 1f)
            {
                t = 0f;
                currentSegmentIndex++;
            }
        }
    }

    Vector3 GetRandomPoint()
    {
        float randomX = Random.Range(-moveRange, moveRange);
        float randomZ = Random.Range(-moveRange, moveRange);
        return new Vector3(randomX, fixedYPosition, randomZ);
    }

    Vector3[] GenerateBezierCurve(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, int resolution)
    {
        Vector3[] points = new Vector3[resolution + 1];
        for (int i = 0; i <= resolution; i++)
        {
            float t = i / (float)resolution;
            points[i] = Mathf.Pow(1 - t, 3) * p0 +
                        3 * Mathf.Pow(1 - t, 2) * t * p1 +
                        3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                        Mathf.Pow(t, 3) * p3;
        }
        return points;
    }
}
