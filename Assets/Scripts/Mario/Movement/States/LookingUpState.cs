using UnityEngine;

namespace Mario.Movement.States
{
    /**
     * The state where the player is looking up.
     */
    public class LookingUpState : IPlayerState
    {
        private static readonly int IsLookingUp = Animator.StringToHash("IsLookingUp");

        public void EnterState(PlayerController player)
        {
            player.Animator.SetBool(IsLookingUp, true);
        }

        public void UpdateState(PlayerController player)
        {
            if (!player.LookAction.IsPressed())
            {
                player.SetState(new IdleState());
            }        
        }

        public void ExitState(PlayerController player)
        {
            player.Animator.SetBool(IsLookingUp, false);
        }
    }
}