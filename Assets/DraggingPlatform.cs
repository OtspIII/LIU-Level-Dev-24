using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DraggingPlatform : MonoBehaviour
{
    public bool Dragging = false;
    public Vector3 LastMouse;
    public SpriteRenderer SR;
    public Color MoveColor = Color.blue;
    public Color OriginalColor;
    public BoxCollider2D BodyBox;
    public BoxCollider2D StickyBox;
    public bool Sticky = true;
    public List<Rigidbody2D> Touching;

    private void Start()
    {
        if(SR != null)
            OriginalColor = SR.color;
        if (!Sticky)
        {
            StickyBox.gameObject.SetActive(false);
        }
        else
        {
            Vector2 size = BodyBox.size;
            size.y *= 0.5f;
            StickyBox.size = size;
            StickyBox.offset = BodyBox.offset + new Vector2(0,0.01f + size.y/2);
        }
    }

    private void Update()
    {
        if (Dragging)
        {
            Vector3 old = LastMouse;
            LastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 move = LastMouse - old;
            transform.position += move;
            if (Sticky)
            {
                foreach (Rigidbody2D rb in Touching)
                {
                    rb.MovePosition(rb.transform.position + move);
                }
            }
        }
    }

    private void OnMouseDown()
    {
        Dragging = true;
        if(SR != null)
            SR.color = MoveColor;
        LastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if(SR != null)
            SR.color = OriginalColor;
        Dragging = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        Debug.Log("OTE: " + other.gameObject.name + " / " + rb);
        if(rb != null && !Touching.Contains(rb))
            Touching.Add(rb);
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if(rb != null)
            Touching.Remove(rb);
    }
}
