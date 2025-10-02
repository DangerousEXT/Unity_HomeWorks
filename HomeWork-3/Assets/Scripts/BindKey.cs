using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
public class BindKey : MonoBehaviour
{
    [SerializeField] private TMP_Text bindingPath;
    private PlayerInput playerInput;
    private InputAction action;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        action = playerInput.actions["Throw"];
        bindingPath.text = action.bindings[0].effectivePath;
    }

    public void OnRebind(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        StartRebind();
    }

    public void StartRebind() //Рекомендовали на практике
    {
        bindingPath.text = "Нажми любую кнопку";
        action.Disable();
        action.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(operation =>
            {
                bindingPath.text = action.bindings[0].effectivePath;
                action.Enable();
                operation.Dispose();
            })
            .Start();
    }
}
