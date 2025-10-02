using TMPro;
using UnityEngine;

public class ManageScore : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    public int TotalScore { get; private set; }
    public void IncreaseTotalScore(int increment)
    {
        TotalScore += increment;
        scoreText.text = TotalScore.ToString();
    }

    public void ClearScore()
    {
        TotalScore = 0;
    }
}
