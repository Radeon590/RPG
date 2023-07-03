using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public class StaminaController : MonoBehaviour
{
    [SerializeField] Slider StaminaBar;
    [SerializeField] PostProcessVolume postProcessVolume;

    ChromaticAberration chromaticAberration;

    public static float staminaMax = 100;
    public static float stamina = 100;

    public static bool isRunning = false;
    public static bool isAiming = false;
    public static bool isHeroTired = false;

    float tirednessCooldownTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        postProcessVolume.profile.TryGetSettings(out chromaticAberration);
        StaminaBar.maxValue = staminaMax;
        stamina = staminaMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameUIContr.paused) 
        {
            StaminaBar.value = stamina;
            //мы увеличиваем интенсвиность аберации с усталостью героя
            chromaticAberration.intensity.value = 1 - (stamina / 100);

            //check tiredness
            if (stamina < 1)
            {
                isRunning = false;
                isAiming = false;
                isHeroTired = true;
            }



            //если герой устал, то это должно наложить свои эффекты и дебафы
            if (isHeroTired == true)
            {
                MoveScript.moveSpeed = 0.06f;
                tirednessCooldownTimer += Time.deltaTime;
                stamina += 2.5f * Time.deltaTime;

                if (tirednessCooldownTimer > 10)
                {
                    isHeroTired = false;
                    tirednessCooldownTimer = 0;
                }
            }
            else
            {
                //тратим стамину, если она где-то используется
                if (isAiming)
                    stamina -= 2 * Time.deltaTime;
                if (isRunning)
                    stamina -= 2 * Time.deltaTime;

                //или восстанавливаем
                if (!isAiming && !isRunning)
                    stamina += 1 * Time.deltaTime;

                if (stamina > staminaMax)
                    stamina = staminaMax;
                if (stamina < 0)
                    stamina = 0;
            }
        }
    }
}
