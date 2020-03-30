using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdAndFirstPersonCamera : MonoBehaviour
{
    public bool isThirdPerson = true;

    private float distance = 10f;
    private float ySpeed = 50f;
    private float yMinLimit = -20f;
    private float yMaxLimit = 50f;
    private float heightOffset = 7f;

    private float mouse_y = 0f;
    // Start is called before the first frame update

    public void RotateWithTarget(float y, GameObject target)
    {


        Quaternion rotation;
        Vector3 position;

        if (isThirdPerson)
        {
            mouse_y -= y * Time.deltaTime * ySpeed;
            mouse_y = ClampAngel(mouse_y, yMinLimit, yMaxLimit);

            rotation = Quaternion.Euler(mouse_y, target.transform.localEulerAngles.y, 0);
            position = rotation * new Vector3(5f, heightOffset, -distance) + target.transform.position;
        }
        else
        {
            mouse_y -= y * Time.deltaTime * ySpeed;
            mouse_y = ClampAngel(mouse_y, 0, yMaxLimit);

            rotation = Quaternion.Euler(mouse_y, target.transform.localEulerAngles.y, 0);
            position = Quaternion.Euler(0, target.transform.localEulerAngles.y, 0) * new Vector3(0, 7f, 0.5f) + target.transform.position;
        }
          

        transform.rotation = rotation;
        transform.position = Vector3.Lerp(transform.position, position, 0.6f);  
    }

    public void FixView(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    private float ClampAngel(float angel, float min, float max)
    {
        while (angel < -360)
            angel += 360;
        while (angel > 360)
            angel -= 360;

        return Mathf.Clamp(angel, min, max);
    }
}
