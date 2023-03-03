using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class carrierSystem : MonoBehaviour, InputMap.IMapActions
{
    private InputMap input;
    private InputAction Fire;
    private InputAction Scroll;

    [SerializeField] private Transform RigSocket;
    [SerializeField] private Animator RigAnimator;
    public List<HandheldScriptableObject> EquippableHandhelds;

    private HandheldScriptableObject currentHandheld;
    private GameObject currentHandheldGameObject;
    private HandheldObject currentHandheldInterface;

    private int currentHandheldIndex;

    private void Awake()
    {
        SwitchHandheld(EquippableHandhelds[0]);

        input = new InputMap();
        input.Enable();
        Fire = input.Map.Fire;
        Fire.Enable();
        Scroll = input.Map.Scroll;
        Scroll.Enable();

        Fire.performed += OnFire;
        Scroll.performed += OnScroll;
    }

    public void SwitchHandheld(HandheldScriptableObject handheld)
    {
        if(currentHandheld == handheld)
        {
            return;
        }

        Destroy(currentHandheldGameObject);
        currentHandheld = handheld;
        currentHandheldGameObject = Instantiate(currentHandheld.HandheldPrefab, RigSocket, true);
        currentHandheldGameObject.transform.localPosition = Vector3.zero;
        currentHandheldGameObject.transform.localRotation = Quaternion.identity;

        currentHandheldInterface = currentHandheldGameObject.GetComponentInChildren<HandheldObject>();
        if(currentHandheldInterface != null)
        {
            currentHandheldInterface.OnAttachedCarrier(this);
            currentHandheldInterface.OnEquip();

        }
    }
    public void OnFire(InputAction.CallbackContext context)
    {
        if (currentHandheldInterface != null)
        {
            currentHandheldInterface.OnFire(context);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (currentHandheldInterface != null)
        {
            currentHandheldInterface.OnJump(context);
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (currentHandheldInterface != null)
        {
            currentHandheldInterface.OnLook(context);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (currentHandheldInterface != null)
        {
            currentHandheldInterface.OnMove(context);
        }
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            currentHandheldIndex += (int)Mathf.Sign(context.ReadValue<float>());
        }
        if(currentHandheldIndex < 0)
            currentHandheldIndex = EquippableHandhelds.Count - 1;

        if(currentHandheldIndex > EquippableHandhelds.Count - 1)
            currentHandheldIndex = 0;

        SwitchHandheld(EquippableHandhelds[currentHandheldIndex]);
    }
}
