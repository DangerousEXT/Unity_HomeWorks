using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BindKey : MonoBehaviour
{
    [SerializeField] private InputActionReference rollActionReference;
    [SerializeField] private TMP_Text bindingDisplayText;

    private InputAction rollAction;

    private void Awake()
    {
        if (rollActionReference != null)
            rollAction = rollActionReference.action;
        UpdateDisplay();
    }

    public void StartRebind()
    {
        if (rollAction == null) return;

        rollAction.Disable();
        rollAction.PerformInteractiveRebinding()
            .OnMatchWaitForAnother(0.2f)
            .OnComplete(op =>
            {
                rollAction.Enable();
                UpdateDisplay();
                op.Dispose();
            })
            .Start();
    }

    public void OnRebind(InputAction.CallbackContext context)
    {
        if (context.performed) 
            StartRebind();
    }

    private void UpdateDisplay()
    {
        if (bindingDisplayText != null && rollAction != null)
            bindingDisplayText.text = rollAction.GetBindingDisplayString();
    }
}
