using UnityEngine;

//В каждом случае ForceMode.Impulse для постепенного затухания силы вращения (ий) кубика(ов)
//Если бы применили ForceMode.Force, то было бы мгновенное применение силы на 1 кадр.

public class ApplyForces : MonoBehaviour
{
    [SerializeField] private CubeModel cubeModel;
    private Rigidbody rigidBody;
    private float cubeTorque;
    private float cubeForce;
    void Awake()
    {
        cubeTorque = cubeModel.GetCubeTorque;
        cubeForce = cubeModel.GetCubeForce;
        cubeModel.CubeTorqueChanged += OnCubeTorqueChanged;
        cubeModel.CubeForceChanged += OnCubeForceChanged;
        rigidBody = GetComponent<Rigidbody>();
    }
    private void OnDestroy()
    {
        cubeModel.CubeTorqueChanged -= OnCubeTorqueChanged;
        cubeModel.CubeForceChanged -= OnCubeForceChanged;
    }

    public void ThrowCube()
    {
        OnCubeTorqueChanged(cubeTorque);
        OnCubeForceChanged(cubeForce);
    }

    private void OnCubeTorqueChanged(float torque)
    {
        var torqueDirection = new Vector3(Random.Range(-torque, torque),
            Random.Range(-torque, torque),
            Random.Range(-torque, torque));
        rigidBody.AddTorque(torqueDirection, ForceMode.Impulse);
    }


    private void OnCubeForceChanged(float force)
    {
        var rollDirection = new Vector3(Random.Range(-force, force),
            Random.Range(-force, force),
            Random.Range(-force, force));
        rigidBody.AddForce(rollDirection, ForceMode.Impulse);
    }
}
