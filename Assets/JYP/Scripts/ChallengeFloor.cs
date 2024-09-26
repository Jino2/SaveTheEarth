using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeFloor : MonoBehaviour
{
    public Light[] lights;
    public Color[] lightColor;
    
    private int currentLightColorIndex = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void TurnOnLights()
    {
        StopAllCoroutines();
        foreach (var light in lights)
        {
            light.enabled = true;
        }
        
        StartCoroutine(LightColorChange());
    }

    public void TurnOffLights()
    {
        StopAllCoroutines();
        foreach (var light in lights)
        {
            light.enabled = false;
        }
    }
    
    private IEnumerator LightColorChange()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            currentLightColorIndex = (currentLightColorIndex + 1) % lightColor.Length;
            foreach (var light in lights)
            {
                light.color = lightColor[currentLightColorIndex];
            }
        }
    }           
    
}
