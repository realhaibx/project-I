using UnityEngine;

public class TargetFollowCamera : MonoBehaviour
{
    private const float CAMERA_DEFAULT_SPEED = 20.0f;
    
    public Transform target;
    public Vector3 offset;
    
    [SerializeField] public float speed = CAMERA_DEFAULT_SPEED;
    
    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * speed);
    }
}
