using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [Header("Identity")]
    [Tooltip("What type of weapon this model represents.")]
    public WeaponType weaponType;
    [SerializeField] private Transform gunPoint;
    public Transform GunPoint => gunPoint;
}
