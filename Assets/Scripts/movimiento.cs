using UnityEngine;

public class movimiento : MonoBehaviour
{
    public float velocidad = 5f;
    private Rigidbody2D rb;
    private Vector2 movimientoInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Captura teclas WASD o flechas
        movimientoInput.x = Input.GetAxisRaw("Horizontal");
        movimientoInput.y = Input.GetAxisRaw("Vertical");

        // Normaliza para evitar moverse más rápido en diagonal
        movimientoInput = movimientoInput.normalized;
    }

    void FixedUpdate()
    {
        // Movimiento del personaje
        rb.linearVelocity = movimientoInput * velocidad;
        
        // Evita que el personaje rote por colisiones
        rb.angularVelocity = 0f;
    }
}