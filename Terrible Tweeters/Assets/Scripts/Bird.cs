using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[SelectionBase]
public class Bird : MonoBehaviour
{
    //variables listed here
    //serialized fields show in the inspector and can be edited
    [SerializeField] float _launchForce = 500;
    [SerializeField] float _maxDragDistance = 3;

    
    Vector2 _startPosition;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Scene _currentScene;

    //called before start, caches rigidbody var as a rigidbody and sprite render var as a sprite renderer
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    //identifies the start position as equal to the rigidbody2d position, kinematic player is set to true
    void Start()
    {
        _startPosition = _rigidbody2D.position;
        _rigidbody2D.isKinematic = true;
    }

    //turns the character red when the player is clicking on it
    void OnMouseDown()
    {
        _spriteRenderer.color = Color.red;
    }


    void OnMouseUp()
    {
        Vector2 currentPosition = _rigidbody2D.position;
        Vector2 direction = _startPosition - currentPosition;
        direction.Normalize();
        _rigidbody2D.isKinematic = false;
        _rigidbody2D.AddForce(direction * _launchForce);
        FindObjectOfType<AudioManager>().Play("Flying SFX");
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    void OnMouseDrag()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPosition = mousePosition;

        float distance = Vector2.Distance(desiredPosition, _startPosition);
        if (distance > _maxDragDistance)
        {
            Vector2 direction = desiredPosition - _startPosition;
            direction.Normalize();
            desiredPosition = _startPosition + (direction * _maxDragDistance);
        }
        if (desiredPosition.x > _startPosition.x)
            desiredPosition.x = _startPosition.x;
        _rigidbody2D.position = desiredPosition;
        //GetComponent<SpriteRenderer>().sprite = _sprite1;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _rigidbody2D.position = _startPosition;
            _rigidbody2D.isKinematic = true;
            _rigidbody2D.velocity = Vector2.zero;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _currentScene = SceneManager.GetActiveScene(); SceneManager.LoadScene(_currentScene.name);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(ResetAfterDelay());
    }

    IEnumerator ResetAfterDelay()
    {
        FindObjectOfType<AudioManager>().Play("Player Death");
        yield return new WaitForSeconds(3);
        _rigidbody2D.position = _startPosition;
        _rigidbody2D.isKinematic = true;
        _rigidbody2D.velocity = Vector2.zero;
    }
}
