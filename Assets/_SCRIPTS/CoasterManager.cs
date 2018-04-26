using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoasterManager : MonoBehaviour {

	public enum SectionTriggers /* Names of triggers on Animator */
    {
        PlaySectionA,
        PlaySectionB,
        PlaySectionC,
    }
    private static string PlaySpeedMultipier = "PlaySpeedMultipier"; /* Float parameter name on Animator */
    private float slowPlaySpeed = .25f;
    private float fastPlaySpeed = 1.5f;


    private static CoasterManager instance;
    private Animator animator;


    public static CoasterManager GetInstance()
    {
        return instance;
    }

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            animator = GetComponent<Animator>();
        }
        else
        {
            Destroy(this.gameObject);
        }
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
                animator.SetFloat(PlaySpeedMultipier, slowPlaySpeed);
                break;
            case "increase":
                animator.SetFloat(PlaySpeedMultipier, fastPlaySpeed);
                break;
            case "BuildZone":
                if (!collider.GetComponent<BuildZone>().IsGapFilled())
                    GameController.Instance.EndGame(false);
                else
                    animator.SetFloat(PlaySpeedMultipier, 1f);
                break;
            case "Section":
                PlaySection(collider.GetComponent<Section>().GetAnimationTrigger());
                break;
            default:

                break;
        }
    }
}
