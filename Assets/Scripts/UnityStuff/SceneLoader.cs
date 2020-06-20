using Scripts.GameLogic;
using UnityEngine.SceneManagement;

namespace Scripts.UnityStuff
{
    public class SceneLoader
    {
        public static string username;
        public static string oponentName;
        public static PlayerId side;


        public static void LoadWinScreen()
        {
            SceneManager.LoadScene("Win Screen");
        }

        public static void LoadNewGame(string oponentName, PlayerId side)
        {
            SceneLoader.oponentName = oponentName;
            SceneLoader.side = side;
            SceneManager.LoadScene("Board");
        }

        public static void LoadMainMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
