using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    [SerializeField] private GameObject Platform;

    [SerializeField] private Transform Target;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        float step = 5 * Time.deltaTime;
        Platform.transform.position = Vector2.Lerp(Platform.transform.position, Target.position, step);
    }
}
