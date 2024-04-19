using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    //[SerializeField] private Transform player;

    [SerializeField] private string transitionTo;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Vector2 exitDirection;
    [SerializeField] private float exitTime;

    private void Start()
    {
        if (transitionTo == GameManager.Instance.transitionedFromScene)
        {
            Player.Instance.transform.position = startPoint.position;
            StartCoroutine(Player.Instance.gameObject.GetComponentInChildren<PlayerController>().WalkIntoNewScene(exitDirection, exitTime));
        }
        StartCoroutine(UIManager.Instance.screenFader.Fade(ScreenFader.FadeDirection.Out));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.transitionedFromScene = SceneManager.GetActiveScene().name;
            Player.Instance.gameObject.GetComponentInChildren<PlayerController>().playerState.CutScene = true;

            StartCoroutine(UIManager.Instance.screenFader.FadeAndLoadScene(ScreenFader.FadeDirection.In, transitionTo));
        }
    }
}
