using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyaSampleScript : SampleScript
{
    private Transform target;
    
    [SerializeField]
    [Min(0.1f)]
    [Tooltip("Время сжатия в секундах")]
    private float compressTime = 0.5f;
    
    public override void Use()
    {
        target = transform;
        if (target == null)
        {
            Debug.LogError("Target не назначен!");
            return;
        }
        
        StartCoroutine(CompressAllChildren());
    }
    
    private IEnumerator CompressAllChildren()
    {
        // Создаем список дочерних объектов
        List<Transform> children = new List<Transform>();
        
        foreach (Transform child in target)
        {
            children.Add(child);
        }
        
        // Запускаем сжатие для всех детей
        foreach (Transform child in children)
        {
            StartCoroutine(CompressAndDestroy(child));
        }
        
        // Ждем завершения сжатия самого долгого объекта
        yield return new WaitForSeconds(compressTime);
        
        // Удаляем все объекты
        foreach (Transform child in children)
        {
            if (child != null)
                Destroy(child.gameObject);
        }
        
        Debug.Log($"Удалено {children.Count} объектов");
    }
    
    private IEnumerator CompressAndDestroy(Transform obj)
    {
        Vector3 startScale = obj.localScale;
        float elapsedTime = 0f;
        
        // Плавно уменьшаем объект
        while (elapsedTime < compressTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / compressTime;
            
            // Плавное уменьшение от начального размера к нулю
            obj.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            
            yield return null;
        }
        
        // Убеждаемся, что масштаб точно ноль
        obj.localScale = Vector3.zero;
    }
}