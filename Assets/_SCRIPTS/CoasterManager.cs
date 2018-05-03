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
        PlayEnterScreen
    }
    private static string PlaySpeedMultipier = "PlaySpeedMultipier"; /* Float parameter name on Animator */

    private static CoasterManager instance;
    private Animator animator;

    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private AudioSource trackAudio;

    public static CoasterManager Instance {
		get {return instance;}
	}

    public void Awake()
    {
        /* Since the coaster is destroyed onLoad, always update the instance */
        instance = this;
        animator = GetComponent<Animator>();
    }

    public void ChangeColor(Color c)
    {
        foreach (SpriteRenderer sp in sprites)
            sp.color = c;
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
                animator.SetFloat(PlaySpeedMultipier, Constants.slowCoasterSpeed);
                break;
            case "increase":
                animator.SetFloat(PlaySpeedMultipier, Constants.fastCoasterSpeed);
                break;
            case "BuildZone":
                if (!collider.GetComponentInParent<BuildZone>().IsGapFilled())
                    GameController.Instance.EndGame(false);
                break;
            case "Section":
                GameController.Instance.TriggerNextSectionAnimation();
                break;
            default:

                break;
        }
    }

    public void SpeedUp()
    {
        animator.SetFloat(PlaySpeedMultipier, Constants.fastCoasterSpeed);
    }

    public void StopTrackAudio()
    {
        trackAudio.Stop();
    }

    public void PlayTrackAudio()
    {
        trackAudio.Play();
    }
}
