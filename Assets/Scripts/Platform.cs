using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector2[] Points;

    private void Update()
    {
        Patrul();
    }

    private void Patrul()
    {
        float t = Mathf.PingPong(Time.time * 0.2f, 1f);
        gameObject.transform.position = Vector2.Lerp(Points[0], Points[1], t);
    }
}
