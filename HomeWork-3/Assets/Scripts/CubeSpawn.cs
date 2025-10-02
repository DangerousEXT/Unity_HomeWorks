using System.Collections.Generic;
using UnityEngine;

/*
 * Кубы спавнятся в линейку с одинаковым промежутком начиная с нулевого вектора
 * Дабы не вводить вспомогательный список с активными кубами сделал иерархию по трансформу
 * И убирал лишних дочек спавнера по индексам. Без геткомпонента т.к проверка идет в апдейт постоянно
 * Надеюсь неплохо получилось
 */

public class CubeSpawn : MonoBehaviour
{
    [SerializeField] [Range(4,16)] private int cubeQuantity;
    [SerializeField] private CubePool cubePool;
    private Vector3 centreOfSpawnArea;
    private readonly float spawnSpacing = 2f;
    private int lastCubeQuantity;
    private int activeCubesCount;


    public void Awake()
    {
        cubePool.CubeSpawned += OnCubeSpawned;
        cubePool.CubeReturned += OnCubeReturned;
        centreOfSpawnArea = Vector3.zero;
        lastCubeQuantity = cubeQuantity;
        SpawnCubes();
    }


    private void Update()
    {
        if (cubeQuantity != lastCubeQuantity)
        {
            AdjustCubeCount();
            lastCubeQuantity = cubeQuantity;
        }
    }
    

    private void OnDestroy()
    {
        cubePool.CubeSpawned -= OnCubeSpawned;
        cubePool.CubeReturned -= OnCubeReturned;
    }


    private void AdjustCubeCount()
    {
        if (cubeQuantity > activeCubesCount)
        {
            SpawnCubes();
        }
        else if (cubeQuantity < activeCubesCount)
        {
            DespawnExcessCubes();
        }
    }


    private void SpawnCubes()
    {
        var cubeNumber = 0;
        while (activeCubesCount < cubeQuantity)
        {
            var cube = cubePool.GetCube();
            cubeNumber++;
            if (cube == null) 
                break;
            PositionCubeInLine(cube, cubeNumber);
        }
    }


    private void DespawnExcessCubes()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            if (activeCubesCount > cubeQuantity)
            {
                var child = transform.GetChild(i);
                if (child.gameObject.activeInHierarchy)
                    cubePool.ReturnCube(child.gameObject);
            }
        }
    }


    private void OnCubeSpawned(GameObject cube)
    {
        activeCubesCount++;
    }


    private void OnCubeReturned(GameObject cube)
    {
        activeCubesCount--;
    }


    private void PositionCubeInLine(GameObject cube, int indexOfCube)
    {
        var position = new Vector3(
            centreOfSpawnArea.x + indexOfCube * spawnSpacing,
            centreOfSpawnArea.y + 1f,
            centreOfSpawnArea.z
        );
        cube.transform.SetParent(transform);
        cube.transform.position = position;
    }
}
