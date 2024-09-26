using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Unit
{
    public float invincibleTime = 3f;

    private float timer = 0;

    public override void OnUpdate()
    {
        if (death)
            return;

        timer += Time.deltaTime;
        Vector2 pos = transform.position;
        pos.x += Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        pos.y += Input.GetAxis("Vertical") * Time.deltaTime * speed;

        transform.position = pos;

        if (Input.GetButton("Fire1"))
        {
            Fire();
        }
    }

    public void Rebirth()
    {
        StartCoroutine(DoRebirth());
    }

    IEnumerator DoRebirth()
    {
        yield return new WaitForSeconds(2f);
        timer = 0;
        Init();
        Fly();
    }

    public bool Isinvincible
    {
        get { return timer < invincibleTime; }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(death)
            return;
        if (Isinvincible)
            return;

        Item item = col.gameObject.GetComponent<Item>();
        if(item != null)
        {
            item.Use(this);
        }

        Element bullet = col.gameObject.GetComponent<Element>();
        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        if (bullet == null && enemy == null)
        { 
            return;
        }

        if(bullet != null && bullet.side == SIDE.ENEMY)
        {
            hp = hp - bullet.power;
            if(hp <= 0)
            {
                Die();
            }
        }
        if(enemy != null)
        {
            hp = 0;
                Die();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (death)
            return;
        if(Isinvincible)
            return;

        if (col.gameObject.name.Equals("ScoreArea"))
        {
            if(OnScore != null)
            {
                OnScore(1);
            }
        }
    }
}