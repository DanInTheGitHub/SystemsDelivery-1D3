using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject main;
    public GameObject login;
    public GameObject register;

    private void Start()
    {
        main.SetActive(true);
        login.SetActive(false);
        register.SetActive(false);
    }

    public void ShowLoginWithDelay(float delay)
    {
        StartCoroutine(ShowLoginAfterDelay(delay));
    }

    public void HideLoginWithDelay(float delay)
    {
        StartCoroutine(HideLoginAfterDelay(delay));
    }

    public void ShowRegisterWithDelay(float delay)
    {
        StartCoroutine(ShowRegisterAfterDelay(delay));
    }

    public void HideRegisterWithDelay(float delay)
    {
        StartCoroutine(HideRegisterAfterDelay(delay));
    }

    private IEnumerator ShowLoginAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        login.SetActive(true);
        main.SetActive(false);
    }

    private IEnumerator HideLoginAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        login.SetActive(false);
        main.SetActive(true);
    }

    private IEnumerator ShowRegisterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        register.SetActive(true);
        main.SetActive(false);
    }

    private IEnumerator HideRegisterAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        register.SetActive(false);
        main.SetActive(true);
    }
}
