using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [Serializable]
    public struct CharConversion
    {
        public String character;
        public GameObject sprite;
    }
    
    public delegate void SimpleCallback();
    private SimpleCallback m_dialogCallback;
    
    [SerializeField] private float m_waitBeforeSkip = 0.2f;
    [SerializeField] private Transform m_textBase;
    [SerializeField] private float m_pixelSize;
    [SerializeField] private int m_lineMargin;
    [SerializeField] private int m_dialogLength;
    [SerializeField] private Transform m_arrow;
    [SerializeField] private List<CharConversion> m_charConversion;
    [SerializeField] private List<CharConversion> m_redCharConversion;
    
    private float m_drawDialogDuration;
    private bool m_isRedWord;
    private int m_visibleChars;
    private float m_visibleCharTime;
    private string m_dialogToDraw;
    
    private List<Letter> m_letters;

    private void Awake()
    {
        m_letters = new List<Letter>();
    }

    private void Update()
    {
        m_drawDialogDuration += Time.unscaledDeltaTime;
        m_visibleCharTime -= Time.unscaledDeltaTime;
        if(m_visibleCharTime < 0.0f && m_visibleChars < m_letters.Count)
        {
            m_visibleCharTime = 0.05f;
            m_letters[m_visibleChars].gameObject.SetActive(true);
            ++m_visibleChars;   
            
            if(m_visibleChars >= m_letters.Count)
                m_arrow.gameObject.SetActive(true);
        }
    }

    public void StartDialog(string _text, SimpleCallback _callback)
    {
        m_drawDialogDuration = 0.0f;
        m_dialogToDraw = _text;
        m_dialogCallback = _callback;
        DrawText();
        Controller.OnContinuePress += Continue;
        GameManager.Pause();
    }

    public void EndDialog()
    {
        Controller.OnContinuePress -= Continue;
        gameObject.SetActive(false);
        m_dialogCallback?.Invoke();
        m_dialogCallback = null;
    }
    
    public void Continue()
    {
        if (m_drawDialogDuration < m_waitBeforeSkip) return;
        if (m_visibleChars >= m_letters.Count)
        {
            EndDialog();
        }
        else
        {
            while (m_visibleChars < m_letters.Count)
            {
                m_letters[m_visibleChars].gameObject.SetActive(true);
                ++m_visibleChars;
            }
            m_arrow.gameObject.SetActive(true);
        }
    }

    private void ResetText()
    {
        if (m_letters != null)
        {
            foreach (var letter in m_letters)
            {
                Destroy(letter.gameObject);
            }
        }
        
        m_visibleChars = 0;

        m_letters = new List<Letter>();
        gameObject.SetActive(false);
    }

    public void DrawText()
    {
        ResetText();
        gameObject.SetActive(true);

        Vector2 pos = Vector2.zero;
        m_isRedWord = false;
        int remainingLength = m_dialogLength;
        
        foreach(char _character in m_dialogToDraw)
        {
            if (_character == '/')
            {
                m_isRedWord = !m_isRedWord;
                continue;
            }
            GameObject letterObject = Instantiate(SelectChar(_character), m_textBase);
            Letter letter = letterObject.GetComponent<Letter>();
            letter.transform.localPosition = pos * m_pixelSize;
            letter.gameObject.SetActive(false);
            m_letters.Add(letter);
        
            pos.x += letter.size;
            remainingLength -= letter.size;

            if (remainingLength < 3)
            {
                pos.x = 0;
                pos.y -= m_lineMargin;
                remainingLength = m_dialogLength;
            }
        }
        m_arrow.gameObject.SetActive(false);
    }

    private GameObject SelectChar(char _character)
    {
        char lowerChar = char.ToLower(_character);
        if(m_isRedWord) return m_redCharConversion.Find(x => x.character.Contains(lowerChar)).sprite;
        return m_charConversion.Find(x => x.character.Contains(lowerChar)).sprite;
    }
}
