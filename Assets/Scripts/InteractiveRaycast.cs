using UnityEngine;

public class InteractiveRaycast : MonoBehaviour
{
    [SerializeField] private GameObject prefab; // Префаб кубика со скриптом InteractiveBox
    
    private Camera mainCamera;
    private InteractiveBox selectedBox; // Локальная переменная для хранения выбранного компонента
    
    private void Start()
    {
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }
        
        if (prefab == null)
        {
            Debug.LogError("Prefab is not assigned!");
        }
    }
    
    private void Update()
    {
        // Обработка левого клика
        if (Input.GetMouseButtonDown(0))
        {
            HandleLeftClick();
        }
        
        // Обработка правого клика
        if (Input.GetMouseButtonDown(1))
        {
            HandleRightClick();
        }
    }
    
    private void HandleLeftClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            // Проверяем, кликнули ли по поверхности с тегом InteractivePlane
            if (hit.collider.CompareTag("InteractivePlane"))
            {
                CreateObjectAtPosition(hit);
            }
            // Проверяем, кликнули ли по объекту с InteractiveBox
            else
            {
                InteractiveBox hitBox = hit.collider.GetComponent<InteractiveBox>();
                if (hitBox != null)
                {
                    // Если выбранного объекта нет - запоминаем его
                    if (selectedBox == null)
                    {
                        selectedBox = hitBox;
                        Debug.Log($"Selected box: {hitBox.gameObject.name}");
                    }
                    // Если выбранный объект существует и это не тот же самый объект
                    else if (selectedBox != hitBox)
                    {
                        // Добавляем текущий объект как next для выбранного
                        selectedBox.AddNext(hitBox);
                        Debug.Log($"Added {hitBox.gameObject.name} as next to {selectedBox.gameObject.name}");
                        selectedBox = null; // Сбрасываем выбор после добавления
                    }
                    else
                    {
                        // Если кликнули на тот же самый объект - просто сбрасываем выбор
                        selectedBox = null;
                        Debug.Log("Selection cleared");
                    }
                }
            }
        }
    }
    
    private void HandleRightClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit))
        {
            InteractiveBox hitBox = hit.collider.GetComponent<InteractiveBox>();
            if (hitBox != null)
            {
                // Если удаляемый объект был выбран - сбрасываем выбор
                if (selectedBox == hitBox)
                {
                    selectedBox = null;
                }
                
                Destroy(hitBox.gameObject);
                Debug.Log($"Destroyed: {hitBox.gameObject.name}");
            }
        }
    }
    
    private void CreateObjectAtPosition(RaycastHit hit)
    {
        // Используем RaycastHit.normal для корректного размещения объекта
        // Размещаем объект на поверхности, учитывая его размер
        // Получаем размер префаба (предполагаем, что это куб)
        Vector3 prefabSize = prefab.transform.localScale;
        Vector3 position = hit.point + hit.normal * (prefabSize.y / 2f);
        
        // Создаем экземпляр префаба
        GameObject newObject = Instantiate(prefab, position, Quaternion.identity);
        
        // Поворачиваем объект в соответствии с нормалью поверхности
        // (чтобы объект был ориентирован относительно поверхности)
        newObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        
        Debug.Log($"Created object at position: {position}");
    }
}