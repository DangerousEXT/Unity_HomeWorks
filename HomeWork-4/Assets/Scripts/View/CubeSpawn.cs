using System.Collections.Generic;
using UnityEngine;

/*
 * Написать комм
 * 
 * 
 * 
 * 
 */

public class CubeSpawn : MonoBehaviour
{
    [SerializeField] private CubePool cubePool;
    [SerializeField] private CubeModel cubeModel;
    private int cubeQuantity;
    private int activeCubesCount;
    private Vector3 centreOfSpawnArea;
    private float spawnSpacing;
    private List<GameObject> activeCubes;


    void Awake()
    {
        activeCubes = new List<GameObject>();
        activeCubesCount = cubePool.GetActiveCubesCount();
        cubeModel.CubeQuantityChanged += OnCubeQuantityChanged;
        cubePool.CubeSpawned += OnCubeSpawned;
        cubePool.CubeReturned += OnCubeReturned;        
        cubeQuantity = cubeModel.GetCubeQuantity;
        centreOfSpawnArea = Vector3.zero;
        spawnSpacing = 2f;
        SpawnCubes();
    }

    void Update()
    {
        AdjustCubeCount();
    }

    private void OnDestroy()
    {
        cubeModel.CubeQuantityChanged -= OnCubeQuantityChanged;
        cubePool.CubeReturned -= OnCubeReturned;
        cubePool.CubeSpawned -= OnCubeSpawned;
    }

    private void AdjustCubeCount()
    {
        if (cubeQuantity > activeCubesCount)
            SpawnCubes();
        else if (cubeQuantity < activeCubesCount)
            DespawnExcessCubes();    
    }


    private void SpawnCubes()
    {
        while (activeCubesCount < cubeQuantity)
        {
            var cube = cubePool.GetCube();
            if (cube == null)
            {
                Debug.Log("из пула подтянулся null");
                return;
            }
            PositionCubeInLine(cube, activeCubesCount);
        }
    }


    private void DespawnExcessCubes()
    {
        while (activeCubesCount > cubeQuantity)
        {
            var cube = activeCubes[activeCubes.Count - 1];
            cubePool.ReturnCube(cube);
        }
    }


    private void OnCubeSpawned(GameObject cube)
    {
        activeCubesCount++;
        activeCubes.Add(cube);
    }


    private void OnCubeReturned(GameObject cube)
    {
        activeCubesCount--;
        activeCubes.Remove(cube);
    }

    private void OnCubeQuantityChanged(int newQuantity)
    {
        cubeQuantity = newQuantity;
        AdjustCubeCount();
    }


    private void PositionCubeInLine(GameObject cube, int indexOfCube)
    {
        var position = new Vector3(
            centreOfSpawnArea.x + indexOfCube * spawnSpacing,
            centreOfSpawnArea.y + 1f,
            centreOfSpawnArea.z
        );
        cube.transform.position = position;
    }

    public List<GameObject> GetActiveCubes => activeCubes;
}
