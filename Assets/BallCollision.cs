using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject hitMarkerPrefab; // The dot prefab

    void OnCollisionEnter(Collision collision)
    {
        // Check if the ball hits the target
        if (collision.gameObject.CompareTag("Target"))
        {
            // Get the hit point
            Vector3 hitPoint = collision.contacts[0].point;

            // Instantiate a hit marker at the point of collision
            Instantiate(hitMarkerPrefab, hitPoint, Quaternion.identity);
        }

        // Destroy the ball after it hits the target
        Destroy(gameObject);
    }
}
