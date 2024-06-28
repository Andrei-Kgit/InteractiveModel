using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ModelsInteractor : MonoBehaviour
{
    public event Action OnClear;
    public event Action OnSave;
    public event Action<Property[]> OnLoad;

    [SerializeField] private Slider _rSlider;
    [SerializeField] private Slider _gSlider;
    [SerializeField] private Slider _bSlider;
    [SerializeField] private Toggle _activeModelsToggle;
    [SerializeField] private Toggle _visibleModelsToggle;
    [SerializeField] private Transform _modelsLinkHolder;
    [SerializeField] private ModelLinkButton _modelLinkButtonPrefab;
    [SerializeField] private CameraControl _cameraControl;

    private string _filePath;
    private List<ModelPropertyChanger> _models = new List<ModelPropertyChanger>();
    private List<ModelLinkButton> _modelLinksButtons = new List<ModelLinkButton>();
    private ModelPropertyChanger _cameraTarget;

    private void Awake()
    {
        _filePath = Application.persistentDataPath + "/models.json";
    }

    private void SetCameraTarget(Transform target)
    {
        _cameraControl.SetTarget(target);
    }

    public Button SpawnButton(GameObject prefab)
    {
        Button button = Instantiate(prefab, _modelsLinkHolder).GetComponent<Button>();
        return button;
    }

    public void SetLinkModel(ModelPropertyChanger modelPropertyChanger, string modelsName)
    {
        ModelLinkButton modelLinkButton = SpawnButton(_modelLinkButtonPrefab.gameObject).GetComponent<ModelLinkButton>();
        modelLinkButton.Init(modelPropertyChanger, modelsName);
        modelLinkButton.LinkButton.onClick.AddListener(delegate { SetCameraTarget(modelPropertyChanger.transform); });
        _modelLinksButtons.Add(modelLinkButton);
        _models.Add(modelPropertyChanger);
        _cameraTarget = modelPropertyChanger;
        SetCameraTarget(_cameraTarget.transform);
    }

    private void SetAllModelsActive(bool state)
    {
        foreach (ModelLinkButton modelLinkButton in _modelLinksButtons)
        {
            modelLinkButton.SetModelActivityToggle(state);
        }
    }

    private void SetAllModelsVisability(bool state)
    {
        foreach (ModelLinkButton modelLinkButton in _modelLinksButtons)
        {
            modelLinkButton.SetModelVisabilityToggle(state);
        }
    }

    private void SetCameraTarget(ModelPropertyChanger modelPropertyChanger)
    {
        _cameraTarget = modelPropertyChanger;
    }

    public void SetAlphaValue(float a)
    {
        foreach (var model in _models)
        {
            if (model.IsActiveModel)
                model.SetAlpha(a);
        }
    }

    public void SetColorValue()
    {
        float r, g, b;
        r = _rSlider.value;
        g = _gSlider.value;
        b = _bSlider.value;
        foreach (var model in _models)
        {
            if (model.IsActiveModel)
                model.SetColor(r, g, b);
        }
    }

    public void SaveScene()
    {
        Property[] properties = new Property[_models.Count];
        for (int i = 0; i < properties.Length; i++)
        {
            properties[i] = _models[i].ModelProperty;
        }
        string dataToSave = JsonHelper.ToJson(properties);
        File.WriteAllText(_filePath, dataToSave);
        OnSave?.Invoke();
        Debug.Log($"Progress saved to {_filePath}");
    }

    public void ClearScene()
    {
        foreach (var model in _models)
        {
            model.Destroy();
        }
        _models.Clear();
        foreach(var modelLink in _modelLinksButtons)
        {
            Destroy(modelLink.gameObject);
        }
        _modelLinksButtons.Clear();
        OnClear?.Invoke();
    }

    public void LoadScene()
    {
        ClearScene();
        if (File.Exists(_filePath))
        {
            string jsonData = File.ReadAllText(_filePath);
            Property[] loadedList = JsonHelper.FromJson<Property>(jsonData);
            OnLoad?.Invoke(loadedList);
        }
        else
        {
            OnLoad?.Invoke(null);
        }
    }

    private void OnEnable()
    {
        _rSlider.onValueChanged.AddListener(delegate { SetColorValue(); });
        _gSlider.onValueChanged.AddListener(delegate { SetColorValue(); });
        _bSlider.onValueChanged.AddListener(delegate { SetColorValue(); });
        _activeModelsToggle.onValueChanged.AddListener(b => SetAllModelsActive(b));
        _visibleModelsToggle.onValueChanged.AddListener(b => SetAllModelsVisability(b));
    }

    private void OnDisable()
    {
        _rSlider.onValueChanged.RemoveAllListeners();
        _gSlider.onValueChanged.RemoveAllListeners();
        _bSlider.onValueChanged.RemoveAllListeners();
        _activeModelsToggle.onValueChanged.RemoveAllListeners();
        _visibleModelsToggle.onValueChanged.RemoveAllListeners();
    }
}
