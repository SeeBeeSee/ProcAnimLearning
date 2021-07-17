using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegTargeting : MonoBehaviour
{
    public Transform legRoot;
    public LayerMask groundLayer;

    Vector3 currentPosition;
    Vector3 newPosition;
    Vector3 oldPosition;
    public float stepDistance;
    public float stepHeight;
    public float speed;

    float legLerp = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        currentPosition = transform.position;
        newPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Keep the target where it is until we move too far away
        transform.position = currentPosition;

        // Find the position for the target below the hip of this leg
        Ray ray = new Ray(legRoot.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 10, groundLayer.value))
        {
            if (Vector3.Distance(newPosition, hitInfo.point) > stepDistance)
            {
                legLerp = 0;
                newPosition = hitInfo.point;
            }
        }

        // Interpolate target position
        if (legLerp < 1)
        {
            Vector3 footPos = Vector3.Lerp(oldPosition, newPosition, legLerp);
            // Give the leg a vertical movement while walking
            footPos.y += Mathf.Sin(legLerp * Mathf.PI) * stepHeight;

            currentPosition = footPos;
            legLerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(newPosition, 0.5f);
    }

}
