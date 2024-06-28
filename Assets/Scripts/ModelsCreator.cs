using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ModelsInteractor))]
public class ModelsCreator : MonoBehaviour
{
    [SerializeField] private Transform _modelsHolder;
    [SerializeField] private List<GameObject> _modelsPrefabs;
    [SerializeField] private GameObject _addModelButtonPrefab;
    [SerializeField, Range(0, 10)] private int _startModelsCount;
    private Button _addModelButton;
    private Vector3 _newModelsPos = Vector3.zero;
    private ModelsInteractor _interactor;
    private const string BASEMODELNAME = "Model";
    private int _modelIndex = 1;

    void Start()
    {
        _interactor = GetComponent<ModelsInteractor>();
        _interactor.OnLoad += PrepareScene;
        _interactor.OnClear += ResetModelIndex;
        SpawnModelAddingButton(_addModelButtonPrefab);
        _addModelButton.onClick.AddListener(SpawnNewModel);

        for (int i = 0; i < _startModelsCount; i++)
        {
            SpawnNewModel();
        }
    }

    private void ResetModelIndex()
    {
        _modelIndex = 1;
    }

    private void PrepareScene(Property[] savedModels)
    {
        if (savedModels != null && savedModels.Length >= 4)
        {
            LoadModels(savedModels);
        }
        else
        {
            for (int i = 0; i < _startModelsCount; i++)
            {
                SpawnNewModel();
            }
        }
    }

    private void SpawnModelAddingButton(GameObject button)
    {
        _addModelButton = _interactor.SpawnButton(button);
    }

    private void LoadModels(Property[] modelsToLoad)
    {
        foreach (var loadedModel in modelsToLoad)
        {
            GameObject model = Instantiate(_modelsPrefabs[loadedModel.MeshType], _modelsHolder);
            ModelPropertyChanger propertyChanger = model.AddComponent<ModelPropertyChanger>();

            Vector3 modelPos = new Vector3(loadedModel.X, loadedModel.Y, loadedModel.Z);
            propertyChanger.Init(loadedModel.Name);
            propertyChanger.ModelProperty.MeshType = loadedModel.MeshType;
            propertyChanger.SetPosition(modelPos);
            _newModelsPos = modelPos;
            propertyChanger.SetColor(loadedModel.R, loadedModel.G, loadedModel.B);
            propertyChanger.SetAlpha(loadedModel.A);
            propertyChanger.IsActiveModel = true;

            _interactor.SetLinkModel(propertyChanger, loadedModel.Name);
        }
    }

    private void SpawnNewModel()
    {
        int meshType = Random.Range(0, _modelsPrefabs.Count);
        GameObject model = Instantiate(_modelsPrefabs[meshType], _modelsHolder);
        ModelPropertyChanger propertyChanger = model.AddComponent<ModelPropertyChanger>();
        propertyChanger.ModelProperty.MeshType = meshType;
        propertyChanger.SetPosition(_newModelsPos);
        _newModelsPos += (Vector3.right * 10f);
        string name = $"{BASEMODELNAME} {_modelIndex++}";
        propertyChanger.Init(name);
        propertyChanger.IsActiveModel = true;
        _interactor.SetLinkModel(propertyChanger, name);
    }

    private void OnDisable()
    {
        _interactor.OnLoad -= PrepareScene;
        _interactor.OnClear -= ResetModelIndex;
        _addModelButton.onClick.RemoveAllListeners();
    }
}
