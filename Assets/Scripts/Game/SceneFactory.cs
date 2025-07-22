namespace Game
{
    /**
     * This class is responsible for managing the game state and transitions between scenes.
     */
    public class SceneFactory
    {
        public enum SceneName
        {
            MainScene,
            MapScene,
            GameOverScene,
            OpeningScene,
            GameWonScene,
            TimeOverScene,
            GameStartScene, // Add more scenes as needed
        }
    
        public static string GetSceneName(SceneName scene)
        {
            switch (scene)
            {
                case SceneName.MainScene: return "MainScene";
                case SceneName.MapScene: return "MapScene";
                case SceneName.GameOverScene: return "GameOverScene";
                case SceneName.OpeningScene: return "OpeningScene";
                case SceneName.GameWonScene: return "GameWonScene";
                case SceneName.TimeOverScene: return "TimeOverScene";
                case SceneName.GameStartScene: return "GameStart";
                default: return string.Empty;
            }
        } 
    }
}