using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ModelSenseBlink : MonoBehaviour
{
    public int maxBlinks = 3;

    public float moveSpeed = 1;

    private int currentBlinks = 0;

    private GameObject model;

    private Animator ani;

    public GameObject gameOver;
    public TextMeshProUGUI currentBlinkstxt;
    public TextMeshProUGUI maxBlinkstxt;

    public Button restart;

    public void Restart(){

      currentBlinks = 0;

      currentBlinkstxt.text = "0"; maxBlinkstxt.text = maxBlinks.ToString();
      gameOver.SetActive(false);
      model = this.gameObject;
      model.transform.position = new Vector3(0f,1.04f,-6.69f);
      EyeData.Instance.BlinkEvent.AddListener(ModelMove);
      ani = model.GetComponent<Animator>();

       ani.SetBool("gameOver", false);
    }

    void Start()
    {

      restart.GetComponent<Button>();
      restart.onClick.AddListener(Restart);


      Restart();
    }

    public void ModelMove(){
      ani.SetTrigger("nextState");
      currentBlinks ++; currentBlinkstxt.text = currentBlinks.ToString();
      model.transform.position -= new Vector3(0f, 0f, moveSpeed);
      ani.SetBool("gameOver", false);

      if (currentBlinks == maxBlinks){
        gameOver.SetActive(true);
        Debug.Log("Game Over!");
        EyeData.Instance.BlinkEvent.RemoveListener(ModelMove);
        ani.SetBool("gameOver", true);
      }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
