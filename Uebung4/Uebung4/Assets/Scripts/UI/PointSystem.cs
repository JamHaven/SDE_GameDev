using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PointSystem : MonoBehaviour
    {

        public Text pointDisplay; // Displays the current points (startingPoints)
        public int startingPoints = 0; //We start with 0 points
        public int pointsToWin = 3; //How much points until the player wins
        public Text centerTextBox; //Textfield

        private bool m_isGameWon = false;

        // Update is called once per frame
        void Update()
        {
            pointDisplay.text = startingPoints.ToString();
            if (startingPoints >= pointsToWin) //If we have enough points to win
            {
                centerTextBox.text = "You won! Press \"r\" to play again!";
                centerTextBox.enabled = true;
                m_isGameWon = true;
                //Time.timeScale = 0f;
            }
        }

        /**
         * Rewards points, used by enemies
         */
        public void RewardPoints(int points)
        {
            startingPoints += points;
        }

        /**
         * Loosing points. Not used but maybe used in future iterrations
         */
        public void LoosePoints(int points)
        {
            startingPoints -= points;
        }

        /**
         * Lets other components check if we won
         */
        public bool IsGameWon()
        {
            return m_isGameWon;
        }
    }
}
