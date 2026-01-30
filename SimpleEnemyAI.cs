using UnityEngine;

[RequireComponent(typeof(Animator))]
public class SimpleEnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float moveSpeed = 2f;
    public float rotationSpeed = 5f;
    public float attackCooldown = 1.5f;

    public int damage = 10;
    public LayerMask groundMask;

    private Animator animator;
    private float lastAttackTime = -999f;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        if (groundMask == 0)
            groundMask = LayerMask.GetMask("Default");
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
            Attack();
        else if (distance <= detectionRange)
            Chase();
        else
            Idle();
    }

    void Idle()
    {
        animator.SetInteger("State", 0);
    }

    void Chase()
    {
        animator.SetInteger("State", 1);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        Vector3 nextPos = transform.position + transform.forward * moveSpeed * Time.deltaTime;

        // Keep enemy grounded using raycast (works for all ground types)
        if (Physics.Raycast(nextPos + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f, groundMask))
        {
            nextPos.y = hit.point.y;
        }

        transform.position = nextPos;
    }

    void Attack()
    {
        animator.SetInteger("State", 2);

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (Time.time > lastAttackTime + attackCooldown)
        {
            lastAttackTime = Time.time;
            PlayerHealth ph = player.GetComponent<PlayerHealth>();
            if (ph != null)
                ph.TakeDamage(damage);
        }
    }
}
