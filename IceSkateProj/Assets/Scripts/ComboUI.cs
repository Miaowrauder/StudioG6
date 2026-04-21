using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComboUI : MonoBehaviour
{
[Header("Variables")]
    public int maximumTimer;
    public float currentTimer;
    public float ScoreMult;
    public float Score;
    [SerializeField] float PreviousScore;
    [SerializeField] float PreviousTotalScore;
    [SerializeField] float TotalScore;
    int level;

[Header("Instances")]
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI TotalScoreText;
    public Image ProgressBar;
    public Image Bar;
    public GameObject Note;
    public Image Border;
    public ParticleSystem Snow;
    public ParticleSystem Snowflakes;
    [SerializeField] Sprite[] comboNotes;
    [SerializeField] Sprite newNote;


[Header("Sounds")]
    public AudioSource audioSource;
    public AudioClip[] IceSounds;

[Header("Animators")]
    public Animator anim;
    public Animator Baranim;

    void Start()
    {
        StartCoroutine(Reset());
        currentTimer = 0;
        level = 0;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Score > PreviousScore)
        {
            Note.SetActive(true);
            currentTimer = maximumTimer;
            
            StartCoroutine(CountUpToTarget());
        }

        if (currentTimer > 0)
        {
            if (currentTimer > maximumTimer)
            {
                currentTimer = maximumTimer;
            }
            ProgressBar.enabled = true;
            Bar.enabled = true;
            Border.enabled = true;
            UpdateBar();
        }
        else if (currentTimer <= 0 && Score > 0)
        {
            StartCoroutine(Reset());
            audioSource.pitch = Random.Range(0.75f, 1.5f);
            audioSource.PlayOneShot(IceSounds[4], 1);
            StartCoroutine(TotalCountUpToTarget());
        }
    }

    void UpdateBar()
    {
        float fillAmount = (float)currentTimer / (float)maximumTimer;
        Bar.fillAmount = fillAmount;
        currentTimer -= (Time.deltaTime + ScoreMult / 100);
    }

    IEnumerator Reset()
    {
        TotalScore += Score;
        currentTimer = 0;
        Score = 0;
        PreviousScore = 0;
        level = 0;
        ScoreMult = 0;
        newNote = comboNotes[0];
        Note.GetComponent<Image>().sprite = newNote;
        ScoreText.text = "";
        float fillAmount = (float)currentTimer / (float)maximumTimer;
        Bar.fillAmount = fillAmount; 
        ProgressBar.enabled = false;
        Border.enabled = false;
        Note.SetActive(false);
        yield return null;
    }

    IEnumerator CountUpToTarget()
    {
        while (PreviousScore < Score)
        {
            PreviousScore += Time.deltaTime * 50;
            PreviousScore = Mathf.Clamp(PreviousScore, 0f, Score);
            Mathf.Round(PreviousScore);
            ScoreText.text = string.Format("+ {0:#,#}",PreviousScore);

            if (PreviousScore >= 0 && level == 0)
            {
                level = 1;
                Snow.Emit(5);
                Snowflakes.Emit(10);
                newNote = comboNotes[0];
                Baranim.SetTrigger("In");
                anim.SetTrigger("Upgrade");
                audioSource.pitch = Random.Range(0.5f, 1.5f);
                audioSource.PlayOneShot(IceSounds[0], 1);
                ScoreMult = 0f;
            }
            else if (PreviousScore >= 2500 && level == 1)
            {
                Snow.Emit(5);
                Snowflakes.Emit(15);
                level = 2;
                newNote = comboNotes[1];
                audioSource.pitch = Random.Range(0.75f, 1.5f);
                audioSource.PlayOneShot(IceSounds[1], 1);
                anim.SetTrigger("Upgrade");
                ScoreMult = 0.1f;
            }
            else if (PreviousScore >= 5000 && level == 2)
            {
                Snow.Emit(5);
                Snowflakes.Emit(20);
                level = 3;
                newNote = comboNotes[2];
                audioSource.pitch = Random.Range(0.75f, 1.5f);
                audioSource.PlayOneShot(IceSounds[2], 1);
                anim.SetTrigger("Upgrade");
                ScoreMult = 0.25f;
            }
            else if (PreviousScore >= 7500 && level == 3)
            {
                Snow.Emit(5);
                Snowflakes.Emit(25);
                level = 4;
                newNote = comboNotes[3];
                audioSource.pitch = Random.Range(0.75f, 1.5f);
                audioSource.PlayOneShot(IceSounds[3], 1);
                anim.SetTrigger("Upgrade");
                ScoreMult = 0.5f;
            }
            else if (PreviousScore >= 12500 && level == 4)
            {
                anim.SetTrigger("UpgradeMega");
                Snow.Emit(5);
                audioSource.pitch = Random.Range(0.75f, 1.5f);
                audioSource.PlayOneShot(IceSounds[3], 1);
                Snowflakes.Emit(25);
                level = 5;
                ScoreMult = 0.75f;
            }

            Note.GetComponent<Image>().sprite = newNote;

            yield return null;
        }
    }

     IEnumerator TotalCountUpToTarget()
    {
        while (PreviousTotalScore < TotalScore)
        {
            PreviousTotalScore += Time.deltaTime * 1000;
            PreviousTotalScore = Mathf.Clamp(PreviousTotalScore, 0f, TotalScore);
            Mathf.Round(PreviousTotalScore);
            TotalScoreText.text = string.Format("{0:#,#}",PreviousTotalScore);
            yield return null;
        }
    }
}
