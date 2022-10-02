using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public static CameraFollow Instance { get; private set; }

    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        if (target is null)
            return;

        Vector3 newPos = target.transform.position + new Vector3(0f, 56f, -76.5f);
        newPos.y = 70;
        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, .01f);
    }
}