﻿using UnityEngine;

public class CameraCollisions : MonoBehaviour {

    public float minDistance = 1f;
    public float maxDistance = 4f;
    public float smooth = 10f;
    private Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;
    public float distance;

    private void Awake() {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;
    }

    private void Update() {
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if(Physics.Linecast(transform.parent.position, desiredCameraPos, out hit)) {
            distance = Mathf.Clamp((hit.distance * 0.8f), minDistance, maxDistance);

        }
        else {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
    }
}
