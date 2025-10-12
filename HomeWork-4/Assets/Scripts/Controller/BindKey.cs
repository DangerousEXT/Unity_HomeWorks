using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class BindKey : MonoBehaviour
{
    [SerializeField] private TMP_Text bindingPath;
    private PlayerInput playerInput;
    private InputAction action;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        action = playerInput.actions["Roll"];
    }

    public void OnRebind()
    {
        action.Disable();
        action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                action.Enable();
                operation.Dispose();
            })
            .Start();
    }
}
