using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    [SerializeField] private Transform m_textBase;
    [SerializeField] private float m_pixelSize;
    [SerializeField] private int m_lineMargin;
    [SerializeField] private int m_dialogLength;
    [SerializeField] private Transform m_arrow;
    [SerializeField] private List<GameObject> m_blackLetters;
    [SerializeField] private List<GameObject> m_redLetters;
    [SerializeField] private GameObject m_space;
    
    private bool m_isRedWord = false;
    private int m_nbWordChars = 0;
    private int m_visibleChars = 0;
    private float m_visibleCharTime;
    private int m_dialogToDraw = 0;
    
    private List<Letter> m_letters;

    private void Awake()
    {
        m_letters = new List<Letter>();
    }

    private void OnEnable()
    {
        Controller.OnContinuePress += Continue;
    }

    private void OnDisable()
    {
        Controller.OnContinuePress -= Continue;
    }

    private void Update()
    {
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

    public void StartDialog(int _dialogToDraw)
    {
        GameManager.instance.Pause();
        m_dialogToDraw = _dialogToDraw;
        DrawText();
    }
    public void Continue()
    {
        if (m_visibleChars >= m_letters.Count)
        {
            if (m_dialogToDraw > 0) DrawText();
            else
            {
                ResetText();
                GameManager.instance.Resume();
            }
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
        --m_dialogToDraw;
        ResetText();
        gameObject.SetActive(true);

        int totalChar = Random.Range(30, m_dialogLength * 2 - 3);
        Vector2 pos = Vector2.zero;
        

        while (totalChar >= 3)
        {
            m_nbWordChars = 0;
            m_isRedWord = false;
            int remainingLength = m_dialogLength;
            
            while (remainingLength >= 3)
            {
                GameObject letterObject = Instantiate(SelectChar(), m_textBase);
                Letter letter = letterObject.GetComponent<Letter>();
                letter.transform.localPosition = pos * m_pixelSize;
                letter.gameObject.SetActive(false);
                m_letters.Add(letter);
            
                pos.x += letter.size;
                remainingLength -= letter.size;
                totalChar -= letter.size;
            }

            pos.x = 0;
            pos.y -= m_lineMargin;
        }
        m_arrow.gameObject.SetActive(false);
    }

    private GameObject SelectChar()
    {
        if (Random.Range(0f, 5.0f) < m_nbWordChars)
        {
            m_nbWordChars = 0;
            m_isRedWord = Random.Range(0.0f, 1.0f) < 0.1f;
            return m_space;
        }

        ++m_nbWordChars;
        if(m_isRedWord) return m_redLetters[Random.Range(0, m_redLetters.Count)];
         return m_blackLetters[Random.Range(0, m_blackLetters.Count)];
    }
}
