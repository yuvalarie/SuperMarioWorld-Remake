using Sound;
using UnityEngine;

namespace Mario.Movement.States
{
    /**
     * The state where the player is jumping.
     */
    public class JumpingState : IPlayerState
    {
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        public void EnterState(PlayerController player)
        {
            player.Animator.SetBool(IsJumping, true);
            player.Rigidbody2D.linearVelocity = new Vector2(player.Rigidbody2D.linearVelocity.x, player.JumpForce);
            SoundManager.Instance.PlaySfx(SoundType.Jump);
        }

        public void UpdateState(PlayerController player)
        {
            // Handle horizontal movement while in the air
            Vector2 movement = player.MoveAction.ReadValue<Vector2>();
            float horizontal = movement.x;

            float moveSpeed = player.IsRunning ? player.RunSpeed : player.WalkSpeed;
            Vector2 velocity = player.Rigidbody2D.linearVelocity;
            velocity.x = horizontal * moveSpeed; // Allow movement in the air
            player.Rigidbody2D.linearVelocity = velocity;

            // Handle direction flipping
            if (horizontal > 0)
            {
                player.transform.localScale = new Vector3(-4, 4, 4); // Facing right
                player.FacingRight = true;
            }
            else if (horizontal < 0)
            {
                player.transform.localScale = new Vector3(4, 4, 4); // Facing left
                player.FacingRight = false;
            }
            if (player.IsGrounded())
            {
                player.SetState(new IdleState()); // Switch back to idle when landing
            }        
        }

        public void ExitState(PlayerController player)
        {
            player.Animator.SetBool(IsJumping, false);
        }
    }
}