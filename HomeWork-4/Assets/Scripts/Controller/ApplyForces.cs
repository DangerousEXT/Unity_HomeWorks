using UnityEngine;

public class ApplyForces : MonoBehaviour
{
    private CubeModel cubeModel;
    private Rigidbody rb;
    private float cubeTorque;
    private float cubeForce;

    public void Init(CubeModel model)
    {
        cubeModel = model;
        rb = GetComponent<Rigidbody>();
        cubeTorque = cubeModel.GetCubeTorque;
        cubeForce = cubeModel.GetCubeForce;
        cubeModel.CubeTorqueChanged.AddListener(OnCubeTorqueChanged);
        cubeModel.CubeForceChanged.AddListener(OnCubeForceChanged);
    }

    public void ThrowCube()
    {
        Vector3 torqueDir = new Vector3(
            Random.Range(-cubeTorque, cubeTorque),
            Random.Range(-cubeTorque, cubeTorque),
            Random.Range(-cubeTorque, cubeTorque)
        );
        rb.AddTorque(torqueDir, ForceMode.Impulse);

        Vector3 forceDir = new Vector3(
            Random.Range(-cubeForce, cubeForce),
            Random.Range(cubeForce, cubeForce),
            Random.Range(-cubeForce, cubeForce)
        );
        rb.AddForce(forceDir, ForceMode.Impulse);
    }

    private void OnCubeTorqueChanged(float newTorque) => cubeTorque = newTorque;
    private void OnCubeForceChanged(float newForce) => cubeForce = newForce;
}