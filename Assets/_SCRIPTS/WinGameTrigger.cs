using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinGameTrigger : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collider)
    {
        /* Notify GameController that the coaster hit the trigger
         *
         * TODO: this assumes only the coaster can enter it
         */
        StartCoroutine(VictoryLap());
    }

    private IEnumerator VictoryLap()
    {
        EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Confetti);
        EffectsManager.Instance.PlayEffect(EffectsManager.Effects.Yay);
        yield return new WaitForSeconds(Constants.endGameWaitDelay);
        GameController.Instance.EndGame(true);
        yield return null;
    }
}
