using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Ingredient : UI_Base
{
    private float _speed = 500.0f;
    public Ingredient Ingredient { get; set; }

    public override void Init()
    {
        gameObject.BindEvent(OnClick);
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        transform.position += new Vector3(_speed, 0.0f, 0f) * Time.deltaTime;
        if (transform.position.x > 1500.0f)
            Managers.Resource.Destroy(gameObject);
    }
    void OnClick(PointerEventData evt)
    {
        Managers.Resource.Destroy(gameObject);
    }
}
