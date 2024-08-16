using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public float health;
    public bool inRestart = false;
    public GameObject loadUI;
    public RectTransform progressBar;
    public RectTransform healthBar;

    private void Start()
    {
        healthText.text = health.ToString();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (healthBar) healthBar.localScale = new Vector3(health / 100, 1, 1);

        if (health <= 0)
        {
            health = 0;
            if (gameObject.tag == "Player" && !inRestart)
            {
                inRestart = true;
                StartCoroutine(ReStart());
            }
        }

        healthText.text = health.ToString();
    }

    IEnumerator ReStart()
    {
        progressBar.localScale = new Vector3(0, 1, 1);
        loadUI.SetActive(true);

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        asyncOperation.allowSceneActivation = false;
        float progress = 0;

        while (!asyncOperation.isDone)
        {
            progress = Mathf.MoveTowards(progress, asyncOperation.progress, Time.deltaTime);
            progressBar.localScale = new Vector3(progress, 1, 1);

            if (progress >= 0.9f)
            {
                progressBar.localScale = new Vector3(1, 1, 1);
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
