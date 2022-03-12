using System;
using System.Collections.Generic;
using Commands;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardState
    {
        Hand,
        Float,
        Placed,
        Attacking,
    }
    
    public float snapSpeed = 5;
    private Vector3 _velocity = Vector3.zero;
    private BasicCard _target;
    private BasicCard _attackTarget;

    private Transform _cardTransform;
    public BasicCard basicCardInfo;


    private Camera _cachedCamera;

    public Vector3 returnPosition;

    public CardInfo info;
    public CardState state = CardState.Hand;
    public CardStats stats;
    public GameStatus.Side side;

    public bool canAttack;

    private EffectHandler _effectHandler;

    private Plane _placingPlane = new Plane(Vector3.forward, 1);
    private Plane _attackingPlane = new Plane(Vector3.forward, 0);

    private void Awake()
    {
        _cardTransform = transform;
        basicCardInfo = GetComponent<BasicCard>();
    }

    private void ApplyEffectsOnPlaced(PlaceCommand command)
    {
        if (command.WhatToPlace != this) return;
        
        _effectHandler = new EffectHandler(this, info.effects);

        PlaceCommand.Callbacks -= ApplyEffectsOnPlaced;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _cachedCamera = Camera.main;
        stats = info.stats;

        PlaceCommand.Callbacks += ApplyEffectsOnPlaced;
    }

    private void OnDestroy()
    {
        _effectHandler?.Cleanup();
    }

    // Update is called once per frame
    private void Update()
    {
        if (stats.health <= 0)
        {
            int index = basicCardInfo.parent.cardSlots.IndexOf(basicCardInfo);
            GameManager.CommandQueue.Enqueue(new DestroyCommand(basicCardInfo));
            // basicCardInfo.parent.AddCard(basicCardInfo.parent.GeneratePlaceholder(), index);
            if(basicCardInfo.parent.hasPlaceholder)
                GameManager.CommandQueue.Enqueue(new AddCommand(
                basicCardInfo.parent.GeneratePlaceholder(), basicCardInfo.parent, index));
            return;
        }
        
        Vector3 newPos;
        Vector3 position = _cardTransform.position;
        switch (state)
        {
            case CardState.Float:
            {
                Ray ray = _cachedCamera.ScreenPointToRay(Input.mousePosition);
                Vector3 mousePosInWorld = _placingPlane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : Vector3.zero;
                mousePosInWorld.z = 0;
                _target = GameManager.GetClosestPlaceholder(mousePosInWorld, side);
                mousePosInWorld.z = -1;
                if (!(_target is null))
                {
                    Vector3 targetPos = _target.cardTransform.position;
                    targetPos.z = 0;
                    newPos = Vector3.SmoothDamp(position, targetPos, ref _velocity,
                        snapSpeed);
                }
                else
                {
                    newPos = Vector3.SmoothDamp(position, mousePosInWorld, ref _velocity, snapSpeed);
                }
                break;
            }
            case CardState.Placed:
            case CardState.Hand:
            {
                newPos = Vector3.SmoothDamp(position, returnPosition, ref _velocity,
                    snapSpeed);
                break;
            }
            default:
                newPos = position;
                break;
        }

        // newPos.z = position.z;
        _cardTransform.position = newPos;

        if (state != CardState.Attacking) return;
        {
            Ray ray = _cachedCamera.ScreenPointToRay(Input.mousePosition);
            Vector3 mousePosInWorld = _attackingPlane.Raycast(ray, out float distance) ? ray.GetPoint(distance) : Vector3.zero;
            _attackTarget = GameManager.GetClosestPlaceholder(mousePosInWorld, 
                side == GameStatus.Side.Friendly ? GameStatus.Side.Enemy : GameStatus.Side.Friendly, true);
            GameManager.SetAttackIndicatorEnd(_attackTarget?.cardTransform.position ?? mousePosInWorld);
        }
    }

    private void OnMouseDown()
    {
        switch (state)
        {
            // if (side == GameManager.Side.Enemy) return;
            case CardState.Hand:
            {
                // _returnPosition = _cardTransform.position;
                state = CardState.Float;
                List<CardRow> rows = side == GameStatus.Side.Friendly ? GameManager.FriendlyRows : GameManager.EnemyRows;
                rows.ForEach(x => x.ShowPlaceholders());
                break;
            }
            case CardState.Placed when !canAttack:
                return;
            case CardState.Placed:
                state = CardState.Attacking;
                GameManager.ToggleAttackIndicator(true);
                GameManager.SetAttackIndicatorBegin(_cardTransform.position);
                break;
            case CardState.Float:
                break;
            case CardState.Attacking:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnMouseUp()
    {
        switch (state)
        {
            // if (side == GameManager.Side.Enemy) return;
            case CardState.Float:
            {
                List<CardRow> rows = side == GameStatus.Side.Friendly ? GameManager.FriendlyRows : GameManager.EnemyRows;

                rows.ForEach(x => x.HidePlaceholders());

                if (_target is { })
                {
                    GameManager.CommandQueue.Enqueue(new PlaceCommand(this, _target));
                }
                else
                {
                    state = CardState.Hand;
                }

                break;
            }
            case CardState.Attacking:
                state = CardState.Placed;
                GameManager.ToggleAttackIndicator(false);
                if (_attackTarget is { })
                {
                    GameManager.CommandQueue.Enqueue(new AttackCommand(this, _attackTarget.cardComponent));
                }
                break;
            case CardState.Hand:
                break;
            case CardState.Placed:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
