using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{

private float speed = 5;
public Rigidbody rb;
private Vector3 direction;
private KnockbackReceiver knockback;

   void Awake()
   {
       knockback = GetComponent<KnockbackReceiver>();
   }

   void Update()
       {
        transform.rotation = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0);
        
        direction = Vector3.zero;

           if (Keyboard.current[Key.W].isPressed){
                direction += transform.forward;             
         }
                    if (Keyboard.current[Key.S].isPressed){
                direction -= transform.forward;             
         }
                    if (Keyboard.current[Key.D].isPressed){
                direction += transform.right;             
         }
                    if (Keyboard.current[Key.A].isPressed){
                direction -= transform.right;             
         }

           direction = Vector3.ClampMagnitude(direction, 1f);
        }

    void FixedUpdate()
    {
        // Durante o empurrão o jogador perde o controle (senão a velocidade do WASD anula o knockback).
        if (knockback != null && knockback.IsStunned) return;

        rb.linearVelocity = new Vector3(direction.x * speed,  
        rb.linearVelocity.y, direction.z * speed);      
       }
}
