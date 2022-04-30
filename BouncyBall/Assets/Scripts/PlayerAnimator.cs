using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handles the player animations programmatically.
public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Update()
    {
        anim.SetBool("SpaceDown", Input.GetKeyDown(KeyCode.Space));
    }
}
