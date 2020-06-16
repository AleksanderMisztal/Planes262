using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIForm : MonoBehaviour
{
    #region [Input Field]

    [SerializeField] private InputField input;

    public string InputFieldText => input.text;

    public void ClearInputField()
    {
        input.text = "";
    }

    #endregion

    #region [Send Button]

    public UnityAction OnSendButtonClicked;

    public void SendButtonClicked()
    {
        Debug.Log("Send button clicked");
        OnSendButtonClicked?.Invoke();
    }

    #endregion

    #region [Server Response]

    [SerializeField] private Text serverResponse;

    public void ShowServerResponse(string message)
    {
        serverResponse.text = $"Server Response:\n{message}";
    }

    #endregion
}