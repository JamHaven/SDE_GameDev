using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PointSystem : MonoBehaviour
    {

        public Text pointDisplay;
        public int startingPoints = 0;
        public int pointsToWin = 3;
        public Text centerTextBox;

        private bool m_isGameWon = false;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            pointDisplay.text = startingPoints.ToString();
            if (startingPoints >= pointsToWin)
            {
                centerTextBox.text = "You won! Press \"r\" to play again!";
                centerTextBox.enabled = true;
                m_isGameWon = true;
                //Time.timeScale = 0f;
            }
        }

        public void RewardPoints(int points)
        {
            startingPoints += points;
        }

        public void LoosePoints(int points)
        {
            startingPoints -= points;
        }

        public bool IsGameWon()
        {
            return m_isGameWon;
        }
    }
}
