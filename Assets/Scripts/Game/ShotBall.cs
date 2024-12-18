using UnityEngine;

public class ShotBall : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float shotForce = 500f;
    [SerializeField] private float timeToDestroy = 10.0f;
    [SerializeField] private float baseInfectionRadius = 1.0f;

    private Rigidbody _rigidbody;
    
    private float _infectionRadius;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _infectionRadius = baseInfectionRadius;
        
        Destroy(gameObject, timeToDestroy);
    }

    public void IncreaseInfectionRadius(float chargeAmount)
    {
        _infectionRadius += chargeAmount;
    }

    public void Shoot()
    {
        _rigidbody.AddForce(transform.forward * shotForce);
        Debug.Log("Shot fired!");
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.GetComponent<Obstacles>()) return;
        
        Explode();
    }

    private void Explode()
    {
        Collider[] hitObjects = Physics.OverlapSphere(transform.position, _infectionRadius);
        
        foreach (Collider col in hitObjects)
        {
            Obstacles obstacle = col.GetComponent<Obstacles>();

            if (obstacle != null)
            {
                obstacle.Infect();
            }
        }
        
        Destroy(gameObject);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _infectionRadius);
    }
}