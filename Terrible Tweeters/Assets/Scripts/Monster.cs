using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Monster : MonoBehaviour
{
    [SerializeField] Sprite _deathSprite;
    [SerializeField] ParticleSystem _particleSystem;

    bool _hasDied;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (DeathFromCollision(collision))
        {
            StartCoroutine(Die());
        }
    }

    bool DeathFromCollision(Collision2D collision)
    {
        if (_hasDied)
            return false;

        Bird bird = collision.gameObject.GetComponent<Bird>();
        if (bird != null)
            return true;

        if (collision.contacts[0].normal.y < -0.5)
            return true;

        return false;
    }
    IEnumerator Die()
    {
        GetComponent<SpriteRenderer>().sprite = _deathSprite;
        _particleSystem.Play();
        FindObjectOfType<AudioManager>().Play("Enemy Death");
        yield return new WaitForSeconds(1);
        _hasDied = true;
        gameObject.SetActive(false);
    }
}
