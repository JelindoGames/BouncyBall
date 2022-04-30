using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Changes the player's color depending
// on their movement state.
[RequireComponent(typeof(Movement))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] Material playerMat;
    Movement mvmt;
    // Helps to distinguish the movement state that is happening.
    MovementInputHelper mih;

    public bool beingTransported;

    private void Start()
    {
        mvmt = GetComponent<Movement>();
        mih = GetComponent<MovementInputHelper>();
    }

    private void Update()
    {
        if (beingTransported)
        {
            playerMat.color = Color.cyan;
        }
        else if (mih.inPerfectBounceWindow)
        {
            playerMat.color = Color.red;
        }
        else if (mvmt.currentState() == Movement.State.DropSuspension || mvmt.currentState() == Movement.State.DropPhysical)
        {
            playerMat.color = Color.gray;
        }
        else if (mih.inBounceWindow)
        {
            playerMat.color = Color.Lerp(Color.red, Color.white, 0.5f);
        }
        else
        {
            playerMat.color = Color.white;
        }
    }
}
