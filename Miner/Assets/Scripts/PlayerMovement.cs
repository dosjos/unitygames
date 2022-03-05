using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Actions action;
    public int money = 1500;
    public int healt = 100;
    [SerializeField]
    private float speed = 5;
    [SerializeField]
    private Transform movePoint;
    [SerializeField]
    private LayerMask obstacleMask;
    [SerializeField]
    private Tilemap tileMap;
    public Tilemap foreground;
    public Tilemap removables;
    public LayerMask removableMask;

    public float ChanceOfFinding = 0.2f;

    public Tile dirt;
    public Tile diamond;
    public Tile gold;
    public Tile silver;
    public Tile platinum;
    public Tile rock;
    public Tile waterSpout;
    public Tile water;
    public Tile find;
    public Tile sand;
    public Tile sand2;
    public Tile caveIn;
    public Tile clover;
    public Tile ringTile;

    public int goldNumber;
    public int silverNumber;
    public int platiniumNumber;
    public int diamondNumber;

    public bool havePicaxe;
    public bool haveShovel;
    public bool haveTorch;
    public bool haveLantern;
    public bool haveDynamite;
    public bool haveClover;
    public bool haveBucket;
    public bool haveDrill;
    public bool haveRing;
    public bool haveCondom;

    public Text GoldText;
    public Text SilverText;
    public Text PlatinumText;
    public Text DiamondText;
    public Text bankNumber;
    public Text healthNumber;
    public Text silverPrice;
    public Text goldPrice;
    public Text platinumPrice;
    public Text diamondPrice;
    public Text historyText;
    public Text hospitalDaysText;
    public Text MimiTalk;
    public Text MimiPropose;

    public Image picAxe;
    public Image shovel;
    public Image torch;
    public Image lantern;
    public Image dynamiteImage;
    public Image cloverImage;
    public Image bucket;
    public Image drill;
    public Image ring;
    public Image condom;


    public Camera camera;
    public GameObject Bank;
    public GameObject Store;
    public GameObject Hospital;
    public GameObject Help;
    public GameObject Brothel;
    public GameObject GameOverMoney;
    public GameObject GameOverHeatlh;
    public GameObject GameOverMary;
    public GameObject Dynamite;
    private Dynamite dynamiteScript;

    private Vector3 LastPosition;
    private int cloversFound = 0;
    private bool doingDamage;
    private float timeSinceDamage;
    private SpriteRenderer playerSprite;
    private bool direction = false;

    private Mode mode;

    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        mode = Mode.Game;
        movePoint.parent = null; // Detach partent
        LastPosition = movePoint.position;
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        dynamiteScript = Dynamite.GetComponent<Dynamite>();
        Bank.SetActive(false);
        Store.SetActive(false);
        Hospital.SetActive(false);
        Brothel.SetActive(false);
        Help.SetActive(true);
        GameOverMoney.SetActive(false);
        GameOverHeatlh.SetActive(false);
        GameOverMary.SetActive(false);
        MimiPropose.enabled = false;
        MimiTalk.enabled = false;

        mode = Mode.Help;
    }

    void Update()
    {
        GoldText.text = goldNumber.ToString();
        SilverText.text = silverNumber.ToString();
        PlatinumText.text = platiniumNumber.ToString();
        DiamondText.text = diamondNumber.ToString();
        bankNumber.text = "$" + money.ToString();
        healthNumber.text = healt.ToString();
        hospitalDaysText.text = ((100 - healt) / 10.0f).ToString();

        picAxe.enabled = havePicaxe;
        shovel.enabled = haveShovel;
        torch.enabled = haveTorch;
        lantern.enabled = haveLantern;
        dynamiteImage.enabled = haveDynamite;
        cloverImage.enabled = haveClover;
        bucket.enabled = haveBucket;
        drill.enabled = haveDrill;
        ring.enabled = haveRing;
        condom.enabled = haveCondom;
        playerSprite.flipX = direction;

        if (mode == Mode.Game)
        {
            GameLoop();
        }
        else if (mode == Mode.Store)
        {
            StoreLoop();
        }
        else if (mode == Mode.Bank)
        {
            BankLoop();
        }
        else if (mode == Mode.Hospital)
        {
            HospitalLoop();
        }
        else if (mode == Mode.Brothel)
        {
            BrothelLoop();
        }
        else if (mode == Mode.Help)
        {
            HelpLoop();
        }
        else if (mode == Mode.GameOver)
        {
            if (Input.anyKeyDown)
            {
                Application.Quit();
            }
        }
    }

    public void Detonate(Vector3 pos)
    {
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x-1, pos.y-1, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y-1, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x+1, pos.y-1, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x-1, pos.y, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x+1, pos.y, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x-1, pos.y+1, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y+1, 0)), null);
        tileMap.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x+1, pos.y+1, 0)), null);

        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x - 1, pos.y - 1, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y - 1, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x + 1, pos.y - 1, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x - 1, pos.y, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x + 1, pos.y, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x - 1, pos.y + 1, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y + 1, 0)), null);
        removables.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x + 1, pos.y + 1, 0)), null);

        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x - 1, pos.y - 1, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y - 1, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x + 1, pos.y - 1, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x - 1, pos.y, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x + 1, pos.y, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x - 1, pos.y + 1, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x, pos.y + 1, 0)), null);
        foreground.SetTile(Vector3Int.FloorToInt(new Vector3(pos.x + 1, pos.y + 1, 0)), null);
        var newPos = Vector3Int.FloorToInt(new Vector3(pos.x, pos.y, 0));
        if (newPos.x == 0 && newPos.y == 4)
        {
            foreground.SetTile(newPos, ringTile);
        }
        else {
            foreground.SetTile(newPos, gold);
        }
        var dist = Vector3.Distance(pos, transform.position);
        if(dist < 1.5f)
        {
            healt -= 30;
            SetText("Ouf that hurt, move further from the explosion", "red");
        }
    }

    private void BrothelLoop()
    {
        if (money >= 10000)
        {
            MimiTalk.enabled = true;
        }
        else
        {
            MimiTalk.enabled = false;
        }

        if (money >= 20000 && haveRing)
        {
            MimiPropose.enabled = true;
        }
        else
        {
            MimiPropose.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            mode = Mode.Game;
            Brothel.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.A) && money >= 10)
        {
            money -= 10;
            SetText("You drank a drink, you feel somewhat better", "blue");
        }
        if (Input.GetKeyDown(KeyCode.C) && money >= 300)
        {
            money -= 300;
            haveCondom = true;
            SetText("You bought a condom, now the fun can begin");
        }
        if (Input.GetKeyDown(KeyCode.E) && MimiTalk.enabled)
        {
            SetText("Hi honey, I want to marry a wealthy man, with at least $20.000, and my diamond ring that I lost down the outhouse");
        }
        if (Input.GetKeyDown(KeyCode.F) && MimiPropose.enabled)
        {
            if (haveRing)
            {
                SetText("I would love to marry you", "red");
                mode = Mode.GameOver;
                GameOverMary.SetActive(true);
            }
            else
            {
                SetText("You need to find my lost ring, search under the outhouse");
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            money -= 500;
            if (haveCondom)
            {
                SetText("That was fun");
                haveCondom=false;
            }
            else
            {
                healt -= 20;
                SetText("That was fun, but it hurts when I pee");
            }
        }
    }

    private void HospitalLoop()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            mode = Mode.Game;
            Hospital.SetActive(false);
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && healt >= 100)
        {
            SetText("Sorry, you are not sick", "red");
        }

        if (Input.GetKeyDown(KeyCode.A) && healt < 100)
        {
            var total = 0;
            for (int i = healt; i < 100; i += 10)
            {
                healt += 10;
                money -= 100;
                total += 100;
            }
            SetText($"You rested until completly healed, it only cost you ${total}", "red");
        }

        if (Input.GetKeyDown(KeyCode.D) && healt < 100)
        {
            healt += 10;
            money -= 100;
            SetText("You rested for one day, you feel better", "red");
        }
    }

    public void SetText(string text, string color = "white")
    {
        historyText.text = historyText.text + $"\n<color={color}>{text}</color>";
    }

    private void HelpLoop()
    {
        if (Input.anyKeyDown)
        {
            mode = Mode.Game;
            Help.SetActive(false);
        }
    }

    private void BankLoop()
    {
        if (Input.GetKeyDown(KeyCode.A) && silverNumber > 0)
        {
            var price = int.Parse(silverPrice.text.Replace("$", ""));
            money += price * silverNumber;
            SetText("Sold silver for " + (price * silverNumber));
            silverNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.B) && goldNumber > 0)
        {
            var price = int.Parse(goldPrice.text.Replace("$", ""));
            money += price * goldNumber;
            SetText("Sold gold for " + (price * goldNumber));
            goldNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.C) && platiniumNumber > 0)
        {
            var price = int.Parse(platinumPrice.text.Replace("$", ""));
            money += price * platiniumNumber;
            SetText("Sold platinium for " + (price * platiniumNumber));
            platiniumNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.D) && diamondNumber > 0)
        {
            money += 1000 * diamondNumber;
            SetText("Sold diamonds for " + (1000 * diamondNumber));
            diamondNumber = 0;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            mode = Mode.Game;
            Bank.SetActive(false);
        }
    }

    private void StoreLoop()
    {
        if (Input.GetKeyDown(KeyCode.A) && !havePicaxe && money >= 100)
        {
            money -= 100;
            havePicaxe = true;
            SetText("Bought a picaxe");
        }
        if (Input.GetKeyDown(KeyCode.B) && !haveShovel && money >= 150)
        {
            money -= 150;
            haveShovel = true;
            SetText("Bought a shovel");
        }
        if (Input.GetKeyDown(KeyCode.C) && !haveDrill && money >= 250)
        {
            money -= 250;
            haveDrill = true;
            SetText("Bought a drill");
        }
        if (Input.GetKeyDown(KeyCode.D) && !haveBucket && money >= 200)
        {
            money -= 200;
            haveBucket = true;
            SetText("Bought a bucket");
        }
        if (Input.GetKeyDown(KeyCode.E) && !haveTorch && money >= 100)
        {
            money -= 100;
            haveTorch = true;
            SetText("Bought a torch");
        }
        if (Input.GetKeyDown(KeyCode.F) && !haveLantern && money >= 300)
        {
            money -= 300;
            haveLantern = true;
            SetText("Bought a lantern");
        }
        if (Input.GetKeyDown(KeyCode.G) && !haveDynamite && money >= 300)
        {
            money -= 300;
            haveDynamite = true;
            SetText("Bought some dynamite, be carefull");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Store.SetActive(false);
            mode = Mode.Game;
        }
    }

    private void GameLoop()
    {
        if (money <= -100)
        {
            mode = Mode.GameOver;
            GameOverMoney.SetActive(true);
        }

        if (healt <= 0)
        {
            healt = 0;
            mode = Mode.GameOver;
            GameOverHeatlh.SetActive(true);
        }

        float movementAmout = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, movementAmout);

        if (timeSinceDamage > 0.5f)
        {
            doingDamage = false;
        }
        else
        {
            timeSinceDamage += Time.deltaTime;
        }


        var moved = false;

        if (Vector3.Distance(transform.position, movePoint.position) <= 0.05f)
        {
            var roundedPosition = Vector3Int.FloorToInt(transform.position);
            if (Input.GetKeyDown(KeyCode.E) && roundedPosition.y == 6 && roundedPosition.x == 14)
            {
                mode = Mode.Store;
                Store.SetActive(true);
                SetText("Time to shop");
            }
            if (Input.GetKeyDown(KeyCode.E) && roundedPosition.y == 6 && roundedPosition.x == 10)
            {
                silverPrice.text = "$" + Random.Range(10, 20);
                goldPrice.text = "$" + Random.Range(40, 60);
                platinumPrice.text = "$" + Random.Range(250, 300);
                diamondPrice.text = "$1000";
                Bank.SetActive(true);
                mode = Mode.Bank;
                SetText("Do you have something to sell?");
            }
            if (Input.GetKeyDown(KeyCode.E) && roundedPosition.y == 6 && roundedPosition.x == 6)
            {
                if (money < 5000)
                {
                    SetText("You must have at least $5000 to enter");
                }
                else
                {
                    SetText("You enteres Mimi's", "purple");
                    mode = Mode.Brothel;
                    Brothel.SetActive(true);
                }
            }
            if (Input.GetKeyDown(KeyCode.E) && roundedPosition.y == 6 && roundedPosition.x == 2)
            {
                mode = Mode.Hospital;
                Hospital.SetActive(true);
                SetText("Entered the doctors office", "red");
            }
            if (Input.GetKeyDown(KeyCode.E) && roundedPosition.y == 6 && roundedPosition.x == 0)
            {
                SetText("You feel relieved, and lighter...", "brown");
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                mode = Mode.Help;
                Help.SetActive(true);
                return;
            }


            if (Input.GetKeyDown(KeyCode.G) && haveDrill)
            {
                action = Actions.Drill;
            }
            if (Input.GetKeyDown(KeyCode.Y) && haveTorch && haveDynamite && transform.position.y < 5.0)
            {
                action = Actions.Ignite;
                dynamiteScript.SetDynamite(transform.position);
                haveDynamite = false;
            }
            if (Input.GetKeyDown(KeyCode.B) && haveBucket)
            {
                action |= Actions.Drain;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            var dir = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(dir) == 1f)
            {
                moved = Move(new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f));
                if (dir == 1f)
                {
                    direction = false;
                }
                if (dir == -1f)
                {
                    direction = true;
                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
            {
                moved = Move(new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f));
            }
        }
        var backUp = false;
        var hurt = false;

        var veInt = Vector3Int.FloorToInt(transform.position);
        var currentTile = tileMap.GetTile(veInt);
        if (currentTile == dirt)
        {
            tileMap.SetTile(veInt, null);
            (backUp, hurt) = CheckForFind(veInt);
            PicUpItems();
            foreground.SetTile(veInt, null);
            money -= (20 - (haveShovel && havePicaxe ? 12 : 0) - (havePicaxe && !haveShovel ? 9 : 0) - (!havePicaxe && haveShovel ? 9 : 0));
        }

        if (backUp)
        {
            movePoint.position = LastPosition;
            if (hurt)
            {
                healt -= (int)Random.Range(2, 8);
                LooseItems();
            }
        }

        if (!moved)
        {
            return;
        }

        var up = new Vector3(transform.position.x, transform.position.y + 1, 0f);
        var down = new Vector3(transform.position.x, transform.position.y - 1, 0f);
        var left = new Vector3(transform.position.x - 1, transform.position.y, 0f);
        var right = new Vector3(transform.position.x + 1, transform.position.y, 0f);

        if (tileMap.GetTile(Vector3Int.FloorToInt(up)) == dirt)
        {
            FoundSecret(CheckIfSIdeTileContains(), up);
        }
        if (tileMap.GetTile(Vector3Int.FloorToInt(down)) == dirt)
        {
            FoundSecret(CheckIfSIdeTileContains(), down);
        }
        if (tileMap.GetTile(Vector3Int.FloorToInt(left)) == dirt)
        {
            FoundSecret(CheckIfSIdeTileContains(), left);
        }
        if (tileMap.GetTile(Vector3Int.FloorToInt(right)) == dirt)
        {
            FoundSecret(CheckIfSIdeTileContains(), right);
        }

        PicUpItems();
    }

    private void FoundSecret(bool found, Vector3 pos)
    {
        if (!found) return;

        if (foreground.GetTile(Vector3Int.FloorToInt(pos)) == find)
        {
            CheckForFind(Vector3Int.FloorToInt(pos), true);
        }
        else if (foreground.HasTile(Vector3Int.FloorToInt(pos)) == false)
        {
            foreground.SetTile(Vector3Int.FloorToInt(pos), find);
        }
    }

    private bool CheckIfSIdeTileContains()
    {
        var findSomething = Random.value;

        float luckModifier = CalculateLuck();

        if (findSomething - (ChanceOfFinding + luckModifier) < -0.04)
        {
            return true;
        }
        return false;
    }

    private float CalculateLuck()
    {
        var luckModifier = 0f;
        if (transform.position.y < -30)
        {
            luckModifier = 0.05f;
        }
        if (transform.position.y < -60)
        {
            luckModifier = 0.1f;
        }
        if (transform.position.y < -90)
        {
            luckModifier = 0.2f;
        }
        if (haveLantern)
        {
            luckModifier += 0.05f;
        }
        if (haveTorch)
        {
            luckModifier += 0.05f;
        }
        if (haveClover)
        {
            luckModifier += 0.05f;
        }

        return luckModifier;
    }

    private void PicUpItems()
    {
        var lastInt = Vector3Int.FloorToInt(transform.position);
        var tile = tileMap.GetTile(lastInt);
        var tile2 = foreground.GetTile(lastInt);

        if(tile2 == ringTile)
        {
            SetText("You found Mimi's ring");
            haveRing = true;
            foreground.SetTile(lastInt, null);
        }

        if (tile == gold || tile2 == gold)
        {
            tileMap.SetTile(lastInt, null);
            foreground.SetTile(lastInt, null);
            goldNumber += Random.Range(1, 6);
            SetText("Found gold", "#FFD819");
        }
        if (tile == silver || tile2 == silver)
        {
            foreground.SetTile(lastInt, null);
            tileMap.SetTile(lastInt, null);
            silverNumber += Random.Range(1, 6);
            SetText("Found some silver", "#BCBCBC");
        }
        if (tile == platinum || tile2 == platinum)
        {
            foreground.SetTile(lastInt, null);
            tileMap.SetTile(lastInt, null);
            platiniumNumber += Random.Range(1, 4);
            SetText("JACPOT, found some platinium", "#464646");

        }
        if (tile == diamond || tile2 == diamond)
        {
            foreground.SetTile(lastInt, null);
            tileMap.SetTile(lastInt, null);
            diamondNumber++;
            SetText("Hot damn, you just found a diamond", "#BCBCFE");
        }
        if (tile == sand)
        {
            SetText("Grey sand, easy digging", "grey");
            tileMap.SetTile(lastInt, null);
        }
        if (tile == sand2)
        {
            SetText("Red sand, easy digging", "red");
            tileMap.SetTile(lastInt, null);
        }
        if (tile == clover)
        {
            SetText("You found a clover, luck follows you", "green");
            tileMap.SetTile(lastInt, null);
            haveClover = true;
        }
    }

    private (bool, bool) CheckForFind(Vector3Int veInt, bool undigged = false)
    {
        var findSomething = Random.value;

        var luckModifier = CalculateLuck();

        var tileContains = foreground.HasTile(veInt);

        if (findSomething - (ChanceOfFinding + luckModifier) - (undigged == true ? 0.1 : 0.0) < 0 || tileContains)
        {
            var goodOrBad = Random.value;

            if (tileContains && foreground.GetTile(veInt) != find)
            {
                return (false, false);
            }

            if (goodOrBad + luckModifier > 0.6f)
            {
                //Good
                var findWhat = Random.value;
                if (findWhat > 0 && findWhat < 0.03f && cloversFound < 3)
                {
                    cloversFound++;
                    tileMap.SetTile(veInt, clover);
                    if (undigged)
                    {
                        foreground.SetTile(veInt, null);
                    }
                }
                else if (findWhat > 0.05f && findWhat < 0.3f)
                {
                    if (undigged) foreground.SetTile(veInt, gold);
                    else
                    {
                        tileMap.SetTile(veInt, gold);
                    }
                }
                else if (findWhat > 0.3f && findWhat < 0.4f)
                {
                    if (undigged) foreground.SetTile(veInt, platinum);
                    else tileMap.SetTile(veInt, platinum);
                }
                else if (findWhat > 0.4f && findWhat < 0.5f && transform.position.y < -100)
                {
                    if (undigged) foreground.SetTile(veInt, diamond);
                    else tileMap.SetTile(veInt, diamond);
                }
                else
                {
                    if (undigged) foreground.SetTile(veInt, silver);
                    else tileMap.SetTile(veInt, silver);

                }
            }
            else
            {
                //BAD
                var findWhat = Random.Range(0.0f, 1.2f);
                if (findWhat > 0 && findWhat < 0.4)
                {
                    tileMap.SetTile(veInt, null);
                    removables.SetTile(veInt, rock);
                    foreground.SetTile(veInt, null);
                    SetText("Ouf, granite is hard", "grey");
                    return (true, false);
                }
                else if (findWhat > 0.4 && findWhat < 0.5)
                {
                    tileMap.SetTile(veInt, null);
                    removables.SetTile(veInt, water);
                    foreground.SetTile(veInt, null);
                    return (true, true);
                }
                else if (findWhat > 0.5 && findWhat < 0.7)
                {
                    tileMap.SetTile(veInt, null);

                    removables.SetTile(veInt, waterSpout);
                    if (undigged == false)
                    {
                        Overflow(veInt, 1, 0.0f, water);
                    }
                    foreground.SetTile(veInt, null);
                    return (true, true);
                }
                else if (findWhat > 0.7 && findWhat < 0.85)
                {
                    tileMap.SetTile(veInt, sand); ;
                    foreground.SetTile(veInt, null);
                }
                else if (findWhat > 0.85 && findWhat < 1.0f)
                {
                    tileMap.SetTile(veInt, sand2); ;
                    foreground.SetTile(veInt, null);
                }
                else if (findWhat > 1.0f && findWhat < 1.2f)
                {
                    tileMap.SetTile(veInt, null);
                    removables.SetTile(veInt, caveIn);
                    if (undigged == false)
                    {
                        Overflow(veInt, 1, 0.0f, dirt);
                    }
                    foreground.SetTile(veInt, null);
                    return (true, true);
                }
            }
        }
        return (false, false);
    }

    private void Overflow(Vector3Int veInt, int direction, float rand, Tile tile)
    {
        var y = veInt.y;
        var x = veInt.x;

        if (direction == 5)
        {
            return;
        }

        if (direction == 1)//opp
        {
            var random = Random.value;
            if (random - rand > 0.4f)
            {
                var newPosition = new Vector3Int(x, y + 1, 0);
                SetTile(direction, rand, tile, newPosition);
            }
            Overflow(veInt, direction + 1, rand, tile);
        }

        if (direction == 2)
        {
            var random = Random.value;
            if (random - rand > 0.4f)
            {
                var newPosition = new Vector3Int(x + 1, y, 0);
                SetTile(direction, rand, tile, newPosition);
            }
            Overflow(veInt, direction + 1, rand, tile);
        }
        if (direction == 3)
        {
            var random = Random.value;
            if (random - rand > 0.4f)
            {
                var newPosition = new Vector3Int(x, y - 1, 0);
                SetTile(direction, rand, tile, newPosition);
            }
            Overflow(veInt, direction + 1, rand, tile);
        }

        if (direction == 4)
        {
            var random = Random.value;
            if (random - rand > 0.4f)
            {
                var newPosition = new Vector3Int(x - 1, y, 0);
                SetTile(direction, rand, tile, newPosition);
            }
            Overflow(veInt, direction + 1, rand, tile);
        }
    }

    private void LooseItems()
    {
        var rand = Random.value;
        if (haveClover && rand > 0.6f)
        {
            haveClover = false;
        }
        rand = Random.value;

        if (rand > 0.4)
        {
            if (rand > 0.4 && rand < 0.6 && silverNumber != 0)
            {
                silverNumber -= Random.Range(1, 5);
                if (silverNumber < 0) silverNumber = 0;
                SetText("You lost some silver");
            }
            if (rand > 0.6 && rand < 0.8 && goldNumber != 0)
            {
                goldNumber -= Random.Range(1, 5);
                if (goldNumber < 0) goldNumber = 0;
                SetText("You lost some gold");

            }
            if (rand > 0.8 && rand < 1.0 && platiniumNumber != 0)
            {
                platiniumNumber -= Random.Range(1, 5);
                if (platiniumNumber < 0) platiniumNumber = 0;
                SetText("You lost some platinium :(");

            }
        }
        else
        {
            rand = Random.value;

            if (rand > 0 && rand < 0.1 && havePicaxe)
            {
                havePicaxe = false;
                SetText("You lost your picaxe");
            }
            if (rand > 0.1 && rand < 0.2 && haveShovel)
            {
                haveShovel = false;
                SetText("You lost your shovel");

            }
            if (rand > 0.2 && rand < 0.3 && haveTorch)
            {
                SetText("You lost your torch");
                haveTorch = false;
            }
            if (rand > 0.3 && rand < 0.4 && haveLantern)
            {
                SetText("You lost your lantern");
                haveLantern = false;
            }
            if (rand > 0.4 && rand < 0.5 && haveDynamite)
            {
                SetText("You lost your dynamite");
                haveDynamite = false;
            }
            if (rand > 0.5 && rand < 0.6 && haveClover)
            {
                SetText("You lost your clover");
                haveClover = false;
            }
            if (rand > 0.6 && rand < 0.7 && haveBucket)
            {
                SetText("You lost your bucket");
                haveBucket = false;
            }
            if (rand > 0.8 && rand < 1.0 && haveDrill)
            {
                SetText("You lost your drill");
                haveDrill = false;
            }
        }
    }

    private void SetTile(int direction, float rand, Tile tile, Vector3Int newPosition)
    {
        var x = newPosition.x;
        var y = newPosition.y;
        if (y >= 5 || x <= -8 || x >= 23 || y <= -138)
        {
            return;
        }

        if (tileMap.GetTile(newPosition) != dirt && removables.GetTile(newPosition) == null)
        {
            if (tile == dirt)
            {
                tileMap.SetTile(newPosition, dirt);
            }
            else
            {
                removables.SetTile(newPosition, tile);
            }
        }
        Overflow(newPosition, direction, rand + 0.1f, tile);
    }

    private bool Move(Vector3 direction)
    {
        Vector3 newPosition = movePoint.position + direction;
        var tile = removables.GetTile(Vector3Int.FloorToInt(newPosition));

        if (action != Actions.None && Physics2D.OverlapCircle(newPosition, 0.2f, removableMask))
        {
            if (tile == rock && action == Actions.Drill)
            {
                removables.SetTile(Vector3Int.FloorToInt(newPosition), null);
                money -= 5;
                SetText("Drilling is hard work");
            }
            if (tile == water && action == Actions.Drain)
            {
                removables.SetTile(Vector3Int.FloorToInt(newPosition), null);
                money -= 20;
                SetText("Removed some water", "blue");
            }
            action = Actions.None;
        }

        if (Physics2D.OverlapCircle(newPosition, 0.2f, removableMask))
        {

            if ((tile == water || tile == waterSpout) && !doingDamage)
            {
                healt -= (int)Random.Range(2, 8);
                doingDamage = true;
                timeSinceDamage = 0;
                SetText("Carefull, you can drown in water", "blue");
                LooseItems();
                if (tile == waterSpout)
                {
                    Overflow(Vector3Int.FloorToInt(newPosition), 1, -0.3f, water);
                }

                return false;
            }

            if (tile == caveIn && !doingDamage)
            {
                healt -= (int)Random.Range(2, 8);
                doingDamage = true;
                timeSinceDamage = 0;
                SetText("Oh no, there was an cavein", "brown");
                LooseItems();
                if (tile == caveIn)
                {
                    Overflow(Vector3Int.FloorToInt(newPosition), 1, -0.3f, dirt);
                }

                return false;
            }
        }

        if (!Physics2D.OverlapCircle(newPosition, 0.2f, obstacleMask) && !Physics2D.OverlapCircle(newPosition, 0.2f, removableMask))
        {
            LastPosition = movePoint.position;
            movePoint.position = newPosition;
            action = Actions.None;
            return true;
        }

        return false;
    }

    public enum Actions
    {
        None,
        Drill,
        Drain,
        Ignite
    }

    public enum Mode
    {
        Game,
        Store,
        Bank,
        Hospital,
        Help,
        GameOver,
        Brothel
    }
}