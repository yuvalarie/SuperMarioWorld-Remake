namespace Mario.Player
{
    public enum MarioState
    {
        Regular = 0,
        Super = 1,
        Fire = 2
    }
    /**
     * The model for the player.
     */
    public class MarioModel
    {
        public MarioState State { get; private set; } = MarioState.Regular; // Initial state

        public bool TakeDamage()
        {
            if (State == MarioState.Fire || State == MarioState.Super)
            {
                State = MarioState.Regular;
                return false;
            }

            return true;
        }

        public void ApplyPowerUp(MarioState newState)
        {
            State = newState;
        }
    }
}