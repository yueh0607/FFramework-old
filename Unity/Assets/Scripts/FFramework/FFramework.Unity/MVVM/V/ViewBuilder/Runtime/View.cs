using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FFramework.MVVM
{
    public abstract class View : MonoBehaviour
    {
       public IViewModel ViewModelReference { get; set; }
    }
}

namespace FFramework.MVVM.RefCache { }