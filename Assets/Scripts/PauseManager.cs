using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    [SerializeField] private GameObject[] PauseElements;

    [SerializeField] private GameObject PauseAll;
    private Animator PauseAllAnimator;
    private bool PauseAllDone;

    private void Start()
    {
        PauseAllAnimator = PauseAll.GetComponent<Animator>();
    }

    public void PauseActivation()
    {
        PauseAllDone = true;
        PauseAllAnimator.SetBool("PauseActivationBool", PauseAllDone);
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void Reload()
    {
        //PauseAll.transform.localScale = new Vector3(0f, 0f, 0f);
        PauseAllDone = false;
        PauseAllAnimator.SetBool("PauseActivationBool", PauseAllDone);
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
}
