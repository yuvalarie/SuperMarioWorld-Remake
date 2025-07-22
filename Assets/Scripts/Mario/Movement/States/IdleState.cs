
namespace Mario.Movement.States
{
    /**
     * The state where the player is idle.
     */
    public class IdleState : IPlayerState
    {

        public void EnterState(PlayerController player)
        {
        }

        public void UpdateState(PlayerController player)
        {
            if (!player.IsDead())
            {
                if (player.MoveAction.IsPressed())
                {
                    player.SetState(new RunningWalkingState());
                }
                else if (player.JumpAction.IsPressed() && player.IsGrounded())
                {
                    player.SetState(new JumpingState());
                }
                else if (player.Crouch.IsPressed())
                {
                    player.SetState(new CrouchingDownState());
                }
                else if (player.LookAction.IsPressed())
                {
                    player.SetState(new LookingUpState());
                }
                else if (player.SpinJump.WasPerformedThisFrame())
                {
                    player.SetState(new SpinJumpState());
                }
                else if (player.AttackAction.IsPressed() && player.IsSuperMario())
                {
                    player.SetState(new AttackState());
                }
            }
        }

        public void ExitState(PlayerController player)
        {
        }
    }
}