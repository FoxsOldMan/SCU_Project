using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public float lifeTime = 30f;
    private float counter = 0;
    private Light[] myLights;

    private void OnEnable()
    {
        myLights = GetComponentsInChildren<Light>();        
        counter = 0;

    }

    // Update is called once per frame
    void Update()
    {

        if (myLights[0].enabled)
        {
            counter += Time.deltaTime;
            if (counter >= lifeTime)
                Destroy(gameObject);
        }
 
    }

    public bool Switch()
    {
        if (myLights != null)
        {
            gameObject.SetActive(!gameObject.activeSelf);
            foreach (Light light in myLights)
            {
                light.enabled = !light.enabled;
            }
            return true;
        }

        return false;
    }

    public bool LightTorch()
    {
        if(myLights != null)
        {
            gameObject.SetActive(true);
            foreach (Light light in myLights)
            {

                light.enabled = true;

            }
            return true;
        }

        return false;
    }
}
