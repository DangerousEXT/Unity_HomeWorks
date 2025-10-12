using System;
using UnityEngine;
using UnityEngine.Events;
/*
 * По логике тут должно быть следующее:
 * 1) Все параметры куба (скорость, вращение, position, cubeQuantity...)
 * 2) Т.к это все приватное, то обернуть в гет-сеттеры
 * 3) Написать event'ы, на которые подпишутся вьюшки
 * 4) Подразумевается, что кубов от 2 до 20, а физ.величины от 1 до 15
 */

public class CubeModel : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    private int cubeQuantity = 4;
    private float cubeTorque = 2f;
    private float cubeForce = 4f;

    public UnityEvent<int> CubeQuantityChanged;
    public UnityEvent<float> CubeTorqueChanged;
    public UnityEvent<float> CubeForceChanged;

    public GameObject GetCubePrefab()
    {
        if (cubePrefab == null)
        {
            Debug.Log("Забыли присвоить префаб модели");
            return null;
        }
        return cubePrefab;
    }


    public int GetCubeQuantity
    {
        get => cubeQuantity;
        set
        {
            if (cubeQuantity != value)
            {
                cubeQuantity = Mathf.Clamp(value, 2, 20);
                CubeQuantityChanged?.Invoke(cubeQuantity);
            }
        }
    }

    public float GetCubeTorque
    {
        get => cubeTorque;
        set
        {
            if (cubeTorque != value)
            {
                cubeTorque = Mathf.Clamp(value, 0f, 15f);
                CubeTorqueChanged?.Invoke(cubeTorque);
            }
        }
    }

    public float GetCubeForce
    {
        get => cubeForce;
        set
        {
            if (cubeForce != value)
            {
                cubeForce = Mathf.Clamp(value, 0f, 15f);
                CubeForceChanged?.Invoke(cubeForce);
            }
        }
    }
}

