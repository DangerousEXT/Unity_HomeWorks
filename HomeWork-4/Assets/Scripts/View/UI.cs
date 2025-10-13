using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("Display Texts")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text cubeQuantityText;
    [SerializeField] private TMP_Text minDrawText;
    [SerializeField] private TMP_Text minWinText;
    [SerializeField] private TMP_Text minLossText;

    [Header("Input Fields")]
    [SerializeField] private TMP_InputField inputCubeQuantity;
    [SerializeField] private TMP_InputField inputMinDraw;
    [SerializeField] private TMP_InputField inputMinWin;
    [SerializeField] private TMP_InputField inputMinLoss;

    [Header("Buttons")]
    [SerializeField] private Button reRollButton;
    [SerializeField] private Button rebindButton;

    [Header("References")]
    [SerializeField] private CubeModel cubeModel;
    [SerializeField] private MoveLogic moveLogic;
    [SerializeField] private CubeSpawn cubeSpawn;
    [SerializeField] private BindKey bindKey;

    private int minDraw;
    private int minWin;
    private int minLoss;

    private void Awake()
    {
        cubeModel.CubeQuantityChanged.AddListener(OnCubeQuantityChanged);
        moveLogic.OnRollPerformed.AddListener(OnRollPerformed);
        moveLogic.OnAllCubesStopped.AddListener(OnAllCubesStopped);
        inputCubeQuantity.onEndEdit.AddListener(OnInputCubeQuantity);
        inputMinDraw.onEndEdit.AddListener(value => OnInputWinType(WinType.Draw, value));
        inputMinWin.onEndEdit.AddListener(value => OnInputWinType(WinType.Win, value));
        inputMinLoss.onEndEdit.AddListener(value => OnInputWinType(WinType.Lose, value));
        reRollButton.onClick.AddListener(OnReRollButtonClicked);
        rebindButton.onClick.AddListener(OnRebindButtonClicked);
        UpdateCubeQuantityDisplay(cubeModel.GetCubeQuantity);
        GenerateRandomThresholds();
        UpdateThresholdDisplay();
    }

    private void OnRollPerformed()
    {
        scoreText.text = "Бросок...";
    }

    private void OnAllCubesStopped(int totalScore)
    {
        var result = totalScore >= minWin ? "Победа!" :
                        totalScore >= minDraw ? "Ничья" : "Поражение";
        scoreText.text = $"Выпавшее значение: {totalScore} ({result})";
    }

    private void OnCubeQuantityChanged(int quantity)
    {
        UpdateCubeQuantityDisplay(quantity);
        GenerateRandomThresholds();
        UpdateThresholdDisplay();
    }

    private void UpdateCubeQuantityDisplay(int quantity) //Вынести в контрол часть
    {
        cubeQuantityText.text = $"Число кубиков: {quantity}";
        inputCubeQuantity.text = quantity.ToString();
    }

    private void GenerateRandomThresholds() //Вынести в контрол часть
    {
        var maxScore = cubeModel.GetCubeQuantity * 6;
        minLoss = Random.Range(1, maxScore / 3);
        minDraw = Random.Range(minLoss + 1, maxScore * 2 / 3);
        minWin = Random.Range(minDraw + 1, maxScore);
    }

    private void UpdateThresholdDisplay() //Вынести в контрол часть
    {
        minLossText.text = $"Мин. для поражения: {minLoss}";
        minDrawText.text = $"Мин. для ничьей: {minDraw}";
        minWinText.text = $"Мин. для победы: {minWin}";
        inputMinLoss.text = minLoss.ToString();
        inputMinDraw.text = minDraw.ToString();
        inputMinWin.text = minWin.ToString();
    }

    private void OnInputCubeQuantity(string value)
    {
        if (int.TryParse(value, out int newValue))
        {
            if (newValue < 2 || newValue > 20)
            {
                inputCubeQuantity.text = cubeModel.GetCubeQuantity.ToString();
                return;
            }
            cubeModel.GetCubeQuantity = newValue;
        }
        else
            inputCubeQuantity.text = cubeModel.GetCubeQuantity.ToString();
    }

    private void OnInputWinType(WinType type, string value)
    {
        int.TryParse(value, out var newValue);
        switch (type)
        {
            case WinType.Lose:
                if (newValue >= minDraw)
                {
                    inputMinLoss.text = minLoss.ToString();
                    return;
                }
                minLoss = newValue;
                break;
            case WinType.Draw:
                if (newValue <= minLoss || newValue >= minWin)
                {
                    inputMinDraw.text = minDraw.ToString();
                    return;
                }
                minDraw = newValue;
                break;
            case WinType.Win:
                if (newValue <= minDraw)
                {
                    inputMinWin.text = minWin.ToString();
                    return;
                }
                minWin = newValue;
                break;
        }
        UpdateThresholdDisplay();
    }

    private void OnReRollButtonClicked() //повторяет логику спавнера, нужен рефактор в будущем
    {
        for (int i = 0; i < cubeSpawn.GetActiveCubes.Count; i++)
        {
            var cube = cubeSpawn.GetActiveCubes[i];
            var position = new Vector3(2f * i,0f,0f);
            cube.transform.position = position;
            cube.transform.rotation = Quaternion.identity;
            var rb = cube.GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        GenerateRandomThresholds();
        UpdateThresholdDisplay();
        moveLogic.Roll(new InputAction.CallbackContext());
    }

    private void OnRebindButtonClicked()
    {
        if (bindKey != null)
            bindKey.OnRebind();
    }
}