using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // Configuración
    public float speed = 2f;
    public float checkRadius = 5f; // Radio para detectar al jugador
    public float attackRadius = 1f; // Radio para atacar
    public LayerMask whatIsPlayer; // Capa donde está el jugador

    // Referencias
    private Transform player;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveDirection;
    
    // Estados
    private bool isInCheckRadius;
    private bool isInAttackRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // Buscamos al jugador al inicio. ¡Asegúrate de que tu Soldier_0 tenga el TAG "Player"!
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // Comprobamos si el jugador está en los rangos
        isInCheckRadius = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
        isInAttackRadius = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

        if (isInCheckRadius && !isInAttackRadius)
        {
            // Perseguir
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            moveDirection = direction.normalized;
            
            // Animación de caminar
            anim.SetBool("isWalking", true);
            
            // Girar el sprite según la dirección
            if (player.position.x < transform.position.x)
                transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda
            else
                transform.localScale = new Vector3(1, 1, 1);  // Mirar a la derecha
        }
        else if (isInAttackRadius)
        {
            // Atacar
            moveDirection = Vector2.zero; // Parar el movimiento
            anim.SetBool("isWalking", false);
            anim.SetTrigger("attack");
        }
        else
        {
            // Parar
            moveDirection = Vector2.zero;
            anim.SetBool("isWalking", false);
        }
    }

    void FixedUpdate()
    {
        // Aplicar movimiento físico
        if (player != null)
        {
            rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
        }
    }

    // Para cuando necesites activar la muerte (por ejemplo, desde un script de salud)
    public void Die()
    {
        anim.SetTrigger("die");
        // Desactivar colisiones y script para que no siga moviéndose
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
        Destroy(gameObject, 2f); // Destruir el objeto tras la animación
    }

    // Dibujar los radios de detección en el editor para visualizarlos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}