using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public TMP_Text nameText;
    public TMP_Text dialogueText;
    public UnityEvent onDialogueFinished;
	public Animator animator;
    public AudioClip talkSFX;
    public AudioClip dialogueEndSFX;

    private Queue<string> _sentences;

	void Awake () {
		_sentences = new Queue<string>();
	}

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
    }

    public void StartDialogue (Dialogue dialogue)
	{
		animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

		_sentences.Clear();

        foreach (string sentence in dialogue.sentences)
		{
			_sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
		if (_sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		string sentence = _sentences.Dequeue();
		StopAllCoroutines();
        SoundManager.Instance.PlaySound(talkSFX, transform.position);
        StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
        dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

    IEnumerator WaitAndEndDialogue()
    {
        animator.SetBool("IsOpen", false);
        SoundManager.Instance.PlaySound(dialogueEndSFX, transform.position);
        yield return new WaitForSeconds(0.7f);
        onDialogueFinished.Invoke();
    }

	void EndDialogue()
	{
        StartCoroutine(WaitAndEndDialogue());

    }

}
