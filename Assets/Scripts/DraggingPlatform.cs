using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Dictionary<Rigidbody2D,float> TouchingTemp = new Dictionary<Rigidbody2D, float>();
    private bool MidMove = false;
    public float TouchTimedown = 0.5f;

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
            StickyBox.offset = BodyBox.offset + new Vector2(0,size.y/1.5f);
        }
    }

    private void Update()
    {
        if (Dragging)
        {
            MidMove = true;
            Vector3 old = LastMouse;
            LastMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 move = LastMouse - old;

            Dictionary<Rigidbody2D, Vector2> stick = new Dictionary<Rigidbody2D, Vector2>();
            if (Sticky)
            {
                foreach (Rigidbody2D rb in TouchingTemp.Keys.ToArray())
                {
                    stick.Add(rb,rb.transform.position + move);
                }
            }
            transform.position += move;
            foreach (Rigidbody2D rb in stick.Keys)
            {
                rb.transform.position = stick[rb];
            }
            MidMove = false;
        }

        foreach (Rigidbody2D rb in TouchingTemp.Keys.ToArray())
        {
            if (!Touching.Contains(rb))
            {
                TouchingTemp[rb] -= Time.deltaTime;
                if (TouchingTemp[rb] <= 0)
                    TouchingTemp.Remove(rb);
            }
            else
                TouchingTemp[rb] = TouchTimedown;

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb != null && !Touching.Contains(rb))
        {
            Touching.Add(rb);
            TouchingTemp.TryAdd(rb, 0);
            TouchingTemp[rb] = TouchTimedown;
        }
           
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        Rigidbody2D rb = other.gameObject.GetComponent<Rigidbody2D>();
        if(rb != null)
            Touching.Remove(rb);
    }
}
