using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerRtsShip))]
public class PlayerRtsShip : MonoBehaviour
{
    [Required]
    [SerializeField] private GameObject selection = null;
    public bool IsSelected
    {
        set
        {
            selection.gameObject.SetActive(value);
        }
    }
    
    
    
}