using UnityEngine; // базовая библиотека для поддержки классов из Юнити
using UnityEngine.UI; // библиотека для работы с польщовательсикм интерфесом
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour // класс наследуемый Player от MonoBehaviour
{
    public Rigidbody2D PlayerRigidbody; // создаём поле физики для игрока

    public LayerMask GroundMask; // поле, для того чтобы игрок знал, на какой поверхности ему можно прыгать
    private bool isGround; // флажок, обозначающий true = можно прыгать \ false = нельзя прыгать

    public GameObject MoneyObject; // поле для моенты
    public GameObject HeartObject; // поле для сердца

    public int CountMoney; // счётчик собранных монет
    public Text CountMoneyText; // поле текста для отображения количества собрыных монет

    public int CountHearts; // счётчик собранных сердце
    public Text CountHeartsText; // поле текста для отображения количества собранных сердец

    private bool isSecondJump;
    private int CountJump;

    public Animator PlayerAnimator;

    public GameObject GameOverText;
    public GameObject Dark;
    public GameObject ReloadButton;

    [SerializeField] private Transform MainCamera;

    [SerializeField] private ParticleSystem Dust;
    [SerializeField] private GameObject DustObject;

    [SerializeField] private GameObject[] UIHearts;

    public Scene GameScene;

    [SerializeField] private AudioSource MoneySound;

    private void Start() // метод\функия, срабатывающая при старте игры
    {
        PlayerRigidbody = GetComponent<Rigidbody2D>(); // заполняем поле физики\даём ссылку
        CountHearts = 2;
        GameOverText.SetActive(false);
        Dark.SetActive(false);
        ReloadButton.SetActive(false);
    }

    private void Update()// метод срабатываемый каждый игровой кадр
    {
        Move(); // запускаем нами созданнй метод для ходьбы и прыжка
        if (Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1.1f, GroundMask)) // выпускаем маленький, невидимый лучи из нашего игрока и проверяем, касается ли он земли
        {
            isGround = true;// если луч касается земли, игрок может прыгать
        }
        else // иначе\если луч не касается земли
        {
            isGround = false; // игрок не сможет прыгнуть
        }

        if(CountJump == 1)
        {
            isSecondJump = true;
        }
        else
        {
            isSecondJump = false;
        }

        if (Physics2D.Raycast(gameObject.transform.position, Vector2.down, 1.001f, GroundMask))
        {
            CountJump = 1;
        }

        CountMoneyText.text = CountMoney.ToString(); // присваевыем тексту на экране значение количества денег, приобразовывем количество(цмфры) в текст, для отображения на экране
        CountHeartsText.text = CountHearts.ToString(); // присваевыем тексту на экране значение количества сердец, приобразовывем количество(цмфры) в текст, для отображения на экране

        if(CountHearts < 0)
        {
            CountHearts = 0;
        }

        if(CountHearts == 0)
        {
            GameOverText.SetActive(true);
            Dark.SetActive(true);
            ReloadButton.SetActive(true);
        }

        MainCamera.position = new Vector3(gameObject.transform.position.x + 4f, 0f, -1f);

        if(CountHearts == 3)
        {
            for(int i = 0; i < UIHearts.Length; i++)
            {
                UIHearts[i].SetActive(true);
            }
        }
        else if(CountHearts == 2)
        {
            UIHearts[2].SetActive(false);
            UIHearts[1].SetActive(true);
            UIHearts[0].SetActive(true);
        }
        else if(CountHearts == 1)
        {
            UIHearts[1].SetActive(false);
            UIHearts[0].SetActive(true);
        }
        else if(CountHearts == 0)
        {
            UIHearts[0].SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // метод, срабатываемый при входе в триггер
    {
        if(collision.tag == "Money") // если игрок коснётся объекта с тегом Money
        {
            MoneySound.GetComponent<AudioSource>().Play();
            Destroy(MoneyObject); // монета исчезнет
            CountMoney++; // количество монет прибавится на 1
        }

        if(collision.tag == "Heart") // если игрок коснётся объекта с тегом Heart
        {
            MoneySound.GetComponent<AudioSource>().Play();
            Destroy(HeartObject); // сердце исчезнет
            CountHearts++; // количество сердец прибавится на 1
            if(CountHearts == 4)
            {
                CountHearts = 3;
            }
        }

        if(collision.tag == "Enemy")
        {
            CountHearts--;
            GameObject Enemy = collision.gameObject;
        }

        if(collision.tag == "Exit")
        {
            SceneManager.LoadScene("FinishScene");
        }

        if (collision.tag == "EnemyDed")
        {
            SceneManager.LoadScene("GameScene");
        }
    }

    private void Move() // нами созданный метод для передвижения и прыжка персонажа, вызываемый каждый кадр. Вызывается каждый кадр потому что вызываем этот метод мы в 27 строке кода, в Update
    {
        float Direction = Input.GetAxis("Horizontal"); // переменной float присваеваем знаечние "лево-право"\горизонталь\левая или правая кнопка на клавиатуре
        float speed = 5f;
        PlayerRigidbody.linearVelocity = new Vector2(Direction * speed, PlayerRigidbody.linearVelocity.y); // толкаем объект влево или вправо, умножаем это на 5 для скорости

        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = 20f;
        }
        else
        {
            speed = 5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround == true) // если нажмём на пробел и флажок разрешит прыгать
        {
            PlayerRigidbody.linearVelocity = new Vector2(PlayerRigidbody.linearVelocity.x, 15f); // толкнём игрока вверх для прыжка
            CountJump = 1;
            PlayerAnimator.Play("Jump");
        }

        if (Input.GetKeyDown(KeyCode.Space) && isSecondJump == true)
        {
            PlayerRigidbody.linearVelocity = new Vector2(PlayerRigidbody.linearVelocity.x, 10f);
            CountJump = 2;
            PlayerAnimator.Play("Jump");
        }

        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            PlayerAnimator.Play("Walk");
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            gameObject.transform.localScale = new Vector3(-0.1235272f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            gameObject.transform.localScale = new Vector3(0.1235272f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
        }

        if (isGround)
        {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
            {
                Dust.Play();
                DustObject.SetActive(true);
            }
            else
            {
                Dust.Pause();
                DustObject.SetActive(false);
            }
        }
        else
        {
            Dust.Pause();
            DustObject.SetActive(false);
        }
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}