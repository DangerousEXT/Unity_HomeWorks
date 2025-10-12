using UnityEngine;

public class FindTotalScore : MonoBehaviour
{
    [SerializeField] private CubeSpawn cubeSpawn;
    [SerializeField] private FindUpperSide upperSide;
    public int GetTotalScore()
    {
        var computedScore = 0;
        foreach (var cube in cubeSpawn.GetActiveCubes)
        {
            var staticCheck = cube.GetComponent<IsDiceStatic>();
            if (staticCheck.AtRest)
            {
                int value = upperSide.GetUpperSide(cube.transform);
                computedScore += value;
            }
        }
        return computedScore;
    }
}
