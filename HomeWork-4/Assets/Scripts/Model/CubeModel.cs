using System;
using UnityEngine;
using UnityEngine.Events;
/*
 * По логике тут должно быть следующее:
 * 1) Все параметры куба (скорость, вращение, position, cubeQuantity...)
 * 2) Т.к это все приватное, то обернуть в гет-сеттеры
 * 3) Написать event'ы, на которые подпишутся вьюшки
 */

public class CubeModel : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [Range(4, 16)]
    [SerializeField] private int cubeQuantity;
    [Range(0f, 15f)]
    [SerializeField] private float cubeTorque;
    [Range(0f, 15f)]
    [SerializeField] private float cubeForce;

    public event Action<int> CubeQuantityChanged;
    public event Action<float> CubeTorqueChanged;
    public event Action<float> CubeForceChanged;

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
                cubeQuantity = value;
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
                cubeTorque = value;
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
                cubeForce = value;
                CubeForceChanged?.Invoke(cubeForce);
            }
        }
    }
}

