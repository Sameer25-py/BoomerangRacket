using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    public float     Radius;
    public Vector2   Center;
    public LayerMask LayerMask;
    public bool      IsGameStarted = false;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        Vector3 mousePosition       = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 targetPosition      = mousePosition;
        Vector2 direction           = targetPosition - Center;
        Vector2 normalizedDirection = direction.normalized;

        RaycastHit2D hit2D = Physics2D.Raycast(normalizedDirection * (Radius * 2f), -direction,
            Radius                                                 * 2f, LayerMask);
        if (!hit2D) return;
        transform.position = hit2D.point;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}