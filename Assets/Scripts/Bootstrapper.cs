using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    private static ServiceLocator Locator => ServiceLocator.Instance;


    private async void Start()
    {
        var linksStorage = new LinksStorage();
        Locator.Add(linksStorage);
    }

}
