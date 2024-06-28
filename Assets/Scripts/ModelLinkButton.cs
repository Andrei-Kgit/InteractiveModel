using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class ModelLinkButton : MonoBehaviour
{
    [SerializeField] private Toggle _modelActivityToggle;
    [SerializeField] private Toggle _modelVisabilityToggle;
    [SerializeField] private TMP_Text _modelsName;
    private Button _button;
    private ModelPropertyChanger _modelPropertyChanger;
    public Button LinkButton => _button;
    public Toggle VisabilityButton => _modelVisabilityToggle;
    public Toggle LinkToggle => _modelActivityToggle;

    public void Init(ModelPropertyChanger modelPropertyChanger, string modelsName)
    {
        _button = GetComponent<Button>();
        _modelPropertyChanger = modelPropertyChanger;
        _modelsName.text = modelsName;
        _modelActivityToggle.onValueChanged.AddListener(x => _modelPropertyChanger.IsActiveModel = x);
        _modelVisabilityToggle.onValueChanged.AddListener(x => _modelPropertyChanger.SetVisability(x));
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
        _modelActivityToggle.onValueChanged.RemoveAllListeners();
        _modelVisabilityToggle.onValueChanged.RemoveAllListeners();
    }

    public void SetModelActivityToggle(bool state)
    {
        _modelActivityToggle.isOn = state;
    }

    public void SetModelVisabilityToggle(bool state)
    {
        _modelVisabilityToggle.isOn = state;
    }
}
