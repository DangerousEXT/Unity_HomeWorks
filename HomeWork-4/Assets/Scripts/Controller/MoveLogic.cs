using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MoveLogic : MonoBehaviour
{
    [SerializeField] private CubeSpawn cubeSpawn;
    [SerializeField] private FindTotalScore computedScore;
    public UnityEvent OnRollPerformed;
    public UnityEvent<int> OnAllCubesStopped;

    public void Roll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        RollAllDice();
        OnRollPerformed?.Invoke();
    }

    private void FixedUpdate()
    {
        if (AllCubesStopped())
        {
            var totalScore = computedScore.GetTotalScore();
            OnAllCubesStopped?.Invoke(totalScore);
        }
    }

    private void RollAllDice()
    {
        foreach (var cube in cubeSpawn.GetActiveCubes)
        {
            var (applyForces, _) = cubeSpawn.GetCubeComponents(cube);
            applyForces.ThrowCube();
        }
    }

    private bool AllCubesStopped()
    {
        foreach (var cube in cubeSpawn.GetActiveCubes)
        {
            var (_, isDiceStatic) = cubeSpawn.GetCubeComponents(cube);
            if (!isDiceStatic.AtRest)
            {
                return false;
            }
        }
        return true;
    }
}
