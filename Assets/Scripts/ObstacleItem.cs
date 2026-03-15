using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObstacleItem : MonoBehaviour
{
    [Range(0f, 1f)] 
    public float currentValue = 1f; //Здоровье от 0 до 1
    public UnityEvent onDestroyObstacle; //Событие при уничтожении

    private Renderer rend; //Хранение ссылки на компонент Renderer объекта. Понадобится для изменения цвета
    private MaterialPropertyBlock propBlock; //MaterialPropertyBlock позволяет изменять свойства материала (например цвет) у конкретного экземпляра объекта, не создавая копий материала и не влияя на другие объекты с тем же материалом
    private static readonly int ColorProp = Shader.PropertyToID("_Color"); //Статическое поле только для чтения. Shader.PropertyToID("_Color") преобразует строковое имя свойства "_Color" в числовой идентификатор (для оптимизации производительности)
    // Если написать свойство сразу в propBlock.SetColor("_Color", targetColor), то при каждом вызове SetColor юнити снова вычислит этот идентификатор из строки. Даже если строка одна и та же, операция хеширования выполняется каждый раз.
    // Если на сцене много препятствий, и каждое из них получает урон каждый кадр, то UpdateColor будет вызываться очень часто. В такой ситуации лишние вычисления хеша строки при каждом вызове SetColor могут создать заметную нагрузку на процессор
    private void Awake()
    {
        //Получаем компонент Renderer
        rend = GetComponent<Renderer>();
        if (rend == null)
        {
            Debug.LogError("ObstacleItem требует наличия Renderer на объекте!");
            return;
        }

        propBlock = new MaterialPropertyBlock(); 
        UpdateColor(); //Устанавливает начальный цвет объекта в соответствии с текущим currentValue
    }

    //Метод для нанесения урона
    public void GetDamage(float value)
    {
        currentValue = Mathf.Clamp01(currentValue - value); //Вычитает переданное значение value из текущего currentValue, а затем ограничивает результат диапазоном от 0 до 1
        UpdateColor(); //Обновляет цвет объекта в соответствии с новым значением currentValue

        if (currentValue <= 0f)
        {
            onDestroyObstacle.Invoke(); //Вызывает событие onDestroyObstacle. Все методы, подписанные на это событие в инспекторе или через код, будут выполнены
            Destroy(gameObject); //Уничтожает игровой объект, к которому прикреплён скрипт
        }
    }

    //Обновление цвета в зависимости от текущего здоровья
    private void UpdateColor()
    {
        //Вычисляет целевой цвет с помощью линейной интерполяции Color.Lerp. При currentValue = 0 цвет будет красным, при currentValue = 1 — белым, при промежуточных значениях — смесью красного и белого
        Color targetColor = Color.Lerp(Color.red, Color.white, currentValue);

        rend.GetPropertyBlock(propBlock); //Загружает текущие свойства блока материала из рендерера в propBlock (чтобы изменить цвет, сохранив остальные свойства)
        propBlock.SetColor(ColorProp, targetColor); //Устанавливает в propBlock цвет для свойства, идентификатор которого хранится в ColorProp (то есть "_Color"), равным targetColor
        rend.SetPropertyBlock(propBlock); //Применяет изменённый MaterialPropertyBlock к рендереру
    }
}
