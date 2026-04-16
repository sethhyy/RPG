using UnityEngine;

public class MouseGuideAnimator : MonoBehaviour
{
    public float moveDistance = 15f;
    public float moveSpeed = 2f;
    public float scaleSpeed = 2f;

    private Vector3 startPos;

    void OnEnable()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * moveSpeed) * moveDistance;
        transform.position = startPos + new Vector3(0, yOffset, 0);

        float scale = 1f + Mathf.Sin(Time.time * scaleSpeed) * 0.05f;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
