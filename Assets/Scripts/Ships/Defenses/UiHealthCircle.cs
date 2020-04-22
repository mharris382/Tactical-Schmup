using Ships.Defenses;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UiHealthCircle : MonoBehaviour
{
    [ValidateInput("valTarget")]
    public GameObject healthTarget;

   [Required] public Image fillBar;
    [Range(0,1)]
    public float fillRange = 0.5f;
    
    #region [Editor Helpers]

    bool valTarget(GameObject go, ref string msg)
    {
        if (go == null)
        {
            msg = "Health Target is required!";
            return false;
        }

        msg = "Health Target must have IHealthLayer component attached";
        return go.GetComponent<IHealthLayer>()!=null;
    }

    #endregion


    private IHealthLayer _health;

    private void Awake()
    {
        _health = healthTarget.GetComponent<IHealthLayer>();
        _health.OnCurrentHPChanged += HealthOnOnCurrentHPChanged;
        HealthOnOnCurrentHPChanged(_health.MaxHP);
    }

    private void HealthOnOnCurrentHPChanged(float currentHealth)
    {
        float p = currentHealth / _health.MaxHP;
        p = Mathf.Lerp(0, fillRange, p);
        fillBar.fillAmount = p;
    }
}
