using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();
        Color tmp = new Color(Random.Range(0,.8f), Random.Range(0,.8f), Random.Range(0,.8f));
        image.color = tmp;
    }
}
