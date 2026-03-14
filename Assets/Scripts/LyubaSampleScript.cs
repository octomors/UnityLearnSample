using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LyubaSampleScript : SampleScript
{
    [SerializeField] 
    private float speed = 1f; //Скорость перемещения (единиц в секунду)
    [SerializeField] 
    private Vector3 targetPoint; //Целевая точка, куда нужно переместиться

    private bool isMoving = false; // Флаг, предотвращающий повторный запуск движения

    public override void Use()
    {
        // Запускаем корутину только если объект ещё не движется
        if (!isMoving)
        {
            StartCoroutine(MoveCoroutine());
        }
    }

    // Корутина, отвечающая за плавное перемещение
    private IEnumerator MoveCoroutine()
    {
        isMoving = true;

        // Пока расстояние до цели больше заданной точности (0.001f)
        while (Vector3.Distance(transform.position, targetPoint) > 0.001f)
        {
            // Перемещаем объект по направлению к цели с постоянной скоростью
            transform.position = Vector3.MoveTowards(
                transform.position, 
                targetPoint, 
                speed * Time.deltaTime
            );

            // Ждём следующего кадра
            yield return null;
        }

        // Гарантированно устанавливаем финальную позицию (убираем возможную погрешность)
        transform.position = targetPoint;
        isMoving = false;
    }
}
