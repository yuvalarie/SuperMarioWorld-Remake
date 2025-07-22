using DG.Tweening;
using Sound;
using UnityEngine;

namespace Mario.Movement.States
{
    /**
     * The state where the player is spin jumping.
     */
    public class SpinJumpState : IPlayerState
    {
        private static readonly int IsSpinJump = Animator.StringToHash("IsSpinJump");

        public void EnterState(PlayerController player)
        {
            player.Animator.SetBool(IsSpinJump, true);
            player.transform.DOJump(new Vector2(player.transform.position.x, player.transform.position.y+ 2),
                3f, 1, 0.5f);
            SoundManager.Instance.PlaySfx(SoundType.SpinJump);
        }

        public void UpdateState(PlayerController player)
        {
            if (player.IsGrounded())
            {
                player.SetState(new IdleState()); // Transition back to idle when grounded
            }        
        }

        public void ExitState(PlayerController player)
        {
            player.Animator.SetBool(IsSpinJump, false);
            player.ResetFireballCount();
        }
    }
}