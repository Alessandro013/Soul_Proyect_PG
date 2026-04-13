using UnityEngine;

public class EnemyChaseAndAttack2D : MonoBehaviour
{
    // Parametros configurables desde el Inspector.
    public float speed = 1f;
    public float checkRadius = 5f;
    public float attackRadius = 1f;
    public float attackCooldown = 1.5f;
    public float returnThreshold = 0.1f;
    public string playerTag = "Player";
    public string walkBoolName = "isWalking";
    public string attackTriggerName = "attack";
    public string dieTriggerName = "die";
    public LayerMask whatIsPlayer;

    // Referencias internas de componentes y objetivo.
    protected Transform player;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected Vector2 moveDirection;

    // Estados internos.
    protected bool isInCheckRadius;
    protected bool isInAttackRadius;
    protected bool isReturningHome;

    // Datos de reposo para volver al punto inicial.
    protected Vector3 initialScale;
    protected Vector2 homePosition;

    private float lastAttackTime;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialScale = transform.localScale;
        homePosition = transform.position;
        TryFindPlayer();
    }

    protected virtual void Update()
    {
        if (player == null)
        {
            TryFindPlayer();
        }

        if (player == null)
        {
            BeginReturnHome();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        bool playerInValidLayer = IsPlayerInValidLayer();
        isInCheckRadius = distanceToPlayer <= checkRadius && playerInValidLayer;
        isInAttackRadius = distanceToPlayer <= attackRadius && playerInValidLayer;

        if (isInAttackRadius)
        {
            isReturningHome = false;
            moveDirection = Vector2.zero;
            SetWalkState(false);

            if (Time.time >= lastAttackTime + attackCooldown)
            {
                TrySetTrigger(attackTriggerName);
                lastAttackTime = Time.time;
            }

            return;
        }

        if (isInCheckRadius)
        {
            isReturningHome = false;
            MoveToward(player.position);
            return;
        }

        BeginReturnHome();
    }

    protected virtual void FixedUpdate()
    {
        if (rb == null)
        {
            return;
        }

        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    // Metodo publico para la muerte del enemigo (llamado desde vida/dano, por ejemplo).
    public virtual void Die()
    {
        TrySetTrigger(dieTriggerName);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        enabled = false;

        if (rb != null) rb.linearVelocity = Vector2.zero;

        Destroy(gameObject, 2f);
    }

    // Busca el objeto con el tag configurado y guarda su Transform.
    protected virtual void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    protected virtual bool IsPlayerInValidLayer()
    {
        if (whatIsPlayer.value == 0) return true;
        return (whatIsPlayer.value & (1 << player.gameObject.layer)) != 0;
    }

    protected virtual void BeginReturnHome()
    {
        isReturningHome = true;

        if (Vector2.Distance(transform.position, homePosition) <= returnThreshold)
        {
            moveDirection = Vector2.zero;
            SetWalkState(false);
            return;
        }

        MoveToward(homePosition);
    }

    protected virtual void MoveToward(Vector2 targetPosition)
    {
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        moveDirection = direction;
        SetWalkState(true);

        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
        else if (targetPosition.x > transform.position.x)
        {
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        }
    }

    protected virtual void SetWalkState(bool isWalking)
    {
        if (anim != null)
        {
            anim.SetBool(walkBoolName, isWalking);
        }
    }

    // Evita errores si el Animator no tiene creado el parametro solicitado.
    protected virtual void TrySetTrigger(string triggerName)
    {
        if (anim == null) return;

        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger && parameter.name == triggerName)
            {
                anim.SetTrigger(triggerName);
                return;
            }
        }
    }

    // Dibuja en el editor los radios de deteccion para depuracion visual.
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(Application.isPlaying ? (Vector3)homePosition : transform.position, returnThreshold);
    }
}