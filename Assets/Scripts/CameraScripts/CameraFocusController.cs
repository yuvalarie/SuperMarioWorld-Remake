using Mario.Movement;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace CameraScripts
{
    /**
     * This script is responsible for moving the camera to the left or right based on the player's input, by using a
     * child object of the player as a target to follow.
     */
    public class CameraFocusController : MonoBehaviour
    {
        [SerializeField] private PlayerController mario;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        [SerializeField] private float offsetDistance = 2f;
        [SerializeField] private float transitionSpeed = 2f;
        private Vector3 _targetPosition;
    
        void Start()
        {
            _targetPosition = mario.transform.position;
            //cinemachineCamera.Follow = transform;
        }

        void Update()
        {
            _targetPosition = mario.transform.position;
            if (mario.CameraRight.IsPressed())
            {
                _targetPosition += Vector3.right * offsetDistance;
            }
            else if (mario.CameraLeft.IsPressed())
            {
                _targetPosition += Vector3.left * offsetDistance;
            }
            transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * transitionSpeed);
        }
    }
}
