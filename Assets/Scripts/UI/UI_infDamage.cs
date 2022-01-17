using UnityEngine;
using UnityEngine.UI;

public class UI_infDamage : MonoBehaviour
{

    [SerializeField] private GameObject GameObjectDamage;
    private Image ImageDamage;

    [Header("GradientColorForDamage")]
    [SerializeField] private Color c_DamageEnemy;
    [SerializeField] private Color c_KillEnemy;
    [SerializeField] private Color c_DamageHero;

    private void Start()
    {
        ImageDamage = GameObjectDamage.GetComponent<Image>();
    }


    public void DamageEnemy()
    {
        ImageDamage.color = c_DamageEnemy;
        GameObjectDamage.SetActive(true);
    }

    public void KillEnemy()
    {
        ImageDamage.color = c_KillEnemy;
        GameObjectDamage.SetActive(true);
    }

    public void DamageHero()
    {
        ImageDamage.color = c_DamageHero;
        GameObjectDamage.SetActive(true);
    }
}
