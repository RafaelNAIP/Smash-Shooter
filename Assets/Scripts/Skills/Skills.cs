using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class Skills : MonoBehaviour
{
    public float cd_1 = 5f;
    public Image[] skill_Image;
    void Start()
    {
        cd_1 = 5f;
    }

 void Update()
    {        
        if (Keyboard.current[Key.Digit1].wasPressedThisFrame && skill_Image[0].fillAmount >= 1f)
        {            
            skill_Image[0].fillAmount = 0f;
            ActivateSkill1();
        }

        if (skill_Image[0].fillAmount < 1f)
        {
            skill_Image[0].fillAmount += Time.deltaTime / cd_1;
        }
    }   

    void ActivateSkill1()
    {
        Debug.Log("Skill 1 activated!");
    }
}
