using System;
using System.Collections;
using UnityEngine;

namespace CameraScripts
{
    /**
     * This class is responsible for moving the camera from the starting position to the end position.
     */
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Vector3 startPosition = new Vector3(0,0,-10); // Starting position of the camera
        [SerializeField] private Vector3 endPosition = new Vector3(90,0,-10);   // End position for the camera movement
        [SerializeField] private float speed = 1f;      // Speed of the camera movement
        private bool _hasReachedEnd = false;
        public event Action OnCameraReachedEnd;
        
        private void Start()
        {
            transform.position = startPosition;
            StartCoroutine(MoveCamera());
        }

        private void Update()
        {
            if (_hasReachedEnd || !(Mathf.Abs(transform.position.x - endPosition.x) < 0.1f)) return;
            _hasReachedEnd = true;
            OnCameraReachedEnd?.Invoke();
        }

        private IEnumerator MoveCamera()
        {
            float journeyLength = Vector3.Distance(startPosition, endPosition);
            float startTime = Time.time;

            while (Vector3.Distance(transform.position, endPosition) > 0.01f)
            {
                float distanceCovered = (Time.time - startTime) * speed;
                float fractionOfJourney = distanceCovered / journeyLength;
                transform.position = Vector3.Lerp(startPosition, endPosition, fractionOfJourney);
                yield return null;
            }

            transform.position = endPosition; // Ensure the camera reaches the exact end position
        }
    }
}
