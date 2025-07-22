using UnityEngine;

namespace CameraScripts
{
    /**
     * This script is responsible for moving the camera to follow the player, with the ability to lock the camera within
     */
    public class CameraManager : MonoBehaviour
    {
        [Header("Target Settings")]
        public Transform player; // Reference to the player

        [Header("Camera Settings")]
        [SerializeField] private float smoothSpeed = 0.125f; // How smooth the camera movement is
        [SerializeField] private Vector3 offset; // Offset from the player's position

        [Header("Camera Bounds")]
        public float minX = -10f; // Minimum X position of the camera
        public float maxX = 10f;  // Maximum X position of the camera
        public float minY = -5f;  // Minimum Y position of the camera
        public float maxY = 5f;   // Maximum Y position of the camera

        private Vector3 velocity = Vector3.zero;

        void LateUpdate()
        {
            if (player == null) return; // Ensure player is assigned

            // Desired position with offset
            Vector3 desiredPosition = player.position + offset;

            // Lock the camera within bounds
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

            // Smoothly interpolate the camera's position
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);

            // Apply the position to the camera
            transform.position = smoothedPosition;
        }
    }
}
