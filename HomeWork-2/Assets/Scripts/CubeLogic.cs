using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/*
 * И-так, тут будет очень длинный комментарий. Я покумекал и решил, что нужно как-то отличиться (зачем-то)
 * Как итог я решил заняться оптимизацией (концептуально простой и логичной, а на практике проблемной для меня)
 * Дабы не вызывать бесконечно Destroy/Instantiate было принято решение использовать что-то навроде кеширования. 
 * Есть активный список, который состоит из использующихся в сцене кубов. Пример : (c,c,c)
 * Есть список инициализированных кубов, который состоит из списков ранее введенных. Пример : ((с,c,c), (c,c))
 * Первым нашим вводом послужил список из трех кубов, далее из двух. Потом мы снова решили обратиться к трем кубам.
 * Алгоритм прошелся по списку списков, нашел нужный и вернул его как activeCubes.
 * Естественно для кубов это оверкилл, т.к задача уникальности не имеет.
 * Но если бы у нас был условный кортеж из разных объектов, то мне кажется решение было бы хоть чутка обосновано.
 * Чтобы список не разбухал была создана константа с верхней границой. Какое должно быть у нее значение - не знаю.
 * Потому что я тут решаю выдуманные самим собой проблемы забавы ради.
 * Принципы инкапсуляции для сериализованных полей реализовал при помощи свойств.
 * В сеттерах вроде как расписал везде, где можно накосячить при вводе. Плюс проверил, добавлен ли ассет в геттере.
 * Для деактивации кубов принцип таков : элементы взятого из кеша списка кубов делаем активными
 * Остальные элементы списков списка делает не активными. Везде используется проходка циклом
 * Как итог наверное написал шлак, но зато хотя бы интересно было.
 * Касательно математических методов расчета угла
 * Каюсь, попросил нейронку написать мне формулы и чутка отрефакторил написанное, но думаю это не так постыдно.
 */

public class CubeLogic : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float radius;
    [SerializeField] private int cubeQuantity;
    [SerializeField] private CubeMovementType placement;
    [SerializeField] private CircleMovementType direction;
    private List<GameObject> activeCubes;
    private List<List<GameObject>> initializedCubes;
    private const int initThreshhold = 32;
    private float accumulatedAngle = 0f;


    public int CubeQuantity
    {
        get => cubeQuantity;
        set
        {
            if (value < 0)
            {
                Debug.Log("Количество кубов скаляр");
                return;
            }
            cubeQuantity = value;
        }
    }

    public GameObject CubePrefab
    {
        get
        {
            if (cubePrefab == null)
            {
                Debug.Log("Забыли перетащить ассет");
            }
            return cubePrefab;
        }
        set
        {
            if (value == null)
            {
                Debug.Log("Ассет бракован");
                return;
            }
            cubePrefab = value;
        }
    }

    public float Radius
    {
        get => radius;
        set
        {
            if (value < 0)
            {
                Debug.Log("Радиус скаляр");
                return;
            }
            radius = value;
        }
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
        set => rotationSpeed = value;
    }

    void Awake()
    {
        activeCubes = new List<GameObject>();
        initializedCubes = new List<List<GameObject>>();
        ActivateCubes();
    }

    void Update()
    {
        ActivateCubes();
        DeactivateCubes();
        RotateAroundCenter();
        RotateSelf();
    }


    private void ActivateCubes()
    {
        if (initThreshhold < initializedCubes.Count)
        {
            foreach (var listOfCubes in initializedCubes)
            {
                foreach (var cube in listOfCubes)
                    Destroy(cube);  
            }
            initializedCubes.Clear(); //Когда делаем Destroy по границе, то остается куча null'ов, очищаем список.
            activeCubes.Clear(); //Аналогично
        } 

        if (initializedCubes.Count == 0)
        {
            for (int i = 0; i < cubeQuantity; i++)
                activeCubes.Add(Instantiate(cubePrefab));
            initializedCubes.Add(new List<GameObject>(activeCubes));
        }
        else
        {
            activeCubes.Clear();
            var index = initializedCubes.FindIndex(nestList => nestList.Count == cubeQuantity);
            if (index == -1)
            {
                for (int i = 0; i < cubeQuantity; i++)
                    activeCubes.Add(Instantiate(cubePrefab));
                initializedCubes.Add(new List<GameObject>(activeCubes));
            }
            else
                activeCubes = new List<GameObject>(initializedCubes[index]);
        }
    }

    private void DeactivateCubes()
    {
        foreach (var cube in activeCubes)
            cube.SetActive(true);

        foreach (var cubeList in initializedCubes)
        {
            foreach (var cube in cubeList)
            {
                if (!activeCubes.Contains(cube))
                    cube.SetActive(false);
            }
        }
    }

    private void RotateAroundCenter()
    {
        accumulatedAngle += (direction == CircleMovementType.Clockwise ? -1 : 1) * rotationSpeed * Time.deltaTime;
        for (int i = 0; i < activeCubes.Count; i++)
        {
            var theta = placement == CubeMovementType.Even
                ? 2 * Mathf.PI * i / activeCubes.Count
                : Mathf.PI / 6 * i;
            activeCubes[i].transform.position = transform.position + new Vector3(
                radius * Mathf.Cos(theta + accumulatedAngle),
                0,
                radius * Mathf.Sin(theta + accumulatedAngle)
            );
        }
    }

    private void RotateSelf()
    {
        foreach (var cube in activeCubes)
            cube.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
