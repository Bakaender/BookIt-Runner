using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.Advertisements;
#endif
using Prime31;
using Com.LuisPedroFonseca.ProCamera2D;

namespace BookIt
{
    public class NewMovement : MonoBehaviour
    {
        private GameManager_Master gameManagerMaster;

        private int numberOriginResets;
        public Text activeScoreText;
        public Text pagesText;
        private int pagesCollected;
        private float playerStartX;

        public float menuCameraXOffset;
        public float playingCameraXOffset;
        // movement config
        public float gravity = -25f;
        public float glideGravity = -10f;
        public float startGlideVelocity = 8f;
        public float jumpHeight = 3f;
        public float superJumpHeight = 6f;
        public float initialRunSpeed = 2f;
        public float runSpeed = 8f;
        public float startDelay = 1f;
        private float runSpeedUnModified;

        private bool startDelayActive;       

        //Not used unless we can turn around.
        private float groundDamping = 20f; // how fast do we change direction? higher means faster
        private float inAirDamping = 5f;

        [HideInInspector]
        private float normalizedHorizontalSpeed = 0;

        private CharacterController2D _controller;
        private Animator _animator;
        private RaycastHit2D _lastControllerColliderHit;
        private Vector3 _velocity;

        private float currentGravity;
        private bool canJump, canGlide, amGliding;

        //For Handeling if grounded so can jump.
        private Vector2 groundRayLeft, groundRayRight;
        private RaycastHit2D jumpRayHitLeft, jumpRayHitRight;
        public LayerMask groundRayMask;
        public float groundRayLength;

        //For handeling transition from AI
        private GameObject lastObjectTouched;

        //For handeling revive
        private RaycastHit2D reviveRayHit;
        private Collider2D platformToReviveOn;
        public LayerMask reviveHandlerLayerMask;
        public float reviveRayLength;

        //Achievement variables     
        private int jumpsThisLife;
        private int glidesThisLife;
        private float startTime;
        private float timePlayed;
        private int timesDied;
        private int superJumpWhileGliding;
        //Online
        private AchievementReporter achievReporter;

        //Sound variables
        [Header("Sounds")]
        public AudioClip jumpSound;
        public AudioClip superJumpSound;
        public AudioClip deathSound;
        public AudioClip fireDeathSound;
        public AudioClip currencyPickupSound;
        private AudioSource _audioSource;
        private AudioSource _runAudioSource;
        private AudioSource _glideAudioSource;

        //Gameover stats display
        [Header("Gameover Stats Stuff")]
        public Text gameoverScoreText;
        public Text gameoverPagesText;
        public Text gameoverTimePlayedText;
        public Text gameoverJumpsText;
        public Text gameoverGlidesText;

        //Ad variables
        [Header("Ad Stuff")]
        public Text adButtonText;
        public GameObject gameOverCanReviveCanvas;
        public GameObject revivedCanvas;

        //Patch 1.1 variables
        private bool touchHeld;
        private float pressHeldStartGlideVelocity = 0f;
        private int checkJumpDelay = 0;

        private void OnEnable()
        {
            gameManagerMaster.ResetToOriginEvent += OriginReset;
        }

        private void OnDisable()
        {
            gameManagerMaster.ResetToOriginEvent -= OriginReset;
        }

        void Awake()
        {    
            GameManager_Master.gameState = GameState.Menu;
            ProCamera2D.Instance.OffsetX = menuCameraXOffset;

            gameManagerMaster = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager_Master>();
            if (gameManagerMaster == null)
            {
                Debug.LogError("gameManagerMaster null in Movement - Awake.");
            }

            achievReporter = FindObjectOfType<AchievementReporter>();
            if (achievReporter == null)
            {
                Debug.LogError("achievReporter null in Movement - Awake.");
            }

            numberOriginResets = 0;
            GameManager_Master.canRevive = true;
            GameManager_Master.canRevivePages = true;
            startDelayActive = false;

            lastObjectTouched = null;

            runSpeedUnModified = runSpeed;
            runSpeed = initialRunSpeed;

            currentGravity = gravity;

            //multiple sources used so I can do instant cutoff of run and glide sound when jump or land etc.
            AudioSource[] sources = GetComponents<AudioSource>();
            _audioSource = sources[0];
            _glideAudioSource = sources[1];
            _runAudioSource = sources[2];
           
            _animator = GetComponent<Animator>();
            _animator.SetFloat("RunMultiplier", 1);
            _controller = GetComponent<CharacterController2D>();

            _controller.onTriggerEnterEvent += onTriggerEnterEvent;
            _controller.onTriggerExitEvent += onTriggerExitEvent;

            StartCoroutine(PlayerSpeedBuildUp());
        }

