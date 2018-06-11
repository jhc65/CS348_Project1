using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoasterManager : MonoBehaviour {

	public enum SectionTriggers /* Names of triggers on Animator */
    {
        PlaySectionA,
        PlaySectionB,
        PlaySectionC,
        PlayFullSection,
        PlayEnter,
        PlayExit
    }
    private static string PlaySpeedMultipier = "PlaySpeedMultipier"; /* Float parameter name on Animator */
    private Animator animator;

    [SerializeField] private SpriteRenderer[] sprites;
    public SpriteRenderer[] decalSprites;
    public Sprite[] decals;
    [SerializeField] private AudioSource trackAudio;
    private Vector3 startPosition;
    private SplineFollower[] splineFollowers;
    [SerializeField] private float normalSpeed;
    [SerializeField] private float slowedSpeed;

    public static CoasterManager Instance { get; private set; }

    public void Awake()
    {
        /* Since the coaster is destroyed onLoad, always update the instance */
        Instance = this;
        animator = GetComponent<Animator>();
        startPosition = this.transform.position;
        splineFollowers = GetComponentsInChildren<SplineFollower>();
    }

    public void ChangeColor(Color c)
    {
        foreach (SpriteRenderer sp in sprites)
            sp.color = c;
        foreach (SpriteRenderer sp in decalSprites)
        {
            sp.sprite = decals[Constants.decalIndex];
            sp.color = new Color(1 - c.r, 1 - c.g, 1 - c.b);
        }
    }

    private void Start()
    {
        ChangeColor(Constants.trackColor);
    }

    public void PlaySection(SectionTriggers st)
    {
        animator.SetTrigger(st.ToString());
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        switch(collider.tag)
        {
            case "decrease":
                //animator.SetFloat(PlaySpeedMultipier, Constants.slowCoasterSpeed);
                foreach (SplineFollower sf in splineFollowers)
                    sf.followSpeed = slowedSpeed;
                break;
            case "increase":
                SpeedUp();
                break;
            case "lose":
                /* Stop the coaster animation */
                trackAudio.Stop();
                animator.Rebind();
                /* Tell GameController to end the game with a lose state */
                GameController.Instance.EndGame(false);
                break;
///            case "Section":
///                GameController.Instance.TriggerNextSectionAnimation();
///                break;
            default:

                break;
        }
    }

    public void SpeedUp()
    {
        //animator.SetFloat(PlaySpeedMultipier, Constants.fastCoasterSpeed);
        foreach (SplineFollower sf in splineFollowers)
            sf.followSpeed = normalSpeed;
    }

    public void StopTrackAudio()
    {
        trackAudio.Stop();
    }

    public void ExitScreenFinished()
    {
        trackAudio.Stop();
        SceneManager.LoadScene("Main");
    }

    public void PlayTrackAudio()
    {
        trackAudio.Play();
    }

    public void ResetToStartPosition()
    {
        /* Stop the animation and audio */
        trackAudio.Stop();
        animator.Rebind();

        this.transform.position = startPosition;
    }
}
