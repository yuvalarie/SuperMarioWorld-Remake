using Mario.Player;
using Sound;
using UnityEngine;

namespace Mario.Points
{
    /**
     * A class that represents a coin in the game.
     */
    public class Coin : MonoBehaviour
    {
        [SerializeField] private int value; // Value of the coin

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var marioPresenter = other.GetComponent<MarioPresenter>();
                if (marioPresenter != null)
                {
                    marioPresenter.AddPoints(value);
                    SoundManager.Instance.PlaySfx(SoundType.Coin);
                    Destroy(gameObject); // Remove the coin from the game
                }
            }
        }
    }
}
