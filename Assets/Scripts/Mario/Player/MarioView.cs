using TMPro;
using UnityEngine;

namespace Mario.Player
{
    /**
     * The view for the player.
     */
    public class MarioView : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI timeText;

        public void UpdateHealth(int health)
        {
            if (healthText != null)
                healthText.text = health.ToString();
        }

        public void UpdateCoins(int points)
        {
            if (coinsText != null)
                coinsText.text = points.ToString();
        }

        public void UpdateTimer(int time)
        {
            if(timeText != null)
                timeText.text = time.ToString();
        }
    }
}