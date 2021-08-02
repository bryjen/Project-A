using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffHandler : MonoBehaviour
{
    [SerializeField] private EntityBehavior entityBehavior;
    [SerializeField] private Health health;

    public void InflictDebuff(Debuff debuff, float duration)
    {
        switch (debuff)
        {
            case Debuff.ENCODED:
                StartCoroutine(EncodedDebuff(duration));
                break;
        }
    }

    private IEnumerator EncodedDebuff(float duration)
    {
        health.SetDefaultColor(new Color(0, 1, 1, 1));
        entityBehavior.Timescale = 0.5f;
        
        yield return new WaitForSeconds(duration);

        health.SetDefaultColor(Color.white);
        entityBehavior.Timescale = 1;
        yield break;
    }
}
