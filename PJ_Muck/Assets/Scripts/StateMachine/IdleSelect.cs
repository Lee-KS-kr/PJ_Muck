using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleSelect : StateMachineBehaviour
{
    public float minWaitTime = 1.5f;
    public float maxWaitTime = 4.5f;
    public float waitTime;

    private float[] percentages = {0.45f, 0.75f, 0.95f, 1.0f};

    private static readonly int idleSelect = Animator.StringToHash("IdleSelect");
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        waitTime = Random.Range(minWaitTime, maxWaitTime);
        animator.SetInteger(idleSelect, 0);
        // Test();
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime > waitTime)
            animator.SetInteger(idleSelect, SelectIdle());
    }

    private int SelectIdle()
    {
        float random = Random.Range(0.0f, 1.0f);
        int result = 0;
        if (random < percentages[0])
            return result = 1;
        else if (random < percentages[1])
            return result = 2;
        else if (random < percentages[2])
            return result = 3;
        else
            return result = 4;
    }
    
    void Test()
    {
        int[] result = new int[5];

        for (int i = 0; i < 100000 ; i++)
        {
            result[SelectIdle()]++;
        }
        
        Debug.Log($"Result : {result[0]}, {result[1]}, {result[2]}, {result[3]}, {result[4]}");
    }
}
