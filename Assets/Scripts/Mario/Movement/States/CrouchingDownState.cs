using UnityEngine;

namespace Mario.Movement.States
{
    /**
     * The state where the player is crouching down.
     */
    public class CrouchingDownState : IPlayerState
    {
        private static readonly int IsCrouching = Animator.StringToHash("IsCrouching");

        public void EnterState(PlayerController player)
        {
            player.Animator.SetBool(IsCrouching, true);
            player.StandingCollider.enabled = false;
            player.CrouchingCollider.enabled = true;        
        }

        public void UpdateState(PlayerController player)
        {
            if (!player.Crouch.IsPressed())
            {
                player.SetState(new IdleState()); // Transition to idle when crouching is no longer pressed
            }
        }

        public void ExitState(PlayerController player)
        {
            player.Animator.SetBool(IsCrouching, false);
            player.StandingCollider.enabled = true;
            player.CrouchingCollider.enabled = false;        
        }
    }
}