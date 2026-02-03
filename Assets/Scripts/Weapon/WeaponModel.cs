using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("What type of weapon this model represents.")]
    public WeaponType weaponType;

    [Header("Sockets")]
    [Tooltip("Where bullets originate from on this model.")]
    [SerializeField] private Transform gunPoint;

    public Transform GunPoint => gunPoint;

    private void OnValidate()
    {
        // Helps catch missing references early in the editor.
        if (gunPoint == null)
            Debug.LogWarning($"{name}: GunPoint is not assigned on WeaponModel.", this);
    }
}
