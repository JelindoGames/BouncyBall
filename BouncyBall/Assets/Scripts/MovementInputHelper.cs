using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstracts player movement so the Movement script is more manageable.
public class MovementInputHelper : MonoBehaviour
{
    [SerializeField] float perfectBounceWindowLength;
    [SerializeField] float perfectBounceTimeBtwnWindows;
    [SerializeField] int framesNeededGroundedForPeriod;
    [SerializeField] float regroundableTimer;
    bool canPressPerfectBounce; // Can only press between windows
    bool regroundable;

    //// To be seen by Movement.cs //////////////////
    public bool inPerfectBounceWindow { get; private set; }
    public bool inBounceWindow { get; private set; }
    public bool grounded { get; private set; }
    public bool groundedForPeriod { get; private set; }
    /////////////////////////////////////////////////

    void Start()
    {
        canPressPerfectBounce = true;
    }

    private void Update()
    {
        if (!grounded)
        {
            groundedForPeriod = false;
        }
        inBounceWindow = Input.GetAxis("Jump") > 0.005f;
        if (Input.GetKeyDown(KeyCode.Space) && canPressPerfectBounce)
        {
            StartCoroutine("HandlePerfectBounceWindow");
        }
    }

    IEnumerator HandlePerfectBounceWindow()
    {
        inPerfectBounceWindow = true;
        canPressPerfectBounce = false;
        yield return new WaitForSeconds(perfectBounceWindowLength);
        inPerfectBounceWindow = false;
        yield return new WaitForSeconds(perfectBounceTimeBtwnWindows);
        canPressPerfectBounce = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!LevelManager.levelPlaying)
        {
            return;
        }

        if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Speed Booster" || collision.gameObject.tag == "Breakable")
        {
            if (regroundable)
            {
                grounded = true;
                StartCoroutine("WaitForGroundedForPeriod");
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!LevelManager.levelPlaying)
        {
            return;
        }

        if (collision.gameObject.tag == "Untagged" || collision.gameObject.tag == "Speed Booster" || collision.gameObject.tag == "Breakable")
        {
            grounded = false;
            StartCoroutine("HandleRegroundableTimer");
        }
    }

    IEnumerator WaitForGroundedForPeriod()
    {
        int frames = 1;

        while (frames < framesNeededGroundedForPeriod && grounded)
        {
            frames++;
            groundedForPeriod = false;
            yield return new WaitForEndOfFrame();
        }
        groundedForPeriod = grounded;
    }


    IEnumerator HandleRegroundableTimer()
    {
        regroundable = false;
        yield return new WaitForSeconds(regroundableTimer);
        regroundable = true;
    }
}
