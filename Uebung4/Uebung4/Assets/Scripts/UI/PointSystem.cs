using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PointSystem : MonoBehaviour
    {

        public Text pointDisplay;
        public int startingPoints = 0;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            pointDisplay.text = startingPoints.ToString();
        }

        public void RewardPoints(int points)
        {
            startingPoints += points;
        }

        public void LoosePoints(int points)
        {
            startingPoints -= points;
        }
    }
}
