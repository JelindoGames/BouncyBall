using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Handles the incredible and amazing sequence that happenes
// in the opening scene.
// Controls each of the interactions/changes one by one.
public class OpeningSceneController : MonoBehaviour
{
    [SerializeField] Sprite polly1;
    [SerializeField] Sprite polly2;
    [SerializeField] SpriteRenderer pollyRenderer;
    [SerializeField] GameObject pollyLabel;
    [SerializeField] GameObject parentsLabel;
    [SerializeField] GameObject mom;
    [SerializeField] GameObject dad;
    [SerializeField] GameObject rexLabel;
    [SerializeField] Animator rexAnimator;
    [SerializeField] AudioClip pollyLabelClip;
    [SerializeField] AudioClip parentsLabelClip;
    [SerializeField] AudioClip rexLabelClip;
    [SerializeField] AudioClip destructionClip;
    [SerializeField] AudioClip rexWalkClip;
    [SerializeField] Image fadeToBlack;
    [SerializeField] AudioClip endingClip;
    [SerializeField] GameObject audioPlayer2D;
    [SerializeField] int firstLevelIdx;

    private void Start()
    {
        StartCoroutine("OpeningScene");
    }

    IEnumerator OpeningScene()
    {
        // A string of chronologically ordered things that happen in the
        // opening sequence.
        fadeToBlack.color = new Color(0, 0, 0, 0);
        pollyRenderer.sprite = polly1;
        yield return new WaitForSeconds(1.5f);
        AudioSource.PlayClipAtPoint(pollyLabelClip, Camera.main.transform.position);
        pollyLabel.SetActive(true);
        yield return new WaitForSeconds(1.75f);
        AudioSource.PlayClipAtPoint(parentsLabelClip, Camera.main.transform.position);
        pollyLabel.SetActive(false);
        parentsLabel.SetActive(true);
        yield return new WaitForSeconds(1.75f);
        parentsLabel.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        AudioSource.PlayClipAtPoint(rexWalkClip, Camera.main.transform.position);
        rexAnimator.SetTrigger("Trigger");
        yield return new WaitForSeconds(1.75f);
        AudioSource.PlayClipAtPoint(rexLabelClip, Camera.main.transform.position);
        rexLabel.SetActive(true);
        yield return new WaitForSeconds(1.75f);
        rexLabel.SetActive(false);
        yield return new WaitForSeconds(0.65f);
        mom.SetActive(false);
        dad.SetActive(false);
        AudioSource.PlayClipAtPoint(destructionClip, Camera.main.transform.position);
        yield return new WaitForSeconds(3f);
        pollyRenderer.sprite = polly2;
        float zoomTime = 0f;
        GameObject cam = Camera.main.gameObject;
        GameObject audioPlayer = Instantiate(audioPlayer2D);
        AudioSource source = audioPlayer.GetComponent<AudioSource>();
        source.clip = endingClip;
        source.Play();
        while (zoomTime < 4f)
        {
            cam.transform.position += new Vector3(2f, -1f, 2f) * Time.deltaTime;
            zoomTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        while (fadeToBlack.color.a < 1)
        {
            fadeToBlack.color += new Color(0, 0, 0, 0.5f) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene(firstLevelIdx);
    }
}
