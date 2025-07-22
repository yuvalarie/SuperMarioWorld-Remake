using TMPro;
using UnityEngine;

namespace Game
{
    /**
     * This class is responsible for displaying the game won screen.
     */
    public class GameWonView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI coinHeaderText;
        [SerializeField] private TextMeshProUGUI timeHeaderText;
        [SerializeField] private TextMeshProUGUI healthHeaderText;
        
        private void Start()
        {
            // Get stats from GameManager
            int coins = GameManager.Instance.GetFinalCoins();
            int time = GameManager.Instance.GetFinalTime();
            int lives = GameManager.Instance.GetFinalLives();
            int points = time * coins;
            
            // Update UI
            if (coinsText != null)
                coinsText.text = coins.ToString();
            if (timeText != null)
                timeText.text = time.ToString();
            if (finalScoreText != null)
                finalScoreText.text = points.ToString();
            if (coinHeaderText != null)
                coinHeaderText.text = coins.ToString();
            if (timeHeaderText != null)
                timeHeaderText.text = (300 - time).ToString();
            if (healthHeaderText != null)
                healthHeaderText.text = lives.ToString();
        }
    }
}