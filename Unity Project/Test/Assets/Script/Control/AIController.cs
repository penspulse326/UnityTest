using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
  [SerializeField] float chaseDistance = 10f;
  [SerializeField] float confuseTime = 5f;


  GameObject player;
  Animator animator;

  //起始位置
  Vector3 beginPosition;

  //上次看到玩家的時間
  private float timeLastSawPlayer = Mathf.Infinity;

  Mover mover;

  private void Awake()
  {
    mover = GetComponent<Mover>();
    animator = GetComponent<Animator>();
    player = GameObject.FindGameObjectWithTag("Player");
    beginPosition = transform.position;
  }
  
  private void Update() {

  if(IsInRange())
  {
      timeLastSawPlayer = 0;
      
      //移動
       
      animator.SetBool("IsConfuse", false);
      mover.MoveTo(player.transform.position, 1);
      transform.LookAt(player.transform.position); 
      
  }
  else if(timeLastSawPlayer < confuseTime)
  {
    mover.CancleMove();
    //困惑
    animator.SetBool("IsConfuse", true);
  }
  else
  {
    //回到原點
    animator.SetBool("IsConfuse", false);
    mover.MoveTo(beginPosition,0.2f);
  }

  UpdateTimer();

  }

  //是否小於追趕距離
  private bool IsInRange()
  {
    return Vector3.Distance(transform.position,player.transform.position) < chaseDistance;
  }

  private void UpdateTimer()
  {
    timeLastSawPlayer += Time.deltaTime;
  }
}
