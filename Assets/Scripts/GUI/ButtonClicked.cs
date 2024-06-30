using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClicked : MonoBehaviour
{
    private Animator animator;
            float timer = 0f;
            public bool onclick;

    private void Start() {
        animator = GetComponent<Animator>();
    }
       public void playAnim()
    {
        animator.SetBool("İsClicked", true);
    }


private void Update() {
    if (onclick)
    {
              timer += Time.deltaTime;

        if (timer > 1f)
        {
            timer = 0f;
            animator.SetBool("İsClicked", false);
            onclick = false;
           SceneManager.LoadScene("MainScene");
        }
    }


}

    public void setfalse()
    {
        onclick = true;
    }
}


