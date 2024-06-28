using System;
using UnityEngine;

[System.Serializable]
public class Property
{
    public string Name;
    public float X, Y, Z;
    public float R = 255, G = 255, B = 255, A = 255;
    public int MeshType;
}

[RequireComponent(typeof(MeshRenderer))]
public class ModelPropertyChanger : MonoBehaviour
{
    public Property ModelProperty { get; set; } = new Property();
    public bool IsActiveModel { get; set; }
    public string Name { get; set; }
    private MeshRenderer _meshRenderer;
    private Material _material;

    public void Init(string name, Property property = null)
    {
        if (property != null)
            ModelProperty = property;

        _meshRenderer = GetComponent<MeshRenderer>();
        _material = _meshRenderer.material;

        if (string.IsNullOrEmpty(ModelProperty.Name))
        {
            Name = name;
            ModelProperty.Name = name;
        }
        else
        {
            Name = ModelProperty.Name;
        }
    }

    public void SetPosition(Vector3 pos)
    {
        transform.position = pos;

        ModelProperty.X = pos.x;
        ModelProperty.Y = pos.y;
        ModelProperty.Z = pos.z;
    }

    public void SetColor(float r, float g, float b)
    {
        ModelProperty.R = r;
        ModelProperty.G = g;
        ModelProperty.B = b;

        Color newColor = new Color(r, g, b) / 255;
        newColor.a = _material.color.a;
        _material.color = newColor;
    }

    public void SetAlpha(float a)
    {
        ModelProperty.A = a;

        Color newColor = _material.color;
        newColor.a = a / 255;
        _material.color = newColor;
    }

    public void SetVisability(bool state)
    {
        gameObject.SetActive(state);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
