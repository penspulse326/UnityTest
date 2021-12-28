using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    int numssss;
    //wayPoint半徑
    [SerializeField] float wayPointGizmosRadius = 1f;

    //取得下一個WayPoint的編號
    public int GetNextWayPointNumber(int wayPointNumber){
        if(wayPointNumber + 1 > transform.childCount - 1)
        {
            return 0;
        }

        return wayPointNumber + 1;
    }

    //取得WayPoint的位置
    public Vector3 GetWayPointPosition(int wayPointNumber)
    {
        return transform.GetChild(wayPointNumber).position;
    }

    //call by Unity
    //繪製可視化物件 怪物的巡邏路徑
    private void OnDrawGizmos()
    {
        for(int i =0 ; i < transform.childCount ; i++)
        {
            Gizmos.color = Color.green;
            int j = GetNextWayPointNumber(i);
            Gizmos.DrawLine(GetWayPointPosition(i),GetWayPointPosition(j));
            Gizmos.DrawSphere(GetWayPointPosition(i),wayPointGizmosRadius);
        }
    }
}
