using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ingredient : UI_Base
{
    private float _speed = 500.0f;
    private RectTransform _rectTransform;
    public Ingredient Ingredient { get; set; }

    public override void Init()
    {
        gameObject.BindEvent(OnClick);
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        _rectTransform.anchoredPosition += new Vector2(_speed, 0.0f) * Time.deltaTime;
        if (_rectTransform.anchoredPosition.x > 700.0f)
            Managers.Resource.Destroy(gameObject);
    }
    void OnClick(PointerEventData evt)
    {
        Managers.Resource.Destroy(gameObject);
    }
}
