using System.Collections.Generic;
using System;
using UnityEngine;
/*
 * Проблема : destroy / instantiate очень затратные операции, в update напостоянку вызывать бо-бо + GC даст прикурить.
 * Решение : Object pool (в юнити есть built-in, но стоит реализовать самому)
 * Объяснение : Есть некоторый пул (контейнер) с неактивными объектами. Достаем объект - делаем его активным, используем - профит.
 * Он нам не нужен - делаем его не активным, убираем обратно в пул и радуемся жизни.
 * Если объектов не хватает, то создаем дополнительные.
 * 
 * Методы (методичка юнити, версия "пока не смешарик". Написал как могу =( )
 * 1) GetCube - получаем куб, либо вызываем метод расширения
 * 2) ReturnCube - возвращаем куб в пул
 * 3*) ExpandPool - расширяем пул новым неактивным объектом
 * 4*) GetActiveCount - получили активные кубы. Проверка на необходимость доинициализации включена
 * Дополнение : алгоритм до смешного простой, но мои кривые руки + nullReference отовсюду...
 * Как итог : лучше бы я сделал обычный спавнер =0
 * Ввел события для отслеживания количества кубов, может будет полезно в будущем
 * Плюс не придется создавать списки вспомогательные
 */
public class CubePool : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private int maximumElementsInPool;
    private Stack<GameObject> cubePool;
    private int poolSize;
    public event Action<GameObject> CubeSpawned;
    public event Action<GameObject> CubeReturned;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        poolSize = 10;
        cubePool = new Stack<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewCube();
        }
    }

    public GameObject GetCube()
    {
        if (cubePool.Count == 0)
        {
            if (poolSize < maximumElementsInPool)
                ExpandPool();
            else
            {
                Debug.Log("Пул переполнен!");
                return null;
            }
        }
        var cube = cubePool.Pop();
        cube.SetActive(true);
        CubeSpawned?.Invoke(cube);
        return cube;
    }

    public void ReturnCube(GameObject cube)
    {
        if (cube == null)
        {
            Debug.Log("Возвращается null в пул -_-");
            return;
        }
        cubePool.Push(cube);
        cube.SetActive(false);
        CubeReturned?.Invoke(cube);
    }

    private void ExpandPool()
    {
        CreateNewCube();
        poolSize++;
    }

    private void CreateNewCube()
    {
        var newCube = Instantiate(cubePrefab, transform);
        cubePool.Push(newCube);
        newCube.SetActive(false);
    }
}
