using Mario.Player;
using UnityEngine;

namespace Mario.Movement.States
{
    /**
     * The state where the player is running or walking.
     */
    public class RunningWalkingState : IPlayerState
    {
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        public void EnterState(PlayerController player)
        {
        }

        public void UpdateState(PlayerController player)
        {
            float horizontal = 0f;

            // Determine horizontal movement direction
            Vector2 movement = player.MoveAction.ReadValue<Vector2>();
            horizontal = movement.x;

            // Determine movement speed based on running or walking
            player.IsRunning = player.Sprint.IsPressed();
            bool isFireMode = (player.GetMarioState() == MarioState.Fire);

            if (isFireMode)
            {
                player.IsRunning = player.Sprint.IsPressed() && !player.Fire.IsPressed(); // Prevents running while firing
            }
            else
            {
                player.IsRunning = player.Sprint.IsPressed(); // Normal running behavior
            }
            float moveSpeed = player.IsRunning ? player.RunSpeed : player.WalkSpeed;

            // Apply horizontal velocity
            Vector2 velocity = player.Rigidbody2D.linearVelocity;
            velocity.x = horizontal * moveSpeed;
            player.Rigidbody2D.linearVelocity = velocity;

            // Handle animations and direction flipping
            if (horizontal != 0)
            {
                if (player.IsRunning)
                {
                    player.Animator.SetBool(IsRunning, true); // Play running animation
                }
                else
                {
                    player.Animator.SetBool(IsWalking, true); // Play walking animation
                }

                if (horizontal > 0)
                {
                    player.transform.localScale = new Vector3(-4, 4, 4); // Facing right
                    player.FacingRight = true;
                }
                else
                {
                    player.transform.localScale = new Vector3(4, 4, 4); // Facing left
                    player.FacingRight = false;
                }
            }
            else
            {
                // If not moving, switch to idle state
                player.SetState(new IdleState());
            }

            // Transition to Jumping state if jump is pressed
            if (Input.GetKeyDown(KeyCode.X) && player.IsGrounded())
            {
                player.SetState(new JumpingState());
            }
        }

        public void ExitState(PlayerController player)
        {
            player.Animator.SetBool(IsWalking, false);
            player.Animator.SetBool(IsRunning, false);        
        }
    }
}