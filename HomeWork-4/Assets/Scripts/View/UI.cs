using TMPro;
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
        inputMinDraw.onEndEdit.AddListener(OnInputMinDraw);
        inputMinWin.onEndEdit.AddListener(OnInputMinWin);
        inputMinLoss.onEndEdit.AddListener(OnInputMinLoss);
        reRollButton.onClick.AddListener(OnReRollButtonClicked);
        if (rebindButton != null)
            rebindButton.onClick.AddListener(OnRebindButtonClicked);
        UpdateCubeQuantityDisplay(cubeModel.GetCubeQuantity);
        GenerateRandomThresholds();
        UpdateThresholdDisplay();
    }

    private void OnDestroy()
    {
        cubeModel.CubeQuantityChanged.RemoveListener(OnCubeQuantityChanged);
        moveLogic.OnRollPerformed.RemoveListener(OnRollPerformed);
        moveLogic.OnAllCubesStopped.RemoveListener(OnAllCubesStopped);
        inputCubeQuantity.onEndEdit.RemoveListener(OnInputCubeQuantity);
        inputMinDraw.onEndEdit.RemoveListener(OnInputMinDraw);
        inputMinWin.onEndEdit.RemoveListener(OnInputMinWin);
        inputMinLoss.onEndEdit.RemoveListener(OnInputMinLoss);
        reRollButton.onClick.RemoveListener(OnReRollButtonClicked);
        if (rebindButton != null)
            rebindButton.onClick.RemoveListener(OnRebindButtonClicked);
    }

    private void OnRollPerformed()
    {
        scoreText.text = "Бросок...";
    }

    private void OnAllCubesStopped(int totalScore)
    {
        string result = totalScore >= minWin ? "Победа!" :
                        totalScore >= minDraw ? "Ничья" : "Поражение";
        scoreText.text = $"Выпавшее значение: {totalScore} ({result})";
    }

    private void OnCubeQuantityChanged(int quantity)
    {
        UpdateCubeQuantityDisplay(quantity);
        GenerateRandomThresholds();
        UpdateThresholdDisplay();
    }

    private void UpdateCubeQuantityDisplay(int quantity)
    {
        cubeQuantityText.text = $"Число кубиков: {quantity}";
        inputCubeQuantity.text = quantity.ToString();
    }

    private void GenerateRandomThresholds()
    {
        int maxScore = cubeModel.GetCubeQuantity * 6;
        minLoss = Random.Range(1, maxScore / 3);
        minDraw = Random.Range(minLoss + 1, maxScore * 2 / 3);
        minWin = Random.Range(minDraw + 1, maxScore);
    }

    private void UpdateThresholdDisplay()
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
                Debug.Log("Число кубиков должно быть от 2 до 20");
                inputCubeQuantity.text = cubeModel.GetCubeQuantity.ToString();
                return;
            }
            cubeModel.GetCubeQuantity = newValue;
        }
        else
        {
            Debug.Log("Некорректное число кубиков");
            inputCubeQuantity.text = cubeModel.GetCubeQuantity.ToString();
        }
    }

    private void OnInputMinDraw(string value)
    {
        if (int.TryParse(value, out int newValue))
        {
            if (newValue <= minLoss)
            {
                inputMinDraw.text = minDraw.ToString();
                return;
            }
            if (newValue >= minWin)
            {
                inputMinDraw.text = minDraw.ToString();
                return;
            }
            if (newValue < 0)
            {
                inputMinDraw.text = minDraw.ToString();
                return;
            }
            minDraw = newValue;
            UpdateThresholdDisplay();
        }
        else
        {
            Debug.Log("Некорректное значение для мин. ничьей");
            inputMinDraw.text = minDraw.ToString();
        }
    }

    private void OnInputMinWin(string value)
    {
        if (int.TryParse(value, out int newValue))
        {
            if (newValue <= minDraw)
            {
                Debug.Log("Мин. для победы должно быть больше мин. для ничьей");
                inputMinWin.text = minWin.ToString();
                return;
            }
            if (newValue < 0)
            {
                Debug.Log("Мин. для победы не может быть отрицательным");
                inputMinWin.text = minWin.ToString();
                return;
            }
            minWin = newValue;
            UpdateThresholdDisplay();
        }
        else
        {
            Debug.Log("Некорректное значение для мин. победы");
            inputMinWin.text = minWin.ToString();
        }
    }

    private void OnInputMinLoss(string value)
    {
        if (int.TryParse(value, out int newValue))
        {
            if (newValue >= minDraw)
            {
                Debug.Log("Мин. для поражения должно быть меньше мин. для ничьей");
                inputMinLoss.text = minLoss.ToString();
                return;
            }
            if (newValue < 0)
            {
                Debug.Log("Мин. для поражения не может быть отрицательным");
                inputMinLoss.text = minLoss.ToString();
                return;
            }
            minLoss = newValue;
            UpdateThresholdDisplay();
        }
        else
        {
            Debug.Log("Некорректное значение для мин. поражения");
            inputMinLoss.text = minLoss.ToString();
        }
    }

    private void OnReRollButtonClicked()
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