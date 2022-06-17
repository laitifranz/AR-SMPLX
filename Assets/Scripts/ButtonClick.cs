using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour
{
    public int value_button = -1;

	public Button warmUpButton;
	public Button ex1Button;
	public Button ex2Button;
	public Button stretchingButton;

	void Start()
	{
		warmUpButton.GetComponent<Button>().onClick.AddListener(() => { SetValue(0); });
		ex1Button.GetComponent<Button>().onClick.AddListener(() => { SetValue(1); });
		ex2Button.GetComponent<Button>().onClick.AddListener(() => { SetValue(2); });
		stretchingButton.GetComponent<Button>().onClick.AddListener(() => { SetValue(3); });

	}

	void SetValue(int value)
    {
		value_button = value;
    }
}
