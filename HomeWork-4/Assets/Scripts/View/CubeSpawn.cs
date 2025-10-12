using System.Collections.Generic;
using UnityEngine;

public class CubeSpawn : MonoBehaviour
{
    [SerializeField] private CubePool cubePool;
    [SerializeField] private CubeModel cubeModel;
    private int cubeQuantity;
    private int activeCubesCount;
    private Vector3 centreOfSpawnArea;
    private float spawnSpacing;
    private List<GameObject> activeCubes;
    private Dictionary<GameObject, (ApplyForces applyForces, IsDiceStatic isDiceStatic)> cubeComponents;

    void Awake()
    {
        activeCubes = new List<GameObject>();
        cubeComponents = new Dictionary<GameObject, (ApplyForces, IsDiceStatic)>();
    }

    private void Start()
    {
        cubeModel.CubeQuantityChanged.AddListener(OnCubeQuantityChanged);
        cubePool.CubeSpawned.AddListener(OnCubeSpawned);
        cubePool.CubeReturned.AddListener(OnCubeReturned);
        activeCubesCount = cubePool.GetActiveCubesCount();
        cubeQuantity = cubeModel.GetCubeQuantity;
        centreOfSpawnArea = Vector3.zero;
        spawnSpacing = 2f;
        SpawnCubes();
    }

    void Update()
    {
        AdjustCubeCount();
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
                Debug.Log("Не удалось получить куб из пула");
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
        var applyForces = cube.GetComponent<ApplyForces>();
        var isDiceStatic = cube.GetComponent<IsDiceStatic>();
        cubeComponents[cube] = (applyForces, isDiceStatic);
    }

    private void OnCubeReturned(GameObject cube)
    {
        activeCubesCount--;
        activeCubes.Remove(cube);
        cubeComponents.Remove(cube);
    }

    private void OnCubeQuantityChanged(int newQuantity)
    {
        cubeQuantity = newQuantity;
        AdjustCubeCount();
    }

    public void PositionCubeInLine(GameObject cube, int indexOfCube)
    {
        var position = new Vector3(
            centreOfSpawnArea.x + indexOfCube * spawnSpacing,
            centreOfSpawnArea.y + 1f,
            centreOfSpawnArea.z
        );
        cube.transform.position = position;
    }

    public List<GameObject> GetActiveCubes => activeCubes;

    public (ApplyForces applyForces, IsDiceStatic isDiceStatic) GetCubeComponents(GameObject cube)
    {
        return cubeComponents[cube];
    }
}
