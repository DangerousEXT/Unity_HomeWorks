using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ThrowController : MonoBehaviour
{
    [SerializeField] private CubeSpawn cubeSpawn;
    private PlayerInput playerInput;
    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    public void OnThrow(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            for (int i = 0; i < cubeSpawn.transform.childCount; i++)
            {
                var cube = cubeSpawn.transform.GetChild(i).gameObject;
                if (cube.activeInHierarchy)
                {
                    cube.GetComponent<CubeLogic>().OnThrow();
                }
            }
        }
    }
}
