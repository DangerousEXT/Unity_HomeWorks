using UnityEngine;

//В каждом случае ForceMode.Impulse для постепенного затухания силы вращения (ий) кубика(ов)
//Если бы применили ForceMode.Force, то было бы мгновенное применение силы на 1 кадр.
public class CubeThrow : MonoBehaviour
{
    [Range(0,15)]
    [SerializeField] private float appliedTorque;
    [Range(0,15)]
    [SerializeField] private float appliedForce;
    private Rigidbody rigidBody;
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    public void ThrowCube()
    {
        ApplyRandomTorque(appliedTorque);
        ApplyRandomForce(appliedForce);
    }

    private void ApplyRandomTorque(float torque)
    {
        var torqueDirection = new Vector3(Random.Range(-torque, torque),
            Random.Range(-torque, torque),
            Random.Range(-torque, torque));
        rigidBody.AddTorque(torqueDirection, ForceMode.Impulse);
    }
    
    private void ApplyRandomForce(float force)
    {
        var rollDirection = new Vector3(Random.Range(-appliedForce, appliedForce),
            Random.Range(-appliedForce, appliedForce),
            Random.Range(-appliedForce, appliedForce));
        rigidBody.AddForce(rollDirection, ForceMode.Impulse);
    }
}
