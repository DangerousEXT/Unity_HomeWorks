using System.Collections;
using UnityEngine;

/*
 * Проверяем, остановился ли кубик путем сравнения с низкой погрешностью.
 * Не забываем про хелпер IsBottom, чтоб не проверять логику, когда кубы в воздухе вне триггера
 * В корутине ждем, пока куб не войдет в триггер, далее пока он в триггере - обрабатываем его скорость
 * Результат проверки возвращаю свойством AtRest.
 */
public class IsDiceStatic : MonoBehaviour
{
    public bool AtRest { get; private set;}
    private const float velocityThreshold = 0.01f;
    private Rigidbody rb;
    [SerializeField] private IsDiceBottom bottom;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(CheckIfDiceStatic());
    }

    IEnumerator CheckIfDiceStatic()
    {
        while (true)
        {
            yield return new WaitUntil(() => bottom.IsBottom);
            while (bottom.IsBottom)
            {
                AtRest = rb.linearVelocity.magnitude < velocityThreshold &&
                         rb.angularVelocity.magnitude < velocityThreshold;
                yield return new WaitForFixedUpdate();
            }
            AtRest = false;
        }
    }
}
