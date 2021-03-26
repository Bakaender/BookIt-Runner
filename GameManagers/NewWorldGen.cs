using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BookIt
{
    enum SpawnDifficulty
    {
        Casual,
        Easy,
        Medium,
        Hard,
        BookItPro
    }

    public class NewWorldGen : MonoBehaviour
    {
        [Header("------------------Patch 1.1 Stuff------------")]
        public float backgroundFadeTime = 45f;
        [Range(1, 7)]
        public float fadeDuration = 2.5f;
        private float fadeTime;

        public const float originResetDistance = 9900f;
        private List<GameObject> platforms;

        private GameManager_Master gameManagerMaster;

        [Header("premium Currency")]
        public GameObject premiumCurrency;
        [Range(0, 100)]
        public int chanceToSpawnPremium;

        [Tooltip("Element 0 = easy, 1 = medium, 2 = hard")]
        public int[] weightedPremiumSpawnDif = new int[3];

        [Header("Double premium Currency")]
        public GameObject doublePremiumCurrency;
        [Range(0, 100)]
        public int chanceToSpawnDoublePremium;

        private bool spawnCurrency;
        private bool spawnDoubleCurrency;

        [Header("Min and Max distance possible between 2 platforms")]
        public float minJumpDistance;
        public float maxJumpDistance;

        [Header("Location of last manually placed level object.")]
        [Tooltip("Random object spawning will start after this object, keeping same bottom Y value.")]
        public GameObject lastManuallyPlacedObject;

        private Vector3 nextSpawnLocation;

        [Header("Number of random objects to spawn ahead of the player")]
        public int numberOfObjectsToSpawn = 10;

        [Header("------------------EXTRA NEW------------")]
        [Header("------------------TEST STUFF------------")]
        public bool testMode = false;
        public GameObject testPlatform;
     
        [Header("------------------RELEASE STUFF------------")]
        public GameObject[] casualPlatforms = new GameObject[1];
        public GameObject[] easyPlatforms = new GameObject[1];
        public GameObject[] mediumPlatforms = new GameObject[1];
        public GameObject[] hardPlatforms = new GameObject[1];
        public GameObject[] proPlatforms = new GameObject[1];

        private int totalNumberCasual;
        private int totalNumberEasy;
        private int totalNumberMedium;
        private int totalNumberHard;
        private int totalNumberBookItPro;

        private GameObject[] platformPool;
        private PlatformComponentLookupScript[] platformPoolsComponents;

        private bool firstSpawn;

        [Header("weighted chance to randomly select object difficulty to spawn after the last spawned platform.")]
        [Header("0 = casual, 1 = easy, 2 = medium, 3 = hard, 4 = pro")]
        [SerializeField]
        private int[] spawnDifChance = new int[5];

        //If add more dif increase distances need to also add to difficultyincrease method.
        [Header("Increase Difficulty after this distance.")]
        [Header("Can be duplicated as many times as want.")]
        public float dif1IncreaseDistance = 100;
        [Header("This is simply added to spawnDifChance everytime X distance is crossed")]
        public int[] dif1IncreaseAmmount = new int[5];
        private int dif1TimesIncreased;
        [Header("Stop using this increase after this distance. 0 never stops.")]
        public int dif1Cutout;

        [Header("Increase Difficulty after this distance")]
        public float dif2IncreaseDistance = 1000;
        [Header("This is simply added to spawnDifChance everytime X distance is crossed")]
        public int[] dif2IncreaseAmmount = new int[5];
        private int dif2TimesIncreased;
        [Header("Stop using this increase after this distance. 0 never stops.")]
        public int dif2Cutout;

        [Header("Increase Difficulty after this distance")]
        public float dif3IncreaseDistance = 1000;
        [Header("This is simply added to spawnDifChance everytime X distance is crossed")]
        public int[] dif3IncreaseAmmount = new int[5];
        private int dif3TimesIncreased;
        [Header("Stop using this increase after this distance. 0 never stops.")]
        public int dif3Cutout;

        [Header("Increase Difficulty after this distance")]
        public float dif4IncreaseDistance = 1000;
        [Header("This is simply added to spawnDifChance everytime X distance is crossed")]
        public int[] dif4IncreaseAmmount = new int[5];
        private int dif4TimesIncreased;
        [Header("Stop using this increase after this distance. 0 never stops.")]
        public int dif4Cutout;

        private SpawnDifficulty nextSpawnDif;

        private int nextSpawn;
        private int lastSpawn;

        private void Awake()
        {
            totalNumberCasual = casualPlatforms.Length;
            totalNumberEasy = easyPlatforms.Length;
            totalNumberMedium = mediumPlatforms.Length;
            totalNumberHard = hardPlatforms.Length;
            totalNumberBookItPro = proPlatforms.Length;

            platformPool = new GameObject[totalNumberCasual + totalNumberEasy + totalNumberMedium + totalNumberHard + totalNumberBookItPro];
            platformPoolsComponents = new PlatformComponentLookupScript[platformPool.Length];

            GameObject spawnedPlatform;
            int index = 0;
            for (int i = 0; i < casualPlatforms.Length; i++)
            {
                spawnedPlatform = Instantiate(casualPlatforms[i], Vector3.zero, Quaternion.identity);
                spawnedPlatform.SetActive(false);
                platformPool[index] = spawnedPlatform;
                platformPoolsComponents[index] = spawnedPlatform.GetComponent<PlatformComponentLookupScript>();
                platformPoolsComponents[index].poolIndex = index;

                index++;
            }
            for (int i = 0; i < easyPlatforms.Length; i++)
            {
                spawnedPlatform = Instantiate(easyPlatforms[i], Vector3.zero, Quaternion.identity);
                spawnedPlatform.SetActive(false);
                platformPool[index] = spawnedPlatform;
                platformPoolsComponents[index] = spawnedPlatform.GetComponent<PlatformComponentLookupScript>();
                platformPoolsComponents[index].poolIndex = index;

                index++;
            }
            for (int i = 0; i < mediumPlatforms.Length; i++)
            {
                spawnedPlatform = Instantiate(mediumPlatforms[i], Vector3.zero, Quaternion.identity);
                spawnedPlatform.SetActive(false);
                platformPool[index] = spawnedPlatform;
                platformPoolsComponents[index] = spawnedPlatform.GetComponent<PlatformComponentLookupScript>();
                platformPoolsComponents[index].poolIndex = index;

                index++;
            }
            for (int i = 0; i < hardPlatforms.Length; i++)
            {
                spawnedPlatform = Instantiate(hardPlatforms[i], Vector3.zero, Quaternion.identity);
                spawnedPlatform.SetActive(false);
                platformPool[index] = spawnedPlatform;
                platformPoolsComponents[index] = spawnedPlatform.GetComponent<PlatformComponentLookupScript>();
                platformPoolsComponents[index].poolIndex = index;

                index++;
            }
            for (int i = 0; i < proPlatforms.Length; i++)
            {
                spawnedPlatform = Instantiate(proPlatforms[i], Vector3.zero, Quaternion.identity);
                spawnedPlatform.SetActive(false);
                platformPool[index] = spawnedPlatform;
                platformPoolsComponents[index] = spawnedPlatform.GetComponent<PlatformComponentLookupScript>();
                platformPoolsComponents[index].poolIndex = index;

                index++;
            }

            firstSpawn = true;

            //1.1
            fadeTime = Time.time + backgroundFadeTime;
            GameManager_Master.fadeDuration = fadeDuration;
        }

        private void OnEnable()
        {
            gameManagerMaster = GetComponentInParent<GameManager_Master>();
            if (gameManagerMaster == null)
            {
                Debug.Log("Failed to get gameManager_Master from parent object. WorldGeneration - OnEnable.");
            }

            gameManagerMaster.ResetToOriginEvent += ResetToOrigin;

            platforms = new List<GameObject>();
        }

        private void OnDisable()
        {
            gameManagerMaster.ResetToOriginEvent -= ResetToOrigin;
        }

        void Start()
        {
            spawnCurrency = false;
            spawnDoubleCurrency = false;

            nextSpawnDif = SpawnDifficulty.Casual;

            dif1TimesIncreased = 0;
            dif2TimesIncreased = 0;
        }

        void Update()
        {
            if (Time.time >= fadeTime)
            {
                gameManagerMaster.CallEventCycleBackground();
                fadeTime += backgroundFadeTime;
            }

            switch (GameManager_Master.gameState)
            {
                case GameState.Menu:
                    break;

                case GameState.Playing:
                    if (numberOfObjectsToSpawn > 0)
                    {
                        if (!testMode)
                        {
                            switch (nextSpawnDif)
                            {
                                case SpawnDifficulty.Casual:
                                    nextSpawn = Random.Range(0, totalNumberCasual);
                                    if (platformPool[nextSpawn].activeSelf == true)
                                    {
                                        //Already spawned in the world so need different one.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        break;
                                    }
                                    else
                                    {
                                        //Do we spawn premium currency with this platform spawn
                                        spawnCurrency = Random.Range(0, 100) <= chanceToSpawnPremium ? true : false;
                                        spawnDoubleCurrency = Random.Range(0, 100) <= chanceToSpawnDoublePremium ? true : false;

                                        //Get next spawn X position
                                        if (!firstSpawn)
                                        {
                                            nextSpawnLocation.x = platformPoolsComponents[lastSpawn].platformCollider.bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;
                                        }
                                        else
                                        {
                                            nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;

                                            firstSpawn = false;
                                        }

                                        //Get next spawn Y position
                                        nextSpawnLocation.y = platformPoolsComponents[nextSpawn].platformCollider.size.y / 2 * platformPool[nextSpawn].transform.localScale.y;

                                        //set position and activate.
                                        platformPool[nextSpawn].transform.position = nextSpawnLocation;
                                        platformPool[nextSpawn].SetActive(true);

                                        //Picks a random Dif to be spawned next based on weights.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        numberOfObjectsToSpawn--;
                                        lastSpawn = nextSpawn;
                                        platforms.Add(platformPool[lastSpawn]);
                                    }

                                    break;

                                case SpawnDifficulty.Easy:
                                    nextSpawn = Random.Range(totalNumberCasual, totalNumberCasual + totalNumberEasy);
                                    if (platformPool[nextSpawn].activeSelf == true)
                                    {
                                        //Already spawned in the world so need different one.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        break;
                                    }
                                    else
                                    {
                                        //Do we spawn premium currency with this platform spawn
                                        spawnCurrency = Random.Range(0, 100) <= chanceToSpawnPremium ? true : false;
                                        spawnDoubleCurrency = Random.Range(0, 100) <= chanceToSpawnDoublePremium ? true : false;

                                        //Get next spawn X position
                                        if (!firstSpawn)
                                        {
                                            nextSpawnLocation.x = platformPoolsComponents[lastSpawn].platformCollider.bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;    
                                        }
                                        else
                                        {
                                            nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x; 

                                            firstSpawn = false;
                                        }

                                        //Get next spawn Y position
                                        nextSpawnLocation.y = platformPoolsComponents[nextSpawn].platformCollider.size.y / 2 * platformPool[nextSpawn].transform.localScale.y;

                                        //set position and activate.
                                        platformPool[nextSpawn].transform.position = nextSpawnLocation;
                                        platformPool[nextSpawn].SetActive(true);

                                        //Picks a random Dif to be spawned next based on weights.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        numberOfObjectsToSpawn--;
                                        lastSpawn = nextSpawn;
                                        platforms.Add(platformPool[lastSpawn]);
                                    }

                                    break;

                                case SpawnDifficulty.Medium:
                                    nextSpawn = Random.Range(totalNumberCasual + totalNumberEasy, totalNumberCasual + totalNumberEasy + totalNumberMedium);
                                    if (platformPool[nextSpawn].activeSelf == true)
                                    {
                                        //Already spawned in the world so need different one.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        break;
                                    }
                                    else
                                    {
                                        //Do we spawn premium currency with this platform spawn
                                        spawnCurrency = Random.Range(0, 100) <= chanceToSpawnPremium ? true : false;
                                        spawnDoubleCurrency = Random.Range(0, 100) <= chanceToSpawnDoublePremium ? true : false;

                                        //Get next spawn X position
                                        if (!firstSpawn)
                                        {
                                            nextSpawnLocation.x = platformPoolsComponents[lastSpawn].platformCollider.bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;
                                        }
                                        else
                                        {
                                            nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x; 

                                            firstSpawn = false;
                                        }

                                        //Get next spawn Y position
                                        nextSpawnLocation.y = platformPoolsComponents[nextSpawn].platformCollider.size.y / 2 * platformPool[nextSpawn].transform.localScale.y;

                                        //set position and activate.
                                        platformPool[nextSpawn].transform.position = nextSpawnLocation;
                                        platformPool[nextSpawn].SetActive(true);

                                        //Picks a random Dif to be spawned next based on weights.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        numberOfObjectsToSpawn--;
                                        lastSpawn = nextSpawn;
                                        platforms.Add(platformPool[lastSpawn]);
                                    }

                                    break;

                                case SpawnDifficulty.Hard:
                                    nextSpawn = Random.Range(totalNumberCasual + totalNumberEasy + totalNumberMedium, totalNumberCasual + totalNumberEasy + totalNumberMedium + totalNumberHard);
                                    if (platformPool[nextSpawn].activeSelf == true)
                                    {
                                        //Already spawned in the world so need different one.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        break;
                                    }
                                    else
                                    {
                                        //Do we spawn premium currency with this platform spawn
                                        spawnCurrency = Random.Range(0, 100) <= chanceToSpawnPremium ? true : false;
                                        spawnDoubleCurrency = Random.Range(0, 100) <= chanceToSpawnDoublePremium ? true : false;

                                        //Get next spawn X position
                                        if (!firstSpawn)
                                        {
                                            nextSpawnLocation.x = platformPoolsComponents[lastSpawn].platformCollider.bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;
                                        }
                                        else
                                        {
                                            nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;

                                            firstSpawn = false;
                                        }

                                        //Get next spawn Y position
                                        nextSpawnLocation.y = platformPoolsComponents[nextSpawn].platformCollider.size.y / 2 * platformPool[nextSpawn].transform.localScale.y;

                                        //set position and activate.
                                        platformPool[nextSpawn].transform.position = nextSpawnLocation;
                                        platformPool[nextSpawn].SetActive(true);

                                        //Picks a random Dif to be spawned next based on weights.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        numberOfObjectsToSpawn--;
                                        lastSpawn = nextSpawn;
                                        platforms.Add(platformPool[lastSpawn]);
                                    }

                                    break;

                                case SpawnDifficulty.BookItPro:
                                    nextSpawn = Random.Range(totalNumberCasual + totalNumberEasy + totalNumberMedium + totalNumberHard, platformPool.Length);
                                    if (platformPool[nextSpawn].activeSelf == true)
                                    {
                                        //Already spawned in the world so need different one.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        break;
                                    }
                                    else
                                    {
                                        //Do we spawn premium currency with this platform spawn
                                        spawnCurrency = Random.Range(0, 100) <= chanceToSpawnPremium ? true : false;
                                        spawnDoubleCurrency = Random.Range(0, 100) <= chanceToSpawnDoublePremium ? true : false;

                                        //Get next spawn X position
                                        if (!firstSpawn)
                                        {
                                            nextSpawnLocation.x = platformPoolsComponents[lastSpawn].platformCollider.bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;
                                        }
                                        else
                                        {
                                            nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                                Random.Range(minJumpDistance, maxJumpDistance) +
                                                platformPoolsComponents[nextSpawn].platformCollider.size.x / 2 * platformPool[nextSpawn].transform.localScale.x;

                                            firstSpawn = false;
                                        }

                                        //Get next spawn Y position
                                        nextSpawnLocation.y = platformPoolsComponents[nextSpawn].platformCollider.size.y / 2 * platformPool[nextSpawn].transform.localScale.y;

                                        //set position and activate.
                                        platformPool[nextSpawn].transform.position = nextSpawnLocation;
                                        platformPool[nextSpawn].SetActive(true);

                                        //Picks a random Dif to be spawned next based on weights.
                                        nextSpawnDif = (SpawnDifficulty)WeightedRandom(spawnDifChance);
                                        numberOfObjectsToSpawn--;
                                        lastSpawn = nextSpawn;
                                        platforms.Add(platformPool[lastSpawn]);
                                    }

                                    break;

                                default:
                                    break;
                            }
                        }
                        else
                        {
                            spawnCurrency = true;
                            spawnDoubleCurrency = true;

                            if (testPlatform.GetComponent<BoxCollider2D>().offset.x == 0)
                            {
                                nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                       Random.Range(minJumpDistance, maxJumpDistance) +
                                       testPlatform.GetComponent<BoxCollider2D>().size.x / 2 * testPlatform.transform.localScale.x;
                            }
                            else
                            {
                                nextSpawnLocation.x = lastManuallyPlacedObject.GetComponent<BoxCollider2D>().bounds.max.x +
                                Random.Range(minJumpDistance, maxJumpDistance);
                            }

                            nextSpawnLocation.y = testPlatform.GetComponent<BoxCollider2D>().size.y / 2 * testPlatform.transform.localScale.y;

                            //Spawns the object and stores it so its location can be used for the next object.
                            lastManuallyPlacedObject = Instantiate(testPlatform, nextSpawnLocation, Quaternion.identity);
                            AddObjectToPlatformList(lastManuallyPlacedObject);
                            numberOfObjectsToSpawn--;
                        }


                        if (spawnCurrency && premiumCurrency != null)
                        {
                            if (!testMode)
                            {
                                int spawnDif = WeightedRandom(weightedPremiumSpawnDif);

                                if (spawnDif == 0)
                                {
                                    if (platformPoolsComponents[nextSpawn].easyPagePosition != null)
                                    {
                                        platformPoolsComponents[lastSpawn].premiumCurrency = Instantiate(premiumCurrency, platformPoolsComponents[nextSpawn].easyPagePosition.transform.position, Quaternion.identity);
                                    }
                                }
                                else if (spawnDif == 1)
                                {
                                    if (platformPoolsComponents[nextSpawn].mediumPagePosition != null)
                                    {
                                        platformPoolsComponents[lastSpawn].premiumCurrency = Instantiate(premiumCurrency, platformPoolsComponents[nextSpawn].mediumPagePosition.transform.position, Quaternion.identity);
                                    }
                                }
                                else
                                {
                                    if (platformPoolsComponents[nextSpawn].hardPagePosition != null)
                                    {
                                        platformPoolsComponents[lastSpawn].premiumCurrency = Instantiate(premiumCurrency, platformPoolsComponents[nextSpawn].hardPagePosition.transform.position, Quaternion.identity);
                                    }
                                }
                            }
                            else
                            {
                                if (spawnCurrency && premiumCurrency != null)
                                {
                                    int spawnDif = WeightedRandom(weightedPremiumSpawnDif);

                                    if (spawnDif == 0)
                                    {
                                        if (lastManuallyPlacedObject.transform.Find("easyspawn") != null)
                                        {
                                            Instantiate(premiumCurrency, lastManuallyPlacedObject.transform.Find("easyspawn").transform.position, Quaternion.identity).transform.SetParent(lastManuallyPlacedObject.transform);
                                        }
                                    }
                                    else if (spawnDif == 1)
                                    {
                                        if (lastManuallyPlacedObject.transform.Find("mediumspawn") != null)
                                        {
                                            Instantiate(premiumCurrency, lastManuallyPlacedObject.transform.Find("mediumspawn").transform.position, Quaternion.identity).transform.SetParent(lastManuallyPlacedObject.transform);
                                        }
                                    }
                                    else
                                    {
                                        if (lastManuallyPlacedObject.transform.Find("hardspawn") != null)
                                        {
                                            Instantiate(premiumCurrency, lastManuallyPlacedObject.transform.Find("hardspawn").transform.position, Quaternion.identity).transform.SetParent(lastManuallyPlacedObject.transform);
                                        }
                                    }
                                }
                            }

                            spawnCurrency = false;
                        }

                        if (spawnDoubleCurrency && doublePremiumCurrency != null)
                        {
                            if (!testMode)
                            {
                                if (platformPoolsComponents[nextSpawn].extremePagePosition != null)
                                {
                                    platformPoolsComponents[lastSpawn].premiumDoubleCurrency = Instantiate(doublePremiumCurrency, platformPoolsComponents[nextSpawn].extremePagePosition.transform.position, Quaternion.identity);
                                }
                            }
                            else
                            {
                                if (lastManuallyPlacedObject.transform.Find("extremespawn") != null)
                                {
                                    Instantiate(doublePremiumCurrency, lastManuallyPlacedObject.transform.Find("extremespawn").transform.position, Quaternion.identity).transform.SetParent(lastManuallyPlacedObject.transform);
                                }
                            }

                            spawnDoubleCurrency = false;
                        }
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

        private int WeightedRandom(int[] weights)
        {
            int weightTotal = 0;
            foreach (int w in weights)
            {
                weightTotal += w;
            }

            int result = 0, total = 0;
            int randVal = Random.Range(0, weightTotal);
            for (result = 0; result < weights.Length; result++)
            {
                total += weights[result];
                if (total > randVal)
                    break;
            }
            return result;
        }

        private void ResetToOrigin()
        {
            foreach (GameObject platform in platforms)
            {
                platform.transform.position = new Vector3(platform.transform.position.x - originResetDistance, platform.transform.position.y, 0);
            }
        }

        public void RemoveObjectFromObjectList(GameObject objectToRemove)
        {
            platforms.Remove(objectToRemove);

            if (!testMode)
            {
                if (!objectToRemove.CompareTag("AIPlatform"))
                {
                    objectToRemove.SetActive(false);

                    if (objectToRemove.GetComponent<PlatformComponentLookupScript>().premiumCurrency != null)
                    {
                        Destroy(objectToRemove.GetComponent<PlatformComponentLookupScript>().premiumCurrency);
                    }
                    if (objectToRemove.GetComponent<PlatformComponentLookupScript>().premiumDoubleCurrency != null)
                    {
                        Destroy(objectToRemove.GetComponent<PlatformComponentLookupScript>().premiumDoubleCurrency);
                    }
                }
                else
                {
                    Destroy(objectToRemove);
                }
            }
            else
            {
                Destroy(objectToRemove);
            }
        }

        public void AddObjectToPlatformList(GameObject objectToAdd)
        {
            platforms.Add(objectToAdd);
        }

        public void IncreaseDifficulty()
        {
            if (GameManager_Master.score - dif1TimesIncreased * dif1IncreaseDistance >= dif1IncreaseDistance)
            {
                if (GameManager_Master.score < dif1Cutout || dif1Cutout == 0)
                {
                    for (int i = 0; i < spawnDifChance.Length; i++)
                    {
                        spawnDifChance[i] += dif1IncreaseAmmount[i];
                        if (spawnDifChance[i] < 0)
                        {
                            spawnDifChance[i] = 0;
                        }
                    }
                    dif1TimesIncreased++;
                }   
            }

            if (GameManager_Master.score - dif2TimesIncreased * dif2IncreaseDistance >= dif2IncreaseDistance)
            {
                if (GameManager_Master.score < dif2Cutout || dif2Cutout == 0)
                {
                    for (int i = 0; i < spawnDifChance.Length; i++)
                    {
                        spawnDifChance[i] += dif2IncreaseAmmount[i];
                        if (spawnDifChance[i] < 0)
                        {
                            spawnDifChance[i] = 0;
                        }
                    }
                    dif2TimesIncreased++;
                }  
            }

            if (GameManager_Master.score - dif3TimesIncreased * dif3IncreaseDistance >= dif3IncreaseDistance)
            {
                if (GameManager_Master.score < dif3Cutout || dif3Cutout == 0)
                {
                    for (int i = 0; i < spawnDifChance.Length; i++)
                    {
                        spawnDifChance[i] += dif3IncreaseAmmount[i];
                        if (spawnDifChance[i] < 0)
                        {
                            spawnDifChance[i] = 0;
                        }
                    }
                    dif3TimesIncreased++;
                }
            }

            if (GameManager_Master.score - dif4TimesIncreased * dif4IncreaseDistance >= dif4IncreaseDistance)
            {
                if (GameManager_Master.score < dif4Cutout || dif4Cutout == 0)
                {
                    for (int i = 0; i < spawnDifChance.Length; i++)
                    {
                        spawnDifChance[i] += dif4IncreaseAmmount[i];
                        if (spawnDifChance[i] < 0)
                        {
                            spawnDifChance[i] = 0;
                        }
                    }
                    dif4TimesIncreased++;
                }
            }
        }   
    }
}