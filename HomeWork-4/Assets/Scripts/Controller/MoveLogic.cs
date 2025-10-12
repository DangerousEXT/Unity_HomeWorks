using UnityEngine;
using UnityEngine.InputSystem;

public class MoveLogic : MonoBehaviour
{
    [SerializeField] private ApplyForces applyForces;
    [SerializeField] private IsDiceBottom isBottom;
    [SerializeField] private IsDiceStatic isStatic;

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (isBottom.IsBottom && isStatic.AtRest)
        {
            applyForces.ThrowCube();
        }
    }
}
