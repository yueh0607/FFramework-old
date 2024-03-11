using FFramework;
using FFramework.MVVM.RefCache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MisslKiller : MonoBehaviour
{

    [SerializeField] string NameStartWith = "Enemy";
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject!=null&&other.gameObject.name.StartsWith(NameStartWith))
        {
            Debug.Log("Kill");
            ((EnemyVM)other.gameObject.GetComponent<Enemy>().ViewModelReference).Kill().Forget();
            ((MissileVM)(GetComponent<Missile>().ViewModelReference)).Kill().Forget();
           
        }
    }


}