        #region Event Listeners

        void onTriggerEnterEvent(Collider2D col)
        {
            switch (GameManager_Master.gameState)
            {
                case GameState.Menu:
                    if (col.CompareTag("AIJump"))
                    {
                        Jump();
                    }
                    else if (col.CompareTag("SuperJump"))
                    {
                        _runAudioSource.Stop();
                        _glideAudioSource.Stop();
                        _audioSource.PlayOneShot(superJumpSound);

                        col.GetComponent<Animator>().Play(Animator.StringToHash("BookOpen"));

                        canJump = false;
                        if (amGliding)
                        {
                            currentGravity = gravity;
                            amGliding = false;
                        }
                        canGlide = true;

                        _velocity.y = Mathf.Sqrt(2f * superJumpHeight * -gravity);
                        _controller.move(_velocity * Time.deltaTime);
                        _animator.Play(Animator.StringToHash("PlayerJump"));
                    }

                    break;

                case GameState.Playing:
                    if (col.CompareTag("SuperJump"))
                    {
                        col.GetComponent<Animator>().Play(Animator.StringToHash("BookOpen"));

                        canJump = false;
                        if (amGliding)
                        {
                            superJumpWhileGliding++;
                            currentGravity = gravity;
                            amGliding = false;
                        }
                        canGlide = true;

                        _runAudioSource.Stop();
                        _glideAudioSource.Stop();
                        _audioSource.PlayOneShot(superJumpSound);

                        _velocity.y = Mathf.Sqrt(2f * superJumpHeight * -gravity);
                        _controller.move(_velocity * Time.deltaTime);
                        _animator.Play(Animator.StringToHash("PlayerJump"));
                    }
                    else if (col.CompareTag("Death"))
                    {
                        _runAudioSource.Stop();
                        _glideAudioSource.Stop();
                        _audioSource.PlayOneShot(deathSound);

                        Death();
                    }
                    //1.1 Changed for fire immunity.
                    else if (col.CompareTag("Fire") && !GameManager_Master.fireImmune)
                    {
                        _runAudioSource.Stop();
                        _glideAudioSource.Stop();
                        _audioSource.PlayOneShot(fireDeathSound);

                        Death();
                    }
                    else if (col.CompareTag("Currency"))
                    {
                        NewSaveGame.Instance.pages += 1;
                        pagesCollected += 1;

                        _audioSource.PlayOneShot(currencyPickupSound);

                        if (pagesText != null)
                        {
                            pagesText.text = pagesCollected.ToString();
                        }
                        //OPTIMIZE have pool of pages instead of destroy.
                        Destroy(col.gameObject);
                    }
                    else if (col.CompareTag("DoubleCurrency"))
                    {
                        NewSaveGame.Instance.pages += 5;
                        pagesCollected += 5;

                        _audioSource.PlayOneShot(currencyPickupSound);

                        if (pagesText != null)
                        {
                            pagesText.text = pagesCollected.ToString();
                        }
                        //OPTIMIZE have pool of pages instead of destroy.
                        Destroy(col.gameObject);
                    }

                    break;

                case GameState.Paused:
                    break;

                case GameState.GameOver:
                    break;

                default:
                    break;
            }
            
        }

        void onTriggerExitEvent(Collider2D col)
        {
            //Debug.Log("onTriggerExitEvent: " + col.gameObject.name);
        }

#endregion

