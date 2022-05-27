using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4.0f;
    private Player _player;
    private Animator _anim;

    private AudioSource _audioSource;
    



    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        //null check player
        if(_player == null)
        {
            Debug.LogError("The Player is Null");
        }

        //assign component to anim
        _anim = GetComponent<Animator>();
        //null check
        if(_anim == null)
        {
            Debug.LogError("Anim is null");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //move down at 4m/s
        //if on bottom of screen
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y < -6f)
        {  
            float randomX = Random.Range(-8f, 8f);
             // respawn at top with new random x positon
            transform.position = new Vector3(randomX , 7 , 0);
        }       
    }


    //detects collisosns
    //other is other gameObject that collides with enemy 
    private void OnTriggerEnter2D(Collider2D other) 
    {
        //if other is player
        //destroy enemy
        if(other.tag == "Player")
        {
            //damage player / 3 lives
            //acesses player component and method
            Player player = other.transform.GetComponent<Player>();

            //null check
            if(player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;

            Destroy(this.gameObject, 2.8f);
            _audioSource.Play();
        }



        //damage the player
        

        //if other is laser
        //destory laser = other.gameObject
        //destory enemy
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);
            //add 10 to score
            if(_player != null)
            {
                _player.AddScore(10);
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
           
        }        
    }
}
