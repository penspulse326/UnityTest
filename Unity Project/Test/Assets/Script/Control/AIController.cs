using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour
{
  [SerializeField] float chaseDistance = 10f;
  [SerializeField] float confuseTime = 5f;


  GameObject player;

  //起始位置
  Vector3 beginPosition;

  //上次看到玩家的時間
  private float timeLastSawPlayer = Mathf.Infinity;

  Mover mover;

  private void Awake()
  {
    mover = GetComponent<Mover>();
    player = GameObject.FindGameObjectWithTag("Player");
    beginPosition = transform.position;
  }

  private void Update() {

  if(IsInRange())
  {
    timeLastSawPlayer = 0;
    //移動
    mover.MoveTo(player.transform.position, 1 );
  }
  else if(timeLastSawPlayer < confuseTime)
  {
    mover.CancleMove();
    //困惑
  }
  else
  {
    mover.MoveTo(beginPosition,0.5f);
  }


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
