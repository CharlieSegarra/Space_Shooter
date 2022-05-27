using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour//Unity specfic
{
    [SerializeField] private float _speed = 3.5f;
    private float _speedMulitiplier = 2;

    float horizontalInput; //Unity gets input from a or d key; Changes x axis of player gameObject
    float verticalInput; //Unity gets input from s or w key; Changes y axis of player gameObject
    [SerializeField] private GameObject _laserPrefab; //able to drag laser prefab in unity to slot in player tab
    [SerializeField] private GameObject _laserCombo1;
    [SerializeField] private GameObject _laserCombo2;
    [SerializeField] private GameObject _laserCombo3;

    [SerializeField] private float _firerate = 0.5f;
    private float _canFire= -1f;
    
   [SerializeField] private int _lives = 3;

   private  SpawnManager _spawnManager; // global to communicate with spawn manager script

   [SerializeField] private bool _laserCombo = false;//is triple shot active
   private bool _isSpeedBoostActive = false;

   [SerializeField] private bool _isShieldActive = false;
   [SerializeField] private GameObject _rightEngine;
   [SerializeField] private GameObject _leftEngine;
   [SerializeField] private GameObject shieldAnimation;
   [SerializeField] private int _score;

    private UIManger _uiManager;

    [SerializeField] private AudioClip _laserAudio;
    [SerializeField] private AudioSource _audioSource;
  




    // Start is called before the first frame update
    void Start()
    {
        //take current position = new position (0,0,0)
        //Accesses Spawn Manger, Canvas, Aduio
        //If Scripts are null will notify in console
        transform.position = new Vector3(0,0,0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManger>();
        _audioSource = GetComponent<AudioSource>();

        if(_audioSource == null)
        {
            Debug.LogError("AudioSource an player is null");
        }
        else
        {
            _audioSource.clip = _laserAudio;
        }
        
        if(_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        if(_uiManager == null)
        {
            Debug.LogError("The UI Manager is null");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();   
        


        //if space key is hit spawn gameObect
        //TIME.time is it greater than _canfire/ meaning how long the game has been running
        //if yes the instantiate
        if(Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }

    void CalculateMovement()
    {
    //calculates movment by controlling the axis of the game object
    //When edge of screen you'll show on the otherside
    // Boundraies prevent you from flying off the screen

        horizontalInput = Input.GetAxis("Horizontal"); 
        verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        ///(1,0,0) *speed* real time // shows how fast the position is changed on the x/y axis
        //transform.Translate(Vector3.right * horizontalInput * _speed * Time.deltaTime);
        //transform.Translate(Vector3.up * verticalInput * _speed * Time.deltaTime);

       
        transform.Translate(direction *_speed * Time.deltaTime);
        
        

        //if player position on the y axis is greater than 0
        //y position = 0
        // else if position on the y is less than -3.8f
        //y position = -3.8f
        if(transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0 ,0);
        }
        else if(transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        //BOUNDARIES
        //if player on the x > 11.3 
        //x position = -11.3
        //else if player on the x is less than -11.3
        //x position = 11.3
        if(transform.position.x >= 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if(transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        {  
            //formula for fire rate cool down
        _canFire = Time.time + _firerate;

            if (_laserCombo == true)
            {
                //instantiate for the triple shot
                Instantiate(_laserCombo1, transform.position + new Vector3(0, 1.007f, 0), Quaternion.identity);
                Instantiate(_laserCombo2, transform.position + new Vector3(0.803f, -0.237f, 0), Quaternion.identity);
                Instantiate(_laserCombo3, transform.position + new Vector3(-0.803f, -0.237f, 0), Quaternion.identity);
            }
            else
            {
                //Quaternion.itendity = default rotation
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.007f, 0), Quaternion.identity);
            }

            _audioSource.Play();
       
        }
    }


    public void Damage()
    {
        // calls damage method from enemy script

        //if shields active 
        //do nothing
        //deactivate shields
        //return;
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
                    shieldAnimation.gameObject.SetActive(false);

            return;
        }
        
        _lives = _lives - 1; // subtracts 1 life

        //if lives is 2
        if(_lives == 2)
        {
            _leftEngine.SetActive(true);
        }
        else if(_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        //enable right engine damage
        //if lives 1 
        //enable left engine


        _uiManager.UpdateLives(_lives);

        //check if dead
        // if yes destroy us
        if(_lives < 1)
        {

            //communicate with spawn manager and let them know to stop spawning

            _spawnManager.OnPlayerDeath();           

            Destroy(this.gameObject); // this.gameObject being player
        }
    }
    public void TripleShotActive()
    {
        //triple shot active becomes true
        _laserCombo = true;
        StartCoroutine(laserComboPowerDownRoutine());
    }

      
    //ienumerator triple shot power down
    //wait 5 seconds
    //set triple shot to false

       IEnumerator laserComboPowerDownRoutine()
       {
           yield return new WaitForSeconds(5.0f);
           _laserCombo = false;
       }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed = _speed *_speedMulitiplier;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive =false;
        _speed = _speed / _speedMulitiplier;
    }
    
    public void ShieldsActive()
    {
        _isShieldActive = true;
        shieldAnimation.gameObject.SetActive(true);
    }

    //method to add 10 to score
    //communicate with UI to update score

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

}




