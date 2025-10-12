using UnityEngine;
using UnityEngine.InputSystem;

public class MoveLogic : MonoBehaviour
{
    [SerializeField] private ApplyForces applyForces;
    [SerializeField] private IsDiceBottom isBottom;
    [SerializeField] private IsDiceStatic isStatic;
    [SerializeField] private CubeSpawn cubeSpawn;
    [SerializeField] private FindUpperSide upperSide;

    public void Roll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        else
            applyForces.ThrowCube();
    }

    public int GetTotalScore()
    {
        var total = 0;
        foreach (var cube in cubeSpawn.GetActiveCubes)
        {
            if (isStatic && isBottom)
            {
                var value = upperSide.GetUpperSide(cube.transform);
                total += value;
                Debug.Log($"[DiceGameController] {cube.name}: {value}");
            }
        }
        Debug.Log($"[DiceGameController] Общий счёт: {total}");
        return total;
    }
}