        // the Update loop contains a very simple example of moving the character around and controlling the animation
        void Update()
        {
            switch (GameManager_Master.gameState)
            {
                case GameState.Menu:
                    CheckJump();
 
                    //Resets gravity and animations if we hit the ground while jumping or gliding.
                    if (canJump && checkJumpDelay == 0)
                    {
                        _glideAudioSource.Stop();
                        if (!_runAudioSource.isPlaying && !startDelayActive)
                        {
                            _runAudioSource.Play();
                        }

                        currentGravity = gravity;
                        canGlide = false;
                        amGliding = false;
                        _velocity.y = 0;
                        _animator.Play(Animator.StringToHash("PlayerRun"));
                    }
                    else
                    {
                        if (checkJumpDelay != 0)
                        {
                            checkJumpDelay--;
                        }
                    }

                    normalizedHorizontalSpeed = 1;

                    var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

                    // apply gravity before moving
                    _velocity.y += currentGravity * Time.deltaTime;

                    _controller.move(_velocity * Time.deltaTime);

                    // grab our current _velocity to use as a base for all calculations
                    _velocity = _controller.velocity;

                    //HARDCODED top of AI platforms so can't die during menu, 
                    //RELEASE probably need updated before release.
                    if (gameObject.transform.position.y < 0.97f)
                    {
                        gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.9755f, 0);
                    }

                    //Resets to origin after player moves to far to avoid float overflow errors.
                    if (gameObject.transform.position.x > NewWorldGen.originResetDistance + 25f)
                    {
                        gameManagerMaster.CallEventResetToOrigin();
                    }

                    break;

                case GameState.Playing:
#if UNITY_EDITOR || UNITY_STANDALONE
                    //RELEASE remove.
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        PressDown();
                    }
                    if (Input.GetKeyUp(KeyCode.Space))
                    {
                        PressUp();
                    }
                    if (Input.GetKeyDown(KeyCode.S))
                    {
                        _animator.runtimeAnimatorController = CharacterDatabase.CharacterAnimations[Random.Range(0, CharacterDatabase.CharacterAnimations.Length)];
                    }
                    if (Input.GetKeyDown(KeyCode.Minus))
                    {
                        Time.timeScale -= 0.1f;
                    }
                    if (Input.GetKeyDown(KeyCode.Plus))
                    {
                        Time.timeScale += 0.1f;
                    }
#endif

                    //Updates the score that is displayed during playing.
                    GameManager_Master.score = ((int)(numberOriginResets * NewWorldGen.originResetDistance + gameObject.transform.position.x - playerStartX)) * 2;
                    activeScoreText.text = GameManager_Master.score.ToString();

                    CheckJump();

                    GetRevivePlatform();

                    //Resets gravity and animations if we hit the ground while jumping or gliding.
                    if (canJump && checkJumpDelay == 0)
                    {
                        _glideAudioSource.Stop();
                        if (!_runAudioSource.isPlaying && !startDelayActive)
                        {
                            _runAudioSource.Play();
                        }

                        currentGravity = gravity;
                        canGlide = false;
                        amGliding = false;
                        _velocity.y = 0;

                        _animator.Play(Animator.StringToHash("PlayerRun"));
                    }
                    else
                    {
                        if (checkJumpDelay != 0)
                        {
                            checkJumpDelay--;
                        }    
                    }

                    normalizedHorizontalSpeed = 1;

                    //TODO May eventually replace this that came with the char controller. Seems to work well with a constant movement force though.
                    // apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
                    smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
                    _velocity.x = Mathf.Lerp(_velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor);

                    // apply gravity before moving
                    _velocity.y += currentGravity * Time.deltaTime;

                    _controller.move(_velocity * Time.deltaTime);

                    //Resets to origin after player moves to far to avoid float overflow errors.
                    if (gameObject.transform.position.x > NewWorldGen.originResetDistance + 25f)
                    {
                        gameManagerMaster.CallEventResetToOrigin();
                    }

                    // grab our current _velocity to use as a base for all calculations
                    _velocity = _controller.velocity;

                    PressHeldAutoGlide();

                    //HACK Kills the player if somehow falls off screen missing death trigger. Seemed to happen sometimes on mobile.
                    if (gameObject.transform.position.y < -10f)
                    {
                        Death();
                    }

                    break;

                case GameState.Paused:
                    //Nothing?
                    break;

                case GameState.GameOver:
                    break;

