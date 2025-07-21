//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ButtonController : MonoBehaviour
//{
//    private Animator animator;
//    public Rigidbody2D object_1;
//    public Rigidbody2D object_2;
//    // Start is called before the first frame update
//    void Start()
//    {
//        animator = GetComponent<Animator>();
//        animator.SetBool("isPressed", false);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (animator.GetBool("isPressed"))
//        {
//            if(object_1 != null)
//            {

//            }
//        }
//        else
//        {
//            controlObject.SetDeactivate();
//        }
//    }
//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Blue"))
//        {
//            animator.SetBool("isPressed", true);
//        }
//    }
//    private void OnCollisionExit2D(Collision2D collision)
//    {
//        if (collision.gameObject.CompareTag("Red") || collision.gameObject.CompareTag("Blue"))
//        {
//            animator.SetBool("isPressed", false);
//        }
//    }
//}
