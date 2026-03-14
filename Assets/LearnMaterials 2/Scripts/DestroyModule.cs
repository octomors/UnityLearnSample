using System.Collections;
using UnityEngine;

[HelpURL("https://docs.google.com/document/d/1RMamVxE-yUpSfsPD_dEa4-Ak1qu6NTo83qY1O4XLxUY/edit?usp=sharing")]
public class DestroyModule : MonoBehaviour
{
    [SerializeField] //Отображает поле в инспекторе юнити
    [Min(0)]
    private float destroyDelay; //Задержка (в секундах) между уничтожением очередного дочернего обьекта
    [SerializeField]
    [Min(0)]
    private int minimalDestroyingObjectsCount; //Минимальное количество дочерних обьектов, которое должно остаться. После достижения этого числа процесс уничтожения остановится

    private Transform myTransform; //Для хранения ссылки на трансформ текущего обьекта

    private void Awake() //Метод вызывается при инициализации обьекта (до Start)
    {
        myTransform = transform;
    }

    [ContextMenu("ActivateModule")] //Добавляет пункт "ActivateModule" в контекстное меню компонента в редакторе. При клике на него будет вызван метод
    public void ActivateModule()
    {
        StartCoroutine(DestroyRandomChildObjectCoroutine());
    }

    private IEnumerator DestroyRandomChildObjectCoroutine()
    {
        while (myTransform.childCount > minimalDestroyingObjectsCount)
        {
            int index = Random.Range(0, myTransform.childCount - 1); //Генерирует индекс случайного дочернего обьекта
            Destroy(myTransform.GetChild(index).gameObject); //Уничтожает игровой обьект дочернего элемента с выбранным индексом
            // GetChild(index) получает трансформ ребёнка, затем .gameObject возвращает сам обьект, который передаётся в Destroy()
            yield return new WaitForSeconds(destroyDelay); //Приостанавливает выполнение на количество секунд, равное destroyDelay
        }
        Destroy(gameObject, Time.deltaTime); //Когда цикл завершён (достигнуто минимальное количество детей), уничтожается сам игровой обьект, на котором висит этот скрипт
    }
}
