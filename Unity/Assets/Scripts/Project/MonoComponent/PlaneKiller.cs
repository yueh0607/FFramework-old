using FFramework;
using FFramework.MVVM.RefCache;
using FFramework.MVVM.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class PlaneKiller : MonoBehaviour
{

    [SerializeField] string NameStartWith = "Enemy";
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != null && other.gameObject.name.StartsWith(NameStartWith))
        {
            Debug.Log("Kill");
            UI.Show<GameOverPanelVM, GameOverPanel>().Forget();           
        }
    }


}
