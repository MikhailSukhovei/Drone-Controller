using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraDroneFolowing : MonoBehaviour
{
    #region Variables
    private Transform cam;

    [SerializeField] private Transform drone;
    [SerializeField] private float lerpSpeed = 2f;

    private Vector3 finalDirection;
    #endregion

    #region Main Methods
    private void Start()
    {
        cam = GetComponent<Camera>().transform;
    }

    void FixedUpdate()
    {
        if (!drone)
        {
            return;
        }

        Vector3 heading = drone.position - cam.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        finalDirection.x = Mathf.Lerp(finalDirection.x, direction.x, Time.deltaTime * lerpSpeed);
        finalDirection.y = Mathf.Lerp(finalDirection.y, direction.y, Time.deltaTime * lerpSpeed);
        finalDirection.z = Mathf.Lerp(finalDirection.z, direction.z, Time.deltaTime * lerpSpeed);

        cam.rotation = Quaternion.LookRotation(finalDirection);
    }
    #endregion
}
