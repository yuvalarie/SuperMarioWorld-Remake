using Mario.Player;
using UnityEngine;

namespace PowerUps
{
    /**
     * A trigger for spawning power-ups.
     */
    public class PowerUpTrigger : MonoBehaviour
    {
        [SerializeField] private bool isFireFlowerTrigger;
        [SerializeField] private Transform spawnPoint;
        private bool _hasBeenTriggered = false;

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_hasBeenTriggered) return;
            if (other.collider.CompareTag("Player"))
            {
                var player = other.gameObject.GetComponent<MarioPresenter>();

                var powerUpManager = FindObjectOfType<PowerUpManager>();
                
                _hasBeenTriggered = true;
                // Spawn the correct power-up
                powerUpManager.SpawnPowerUp(spawnPoint, player.GetMarioState(), isFireFlowerTrigger);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_hasBeenTriggered) return;
            if (collision.CompareTag("Player"))
            {
                var player = collision.GetComponent<MarioPresenter>();

                var powerUpManager = FindObjectOfType<PowerUpManager>();
                
                _hasBeenTriggered = true;

                // Spawn the correct power-up
                powerUpManager.SpawnPowerUp(spawnPoint, player.GetMarioState(), isFireFlowerTrigger);
            }
        }
    }
}
