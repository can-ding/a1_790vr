using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonClick : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform target; 
    public float forceMin = 1000f;  
    public float forceMax = 1500f; 
    public float[] scoringRadii = { 0.5f, 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f, 4.5f };
    private bool wasPressedLastFrame = false;
    private Vector3 targetCenter;

    void Update()
    {
        if (Touchscreen.current.press.isPressed)
        {
            if (!wasPressedLastFrame) 
            {
                if (ballPrefab != null && target != null) 
                {
                    
                    Vector3 randomPosition = GetRandomPositionOnTarget();
                    GameObject ball = Instantiate(ballPrefab, transform.position, Quaternion.identity);
                    Rigidbody rb = ball.GetComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.AddForce((randomPosition - transform.position).normalized * UnityEngine.Random.Range(forceMin, forceMax));
                    StartCoroutine(EnableGravity(rb, 1.0f)); 
                    wasPressedLastFrame = true; 
                }
                else
                {
                    Debug.LogError("ballPrefab or target is not assigned in the Inspector!");
                }
            }
        }
        else
        {
            wasPressedLastFrame = false; 
        }
    }

    
    Vector3 GetRandomPositionOnTarget()
    {
        
        Renderer targetRenderer = target.GetComponent<Renderer>();
        Bounds bounds = targetRenderer.bounds;
        float randomX = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        float randomY = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(randomX, randomY, bounds.center.z); 
    }

    IEnumerator EnableGravity(Rigidbody rb, float delay)
    {
        yield return new WaitForSeconds(delay);
        rb.useGravity = true; 
    }

    public int CalculateScore(Vector3 hitPosition)
    {
        targetCenter = target.position;
        float distance = Vector3.Distance(hitPosition, targetCenter);

        
        for (int i = 0; i < scoringRadii.Length; i++)
        {
            if (distance <= scoringRadii[i])
            {
                return 10 - i; 
            }
        }

        return 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
           
            Vector3 hitPosition = collision.contacts[0].point;
            int score = CalculateScore(hitPosition);
            Debug.Log("Hit position: " + hitPosition + " Score: " + score);
        }
    }
}