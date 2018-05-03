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

    public static CoasterManager Instance {
		get {return instance;}
	}

    public void Awake()
    {
        /* Since the coaster is destroyed onLoad, always update the instance */
        instance = this;
        animator = GetComponent<Animator>();
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
}
