using UnityEngine;

public class InteractiveBox : MonoBehaviour
{
    private InteractiveBox next;

    //метод для добавления следующего InteractiveBox
    public void AddNext(InteractiveBox box)
    {
        next = box;
    }

    void Update()
    {
        if (next == null) return;

        Vector3 start = transform.position;
        Vector3 end = next.transform.position;

        Debug.DrawLine(start, end, Color.green); //луч-дебаг

        Vector3 direction = end - start;
        float distance = direction.magnitude; //magnitude это длина вектора

        //реальный луч 
        Ray ray = new Ray(start, direction.normalized); //normalized задает вектору длину 1, потому что важно только направление
        RaycastHit hit; //переменная, куда запишется результат попадания

        //проверяем попадание луча
        if (Physics.Raycast(ray, out hit, distance))
        //Physics.Raycast выстреливает лучом ray на макс. расст-е distance и записывает объект попадания в hit; вернет true, если попадание произошло
        {
            if (hit.collider.TryGetComponent(out ObstacleItem obstacle)) //проверяем есть ли ObstacleItem
            {
                obstacle.GetDamage(Time.deltaTime);
            }
        }
    }
}