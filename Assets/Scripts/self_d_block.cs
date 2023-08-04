using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class self_d_block : MonoBehaviour
{
    private Animator anim;
    private BoxCollider2D BoxCollider2D;

    private void Start()
    {
        anim = GetComponent<Animator>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isDestroyed", true);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isDestroyed", false);
        }
    }
    public void Destroyed()
    {
        BoxCollider2D.enabled = false;
    }
    public void Restore()
    {
        BoxCollider2D.enabled = true;
        
    }

}
