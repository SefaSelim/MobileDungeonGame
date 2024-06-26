using UnityEngine;
using UnityEngine.UI;

public class QuestArrow : MonoBehaviour
{
    public RectTransform arrow; // Ok imgesi
    public Transform player; // Oyuncu
    public Transform target; // Hedef nokta
    public float arrowDistance = 0.7f; // Oku oyuncudan ne kadar uzakta tutmak istediğinizi belirtir

    void Update()
    {
        // Hedefe doğru olan yönü hesapla
        Vector3 direction = target.position - player.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Oku döndür
        arrow.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Oku oyuncunun etrafında hareket ettir
        Vector3 offset = direction.normalized * arrowDistance;
        arrow.position = player.position + offset;
    }
}
