using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ingredient : UI_Base
{


    public string IngredientName { get; set; }

    private float _moveSpeed = 500.0f;
    private RectTransform _rectTransform;
    private bool _isClick = false;

    private float _putSpeed = 2.0f;
    private float _deltaTime = 0.0f;
    private Vector2 _p0;
    private Vector2 _p1;
    private Vector2 _p2;

    public override void Init()
    {
        gameObject.BindEvent(OnClick);
        _rectTransform = GetComponent<RectTransform>();
        _isClick = false;
        _deltaTime = 0.0f;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (_isClick)
        {
            _deltaTime += Time.deltaTime * _putSpeed;
            Vector2 _p01 = Vector2.Lerp(_p0, _p1, _deltaTime);
            Vector2 _p12 = Vector2.Lerp(_p1, _p2, _deltaTime);
            _rectTransform.anchoredPosition = Vector2.Lerp(_p01, _p12, _deltaTime);
            transform.localScale = Vector2.Lerp(new Vector2(1f, 1f), new Vector2(0f, 0f), _deltaTime);

            if (Mathf.Abs(_rectTransform.anchoredPosition.y - _p2.y) < 0.001f)
            {
                Managers.UI.SceneTail.PutIngredient(IngredientName);
                Managers.Resource.Destroy(gameObject);
            }
        }
        else
        {
            _rectTransform.anchoredPosition += new Vector2(_moveSpeed, 0.0f) * Time.deltaTime;
            if (_rectTransform.anchoredPosition.x > 700.0f)
                Managers.Resource.Destroy(gameObject);
        }
    }

    void OnClick(PointerEventData evt)
    {
        if (_isClick)
            return;

        _isClick = true;
        _p0 = _rectTransform.anchoredPosition;
        _p1 = new Vector2(0f, 200f);
        _p2 = new Vector2(0f, -1150f - Managers.UI.SceneHead.OffsetY);
    }
}