                default:
                    break;
            }
        }

        private void CheckJump()
        {
            groundRayLeft = gameObject.GetComponent<Collider2D>().bounds.min;
            //HACK May need to adjust offset if to far back, shouldn't ever matter though since left will trigger can jump.
            groundRayRight = new Vector2(groundRayLeft.x + gameObject.GetComponent<Collider2D>().bounds.size.x - 0.19f, groundRayLeft.y);

            jumpRayHitLeft = Physics2D.Raycast(groundRayLeft, new Vector2(-0.6f, -1f), groundRayLength, groundRayMask);
            jumpRayHitRight = Physics2D.Raycast(groundRayRight, Vector2.down, groundRayLength, groundRayMask);

            if (jumpRayHitLeft.collider != null)
            {
                //Waits till lands on next platform before player takes over.
                if (GameManager_Master.playPressed && GameManager_Master.gameState == GameState.Menu && lastObjectTouched != null)
                {
                    if (lastObjectTouched != jumpRayHitRight.transform.gameObject)
                    {
                        _runAudioSource.Stop();
                        startDelayActive = true;

                        _animator.SetFloat("RunMultiplier", 0);
                        //Used to subtract start position from score.
                        playerStartX = gameObject.transform.position.x;
                        _velocity.x = 0f;
                        runSpeed = 0f;
                        StartCoroutine(StartDelay());
                        StartCoroutine(gameManagerMaster.GetComponent<GameManager_PlayPressed>().MoveLogoOffScreen());

                        GameManager_Master.gameState = GameState.Playing;
                        gameManagerMaster.CallEventInputToggle();
                        

                        //For time played achievement.
                        startTime = Time.unscaledTime;
                    }
                }

                if (jumpRayHitLeft.transform.gameObject.tag != "MovingPlatform")
                {
                    lastObjectTouched = jumpRayHitLeft.transform.gameObject;
                }
                
                canJump = true;
            }
            else if (jumpRayHitRight.collider != null)
            {
                canJump = true;
            }
            else
            {
                canJump = false;
            }

#if UNITY_EDITOR

            //Debug.DrawRay(groundRayLeft, new Vector2(-0.6f, -1f) * groundRayLength, Color.magenta);
            //Debug.DrawRay(groundRayRight, Vector2.down * groundRayLength, Color.magenta);
            //Debug.DrawRay(groundRayRight, Vector2.right *0.06f, Color.magenta, 600f);
#endif

        }

        private void GetRevivePlatform()
        {
            reviveRayHit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, reviveRayLength, reviveHandlerLayerMask);

            if (reviveRayHit.collider != null)
            {
                platformToReviveOn = reviveRayHit.collider;
            }   
        }

        #region Public Input Methods and handeling

        public void PressDown()
        {
            Jump();
            touchHeld = true; 
        }

        public void PressUp()
        {
            EndGlide();
            touchHeld = false;
        }

        private void PressHeldAutoGlide()
        {
            if (touchHeld && canGlide && !canJump)
            {
                if (_velocity.y < pressHeldStartGlideVelocity)
                {
                    if (!GameManager_Master.halfGlideGravity)
                    {
                        currentGravity = glideGravity;
                    }
                    else
                    {
                        currentGravity = glideGravity / 2;
                    }
                    
                    _velocity.y = 0f;
                    canGlide = false;
                    amGliding = true;

                    //_audioSource.Stop();
                    _runAudioSource.Stop();
                    _glideAudioSource.Play();

                    _animator.Play(Animator.StringToHash("PlayerJumpGlideTransition"));

                    switch (GameManager_Master.gameState)
                    {
                        case GameState.Menu:
                            break;
                        case GameState.Playing:
                            glidesThisLife++;
                            break;
                        case GameState.Paused:
                            break;
                        case GameState.GameOver:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //Own method for controlling with mouse clicks or touch input. Using unity UI events in inspector.
        public void Jump()
        {
            if (canJump)
            {
                //HARDCODED checkJump number of frames to delay.
                checkJumpDelay = 3;

                _runAudioSource.Stop();
                _glideAudioSource.Stop();
                _audioSource.PlayOneShot(jumpSound);

                _animator.Play(Animator.StringToHash("PlayerJump"));
                _velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
                _controller.move(_velocity * Time.deltaTime);

                canGlide = true;

                switch (GameManager_Master.gameState)
                {
                    case GameState.Menu:
                        break;
                    case GameState.Playing:
                        jumpsThisLife++;
                        break;
                    case GameState.Paused:
                        break;
                    case GameState.GameOver:
                        break;
                    default:
                        break;
                }

                return;
            }

            if (canGlide && !canJump)
            //if (!canJump && !amGliding)
            {
                if (_velocity.y < startGlideVelocity)
                {
                    if (!GameManager_Master.halfGlideGravity)
                    {
                        currentGravity = glideGravity;
                    }
                    else
                    {
                        currentGravity = glideGravity / 2;
                    }

                    _velocity.y = 0f;
                    canGlide = false;
                    amGliding = true;

                    //_audioSource.Stop();
                    _runAudioSource.Stop();
                    _glideAudioSource.Play();

                    _animator.Play(Animator.StringToHash("PlayerJumpGlideTransition"));

                    switch (GameManager_Master.gameState)
                    {
                        case GameState.Menu:
                            break;
                        case GameState.Playing:
                            glidesThisLife++;
                            break;
                        case GameState.Paused:
                            break;
                        case GameState.GameOver:
                            break;
                        default:
                            break;
                    }
                }          
            }      
        }

        //Own method for controlling with mouse clicks or touch input. Using unity UI events in inspector.
        public void EndGlide()
        {
            if (amGliding)
            {
                currentGravity = gravity;
                amGliding = false;
                _glideAudioSource.Stop();
                _animator.Play(Animator.StringToHash("PlayerRun"));
            }
        }

#endregion

        //To subscribe to resetorigin event
        private void OriginReset()
        {
            switch (GameManager_Master.gameState)
            {
                case GameState.Menu:
                    //moves player back.
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - NewWorldGen.originResetDistance, gameObject.transform.position.y, 0);

                    break;

                case GameState.Playing:
                    numberOriginResets++;
                    //moves player back.
                    gameObject.transform.position = new Vector3(gameObject.transform.position.x - NewWorldGen.originResetDistance, gameObject.transform.position.y, 0);

                    break;

                case GameState.Paused:
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }
            

        }

        private void Death()
        {
            timesDied++;

            GameManager_Master.gameState = GameState.GameOver;
            gameManagerMaster.CallEventInputToggle();
            gameManagerMaster.CallEventToggleGamePlayUI();

            _animator.Play(Animator.StringToHash("PlayerDeath"));
            StartCoroutine(DeathAnimation());
        }

        //This is public and a method so it can be called on button clicks if the player has a high score and quits without dying.
        public void WriteHighScore()
        {          
            NewSaveGame.Instance.achievementSave[(int)Achievements.DeathsTotal, 0] += timesDied;           
            NewSaveGame.Instance.AddTimePlayed(Mathf.RoundToInt(timePlayed));
            NewSaveGame.Instance.HandlePagesCollectedAchievement(pagesCollected);
            NewSaveGame.Instance.AddJumps(jumpsThisLife);
            NewSaveGame.Instance.AddGlides(glidesThisLife);
            NewSaveGame.Instance.AddSuperJumpWhileGliding(superJumpWhileGliding);

            gameManagerMaster.CallEventWriteHighScore(GameManager_Master.score);

#if UNITY_ANDROID
            AchievmentAndLeaderboardHandler.ReportLeaderboard(GameManager_Master.score, PlayServices.leaderboard_high_score);
            AchievmentAndLeaderboardHandler.ReportLeaderboard(pagesCollected, PlayServices.leaderboard_pages_collected_in_1_run);

            achievReporter.ReportScoreAchievementAndroid(GameManager_Master.score);
            achievReporter.ReportJumpsAchievementAndroid();
            achievReporter.ReportGlidesAchievementAndroid();
            achievReporter.ReportGlidingSuperJumpAchievementAndroid();
#elif UNITY_IOS
            AchievmentAndLeaderboardHandler.ReportLeaderboard(GameManager_Master.score, IosAchievementIDs.ios_leaderboard_high_score);
            AchievmentAndLeaderboardHandler.ReportLeaderboard(pagesCollected, IosAchievementIDs.ios_leaderboard_pages_collected_in_1_run);

            achievReporter.ReportScoreAchievementIOS(GameManager_Master.score);
            achievReporter.ReportJumpsAchievementIos();
            achievReporter.ReportGlidesAchievementIos();
            achievReporter.ReportGlidingSuperJumpAchievementIos();
#endif
        }

        public void PagesRespawn()
        {
            //HARDCODED Page revive cost
            NewSaveGame.Instance.pages -= 10;
            NewSaveGame.Instance.Save();

            //Enable revived canvas after paying pages.
            gameOverCanReviveCanvas.SetActive(false);
            revivedCanvas.SetActive(true);
            GameManager_Master.canRevivePages = false;
            RespawnPlayer();
        }

        public void AdRespawn()
        {
#if UNITY_ANDROID || UNITY_IOS
            if (Advertisement.IsReady("rewardedVideo"))
            {
                var options = new ShowOptions();
                options.resultCallback = HandleShowResult;

                Advertisement.Show("rewardedVideo", options);
            }
            else
            {
                gameOverCanReviveCanvas.SetActive(false);
                revivedCanvas.SetActive(true);
                GameManager_Master.canRevive = false;
                RespawnPlayer();
            }
#else
            gameOverCanReviveCanvas.SetActive(false);
            revivedCanvas.SetActive(true);
            GameManager_Master.canRevive = false;
            RespawnPlayer();
#endif
        }

#if UNITY_ANDROID || UNITY_IOS
        void HandleShowResult(ShowResult result)
        {
            if (result == ShowResult.Finished)
            {
                //Enable revived canvas after successful ad.
                gameOverCanReviveCanvas.SetActive(false);
                revivedCanvas.SetActive(true);
                GameManager_Master.canRevive = false;
                RespawnPlayer();
            }
            else if (result == ShowResult.Skipped)
            {
                gameManagerMaster.GetComponentInChildren<UIConfirmPopup>().ShowPopup("Ad skipped", " Confirm ");
            }
            else if (result == ShowResult.Failed)
            {
                gameManagerMaster.GetComponentInChildren<UIConfirmPopup>().ShowPopup("Ad failed", " Confirm ");
            }
        }
#endif

        private void RespawnPlayer()
        {
            gameObject.transform.position = new Vector3(platformToReviveOn.bounds.min.x + gameObject.GetComponent<Collider2D>().bounds.extents.x + 0.15f, 1f, 0);

            _animator.Play(Animator.StringToHash("PlayerRun"));
            _animator.SetFloat("RunMultiplier", 0);

            gameManagerMaster.CallEventResetMovingPlatforms();

            touchHeld = false;
        }

        private IEnumerator DeathAnimation()
        {
            //Saves pages collected on every death. Probably need to figure out good way to save everything, but still track for achievements.
            NewSaveGame.Instance.Save();

            yield return new WaitForSeconds(GetAnimation("PlayerDeath").length - 0.05f);

            timePlayed += Time.unscaledTime - startTime;

            //gameManagerMaster.CallEventPauseToggle();

            if (!GameManager_Master.canRevive && !GameManager_Master.canRevivePages)
            {
                WriteHighScore();
            }

            //Gameover stats panel update.
            gameoverScoreText.text = GameManager_Master.score.ToString();
            gameoverPagesText.text = pagesCollected.ToString();

            string timePlayedStat;
            if (timePlayed % 60 < 10)
            {
                timePlayedStat = "0" + ((int)(timePlayed % 60)).ToString();
            }
            else timePlayedStat = ((int)(timePlayed % 60)).ToString();

            gameoverTimePlayedText.text = ((int)(timePlayed / 60)).ToString() + ":" + timePlayedStat;
            gameoverJumpsText.text = jumpsThisLife.ToString();
            gameoverGlidesText.text = glidesThisLife.ToString();

            gameManagerMaster.CallEventGameOver();
        }

        private AnimationClip GetAnimation(string name)
        {
            for (int i = 0; i < _animator.runtimeAnimatorController.animationClips.Length; i++)
            {
                if (_animator.runtimeAnimatorController.animationClips[i].name == name)
                {
                    return _animator.runtimeAnimatorController.animationClips[i];
                }
            }
            Debug.LogError("Animation clip: " + name + " not found");
            return null;
        }

        public void StartAfterRespawn()
        {
            runSpeed = runSpeedUnModified;
            gameManagerMaster.CallEventInputToggle();
            revivedCanvas.SetActive(false);
            gameManagerMaster.CallEventToggleGamePlayUI();
            _animator.SetFloat("RunMultiplier", 1);

            startTime = Time.unscaledTime;

            GameManager_Master.gameState = GameState.Playing;
        }

        private IEnumerator StartDelay()
        {
            yield return StartCoroutine(gameManagerMaster.GetComponent<GameManager_PlayPressed>().CameraOffset(playingCameraXOffset));

            startDelayActive = false;
            runSpeed = 2f;
            StartCoroutine(PlayerSpeedBuildUp());
        }

        private IEnumerator PlayerSpeedBuildUp()
        {  
            while (runSpeed < runSpeedUnModified)
            {
                runSpeed += 3f * Time.deltaTime;
                _animator.SetFloat("RunMultiplier", runSpeed / runSpeedUnModified);
                yield return null;
            }
            runSpeed = runSpeedUnModified;
            _animator.SetFloat("RunMultiplier", 1f);
            yield return new WaitForEndOfFrame();
        }
    }
}

