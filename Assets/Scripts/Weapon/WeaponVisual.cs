using UnityEngine;

public class WeaponVisual : MonoBehaviour
{
    [SerializeField] private Transform gunPoint;

    public Transform GunPoint => gunPoint;
}
