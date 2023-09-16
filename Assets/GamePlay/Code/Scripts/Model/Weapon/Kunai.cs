using GamePlay.Code.Scripts;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    private const string PREFAB_ENEMY_TAG = "Enemy";

    public GameObject hitVFX;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    private void OnInit()
    {
        rb.velocity = transform.right * 10f;
        Invoke(nameof(OnDespawn), 4f);
    }

    public void OnDespawn()
    {
        
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(PREFAB_ENEMY_TAG))
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null)
            {
                Instantiate(hitVFX, transform.position, transform.rotation);
                enemy.OnHit(30.0f);
                OnDespawn();
            }
        }
    }
}