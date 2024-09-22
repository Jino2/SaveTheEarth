using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

public class DropItem : MonoBehaviourPun, IDroppable
{
    [SerializeField] private float droppedScale = 0.1f;
    [SerializeField] private float dropPower = 1f;

    [SerializeField] private float floatingHeight = 1f;

    [SerializeField] private float floatingRange = 0.5f;

    [SerializeField] private float floatingSpeed = 1.0f;

    [SerializeField] private float pickUpOffset = 1f;

    [SerializeField] private float pickUpSpeed = 1.0f;

    private GoodsInfo goodsInfo;

    private float elapsedTime = 0f;

    private ICollectible _collectible;
    private GameObject _collectedTargetObj;

    private bool isDropping = false;
    private Vector3 velocity;

    private IDroppable.DropState _state = IDroppable.DropState.None;

    IDroppable.DropState IDroppable.State
    {
        get => _state;
        set => _state = value;
    }

    protected void Start()
    {
        goodsInfo = GetComponent<GoodsInfo>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            case IDroppable.DropState.Drop:
                UpdateDrop();
                break;
            case IDroppable.DropState.PickingUp:
                UpdatePickingUp();
                break;

            case IDroppable.DropState.None:
                print("None");
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateDrop()
    {
        if (isDropping)
        {
            UpdateDropping();
        }
        else
        {
            UpdateDropped();
        }
    }

    private void UpdateDropping()
    {
        transform.position += velocity * Time.deltaTime;
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, floatingHeight, LayerMask.GetMask("Ground")))
        {
            isDropping = false;
            return;
        }
    }

    private void FixedUpdate()
    {
        if (isDropping)
        {
            velocity += Physics.gravity * Time.fixedDeltaTime;
        }
    }

    private void UpdatePickingUp()
    {
        var targetPos = _collectedTargetObj.transform.position + Vector3.up * pickUpOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * pickUpSpeed);
        if (Vector3.Distance(transform.position, targetPos) < 0.1f)
        {
            if (goodsInfo != null)
            {
                _collectible.Collect((int)goodsInfo.goodsType);
            }

            Destroy(gameObject);
        }
    }

    private void UpdateDropped()
    {
        elapsedTime += Time.deltaTime;
        var newPosY = Mathf.PingPong(elapsedTime * floatingSpeed, floatingRange) + floatingHeight;
        transform.position = new Vector3(transform.position.x, newPosY, transform.position.z);


        var colliders = Physics.OverlapSphere(transform.position, pickUpOffset, 1 << LayerMask.NameToLayer("Player"));
        foreach (var col in colliders)
        {
            print("PickedUpBy : " + col.gameObject.name);
            PickedUpBy(col);
        }
    }

    private void PickedUpBy(Collider other)
    {
        if (_state == IDroppable.DropState.PickingUp) return;
        if (!other.gameObject.TryGetComponent(out _collectible))
        {
            return;
        }


        _collectedTargetObj = other.gameObject;
        _state = IDroppable.DropState.PickingUp;
    }


    public void Drop()
    {
        photonView.RPC("RPC_Drop", RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Drop()
    {
        if (_state == IDroppable.DropState.Drop) return;
        _state = IDroppable.DropState.Drop;
        velocity = transform.forward * dropPower;
        transform.localScale = Vector3.one * droppedScale;
        isDropping = true;
        elapsedTime = 0f; 
    }
}