using UnityEngine;
/*
 * В инспекторе создал пол (плоскость) и поверх него накинул триггер (другая плоскость)
 * OnTriggerStay расписывать излишне (пока что как минимум =) ) т.к нет какого-то уникального действия после падения
 * Проверяю "вход" по созданному в инспекторе тегу.
 */

public class IsDiceBottom : MonoBehaviour
{
    public bool IsBottom { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Dice"))
        {
            IsBottom = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Dice"))
        {
            IsBottom = false;
        }
    }
}

