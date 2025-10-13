using UnityEngine;

public class FindTotalScore : MonoBehaviour
{
    [SerializeField] private CubeSpawn cubeSpawn;
    [SerializeField] private FindUpperSide upperSide;
    public int GetTotalScore()
    {
        var totalScore = 0;
        foreach (var cube in cubeSpawn.GetActiveCubes)
        {
            var staticCheck = cube.GetComponent<IsDiceStatic>(); // -_-. Вроде бы не так критично, т.к обложено в fixUpd проверкой
            if (staticCheck.AtRest)
            {
                var value = upperSide.GetUpperSide(cube.transform);
                totalScore += value;
            }
        }
        return totalScore;
    }
}
