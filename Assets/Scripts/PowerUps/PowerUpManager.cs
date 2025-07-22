using Mario.Player;
using UnityEngine;

namespace PowerUps
{
    /**
     * Manages power-ups in the game.
     */
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private GameObject mushroomPrefab;
        [SerializeField] private GameObject fireFlowerPrefab;
        
        public void SpawnPowerUp(Transform spawnPoint, MarioState marioState, bool isFireFlowerTrigger)
        {
            GameObject powerUpToSpawn;

            if (isFireFlowerTrigger)
            {
                // Spawn mushroom if Mario is in regular mode, otherwise spawn fire flower
                powerUpToSpawn = marioState == MarioState.Regular ? mushroomPrefab : fireFlowerPrefab;
            }
            else
            {
                // Always spawn mushroom for mushroom triggers
                powerUpToSpawn = mushroomPrefab;
            }
            Instantiate(powerUpToSpawn, spawnPoint.position, Quaternion.identity);
        }
        
        public static bool ApplyPowerUp(MarioPresenter mario, MarioState powerUpType)
        {
            switch (powerUpType)
            {
                case MarioState.Super when mario.GetMarioState() == MarioState.Regular:
                    mario.MarioModel.ApplyPowerUp(powerUpType);
                    return true;
                case MarioState.Fire when mario.GetMarioState() == MarioState.Super:
                    mario.MarioModel.ApplyPowerUp(powerUpType);
                    return true; 
                default:
                    return false;
            }
        }
    }
}