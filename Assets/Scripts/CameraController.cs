using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static CameraController Main;
    public GameObject Target;
    public PlayerController PC;
    public Rect Bounds;
    private Camera Cam;
    public SpriteRenderer Fader;
    public TextMeshPro HP;
    public TextMeshPro Score;
    public int MaxTreasure = 0;

    public bool YMinOverride = false;
    public float YMin = -0.5f;
    public bool YMaxOverride = false;
    public float YMax;

    private void Awake()
    {
        Main = this;
    }

    void Start()
    {
        if(Fader != null)
            Fader.gameObject.SetActive(false);
        if (!PC) PC = Target.GetComponent<PlayerController>();
        Cam = GetComponent<Camera>();
        Rect bounds = new Rect();
        Collider2D[] plats = BoxCollider2D.FindObjectsOfType<Collider2D>();
        foreach(Collider2D coll in plats)
        {
            GameObject p = coll.gameObject;
            bounds.x = Mathf.Min(bounds.x,(p.transform.position.x + coll.offset.x) - coll.bounds.extents.x);
            bounds.width = Mathf.Max(bounds.width,(p.transform.position.x + coll.offset.x) + coll.bounds.extents.x);
            bounds.y = Mathf.Min(bounds.y,(p.transform.position.y + coll.offset.y) - coll.bounds.extents.y);
            bounds.height = Mathf.Max(bounds.height,(p.transform.position.y + coll.offset.y) + coll.bounds.extents.y);
        }

        if (PC?.MaxHP > 0) HP.gameObject.SetActive(true);

        if (YMinOverride) bounds.y = YMin;
        if (YMaxOverride) bounds.height = YMax;
        bounds.x += 9;
        bounds.width -= 9;
        bounds.y += 5;
        bounds.height -= 5;
        Bounds = bounds;
        StartCoroutine(ShowName());
    }

    void Update()
    {
        if (PC != null)
        {
            if (PC.MaxHP > 0) HP.text = PC.HP + "/" + PC.HP;
            if (MaxTreasure > 0) Score.text = "Score: " + PC.Score + "/" + MaxTreasure;
        }
    }
    
    void FixedUpdate()
    {
        Vector3 desired = Target.transform.position;
        desired.z = transform.position.z;
        desired.x = Mathf.Clamp(desired.x,Bounds.x, Bounds.width);
        desired.y = Mathf.Clamp(desired.y,Bounds.y, Bounds.height);
        transform.position = Vector3.Lerp(transform.position,desired,0.3f);
        transform.position = Vector3.MoveTowards(transform.position,desired,0.01f);
    }

    IEnumerator ShowName()
    {
        GameObject name = new GameObject("Name");
        TextMeshPro tmp = name.AddComponent<TextMeshPro>();
        name.transform.SetParent(transform);
        name.transform.localPosition = new Vector3(0,0,1.1f);
        
//        tmp.au = true;
        tmp.text = SceneManager.GetActiveScene().name.ToUpper();
        tmp.alignment = TextAlignmentOptions.Center;
        yield return new WaitForSeconds(1);
        Destroy(name);

    }

    public void AddTreasure(TreasureScript t)
    {
        MaxTreasure += t.Value;
        Score.gameObject.SetActive(true);
    }
}
