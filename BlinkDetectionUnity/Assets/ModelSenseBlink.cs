using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelSenseBlink : MonoBehaviour
{
    public int maxBlinks = 3;

    public float moveSpeed = 1;

    private int currentBlinks = 0;

    private GameObject model;

    private Animator ani;

    void Start()
    {
      model = this.gameObject;
      EyeData.Instance.BlinkEvent.AddListener(ModelMove);
      ani = model.GetComponent<Animator>();
    }

    public void ModelMove(){
      currentBlinks ++;
      model.transform.position -= new Vector3(0f, 0f, moveSpeed);
      ani.SetBool("gameOver", false);

      if (currentBlinks == maxBlinks){
        Debug.Log("Game Over!");
        EyeData.Instance.BlinkEvent.RemoveListener(ModelMove);
        ani.SetBool("gameOver", true);
      }
      ani.SetTrigger("nextState");
      ani.ResetTrigger("nextState");




    }

    // Update is called once per frame
    void Update()
    {

    }
}
