namespace Mario.Movement.States
{
    /**
     * Interface for the player state.
     */
    public interface IPlayerState
    {
        void EnterState(PlayerController player);
        void UpdateState(PlayerController player);
        void ExitState(PlayerController player);
    }
}