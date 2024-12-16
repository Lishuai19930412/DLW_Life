using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using System;

public class Node
{
    public int id;
    public int limit;
    public string val;
    public List<Node> child;

    public Node(int id, string val, int limit = -1)
    {
        this.id = id;
        this.limit = limit;
        this.val = val;
        child = new List<Node>();
    }
}

public class DAG
{
    public Dictionary<int, Node> Nodes;
    public Node Root;

    public DAG()
    {
        Nodes = new Dictionary<int, Node>();
    }

    public bool AddNode(int id, string value, int limit = -1)
    {
        if (Nodes.ContainsKey(id) == false)
        {
            Nodes.Add(id, new Node(id, value, limit));
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddEdge(int parentID, int childID)
    {
        if (Nodes.ContainsKey(parentID) && Nodes.ContainsKey(childID))
        {
            Nodes[parentID].child.Add(Nodes[childID]);
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class ST_Node
{
    public int id;
    public int limit;
    public string val;
    public int[] child;

    public ST_Node(int id, int limit, string val, int[] child = null)
    {
        this.id = id;
        this.limit = limit;
        this.val = val;
        this.child = child;
    }
}

public class NextBtn : MonoBehaviour
{
    private CanvasGroup m_Cgp;
    private Button m_Btn;
    private Text m_Txt;

    public CanvasGroup Cgp
    {
        get
        {
            if (m_Cgp == null)
            {
                m_Cgp = this.transform.GetComponent<CanvasGroup>();
            }
            return m_Cgp;
        }
    }
    public Button Btn
    {
        get
        {
            if (m_Btn == null)
            {
                m_Btn = this.transform.GetComponent<Button>();
            }
            return m_Btn;
        }
    }
    public Text Txt
    {
        get
        {
            if(m_Txt == null)
            {
                m_Txt = this.transform.GetComponentInChildren<Text>();
            }
            return m_Txt;
        }
    }
}

public class GlobalManager : MonoBehaviour
{
    [SerializeField] private Image m_Img_Bg;
    [SerializeField] private Text m_Txt_StartTxt;
    [SerializeField] private Button m_Btn_Pbf;

    private DAG m_Conf;
    private Node m_Node_RT;
    private NextBtn m_Btn_RT;
    private List<NextBtn> m_Btn_Next = new List<NextBtn>(10);
    private bool m_IsStart = true;

    private void Awake()
    {
        List<ST_Node> nodeDt = new List<ST_Node>()
        {
            new ST_Node(0, -1, "2024��Ϸ��ҵ����̣������������Ա������Ϊ������������ƾ֣�", new int[] {1, 2, 3, 4 }),

            new ST_Node(1, 1, "��ָ�Unity", new int[] {10, 11, 12 }),
            new ST_Node(2, 1, "ת����С��Ϸ", new int[] {13, 19, 3, 4 }),
            new ST_Node(3, 1, "ת�������", new int[] {14, 19, 2, 3 }),
            new ST_Node(4, 1, "�˳�IT��ҵ", new int[] {15, 16, 17, 18 }),

            new ST_Node(10, 1, "ȥ��Ƥ��˾", new int[] {20 }),
            new ST_Node(11, 1, "ȥ���˹�˾", new int[] {90 }),
            new ST_Node(12, 1, "���ĸ�н��", new int[] {14, 2, 3, 4 }),
            new ST_Node(13, 1, "���ܸ�ǿ��997", new int[] {21, 19, 3, 4 }),
            new ST_Node(14, -1, "����offer���ټ�ּ��", new int[] {91 }),
            new ST_Node(15, 1, "������", new int[] {22, 23, 24, 25 }),
            new ST_Node(16, 1, "����ý��", new int[] {22, 23, 24, 25 }),
            new ST_Node(17, 1, "���߻�/��Ӫ", new int[] {22, 23, 25 }),
            new ST_Node(18, 1, "��̯", new int[] {26, 27, 25 }),
            new ST_Node(19, 1, "��ȥ��Unity", new int[] {10, 11, 12 }),

            new ST_Node(20, -1, "��˾����/����ʧҵ", new int[] {11, 12, 2, 3, 4 }),
            new ST_Node(21, -1, "ҧҧ�������ȥ", new int[] {92 }),

            new ST_Node(22, -1, "������ѵ��������ְ", new int[] {91 }),
            new ST_Node(23, -1, "��ѧ���У���Ͷ����", new int[] {14 }),
            new ST_Node(24, -1, "�����Ż���������Ӫ", new int[] {30, 31 }),
            new ST_Node(25, -1, "����ת�а�", new int[] {15, 16, 17, 18, 19 }),

            new ST_Node(26, 1, "������ѵ������Ʒ��", new int[] {93 }),
            new ST_Node(27, 1, "ȫ���Լ��㶨��", new int[] {93 }),

            new ST_Node(30, -1, "Ͷ������������͸�", new int[] {93 }),
            new ST_Node(31, -1, "�����Ͷ�ʣ������Լ���Ӫ", new int[] {91 }),

            new ST_Node(90, -1, "die/��������飬�"),
            new ST_Node(91, -1, "die/����ɽ�ն������"),
            new ST_Node(92, -1, "die/���"),
            new ST_Node(93, -1, "die/�벻�����Ѫ���޹飬�")
        };

        m_Conf = new DAG();

        for(int i = 0; i < nodeDt.Count; i++)
        {
            m_Conf.AddNode(nodeDt[i].id, nodeDt[i].val, nodeDt[i].limit);
        }

        for (int i = 0; i < nodeDt.Count; i++)
        {
            if(nodeDt[i].child != null)
            {
                for (int j = 0; j < nodeDt[i].child.Length; j++)
                {
                    m_Conf.AddEdge(nodeDt[i].id, nodeDt[i].child[j]);
                }
            }
        }

        m_Conf.Root = m_Conf.Nodes[0];
        m_Node_RT = m_Conf.Root;
    }
    private void Start()
    {
        //----------------------------��������----------------------------
        m_Txt_StartTxt.text = "";
        m_Btn_Next = CreateNextBtn();
        for (int i = 0; i < m_Btn_Next.Count; i++)
        {
            ((RectTransform)m_Btn_Next[i].transform).anchoredPosition = new Vector2(-480, 100 - 80 * i);
            m_Btn_Next[i].Btn.targetGraphic.color = Color.white;
            m_Btn_Next[i].Cgp.alpha = 0;
            m_Btn_Next[i].Cgp.blocksRaycasts = false;
        }

        Sequence start = DOTween.Sequence();
        start.AppendInterval(1);
        start.Append(m_Txt_StartTxt.DOText(m_Conf.Root.val, 5).SetEase(Ease.Linear));
        start.AppendInterval(1);
        float baseTime = start.Duration(false);

        for (int i = 0; i < m_Btn_Next.Count; i++)
        {
            start.Insert(baseTime + i * 0.15f, ((RectTransform)m_Btn_Next[i].transform).DOAnchorPos(new Vector2(0, 100 - 80 * i), 1f));
            start.Insert(baseTime + i * 0.15f, m_Btn_Next[i].Cgp.DOFade(1, 0.7f));
        }

        start.AppendCallback(() =>
        {
            for (int i = 0; i < m_Btn_Next.Count; i++)
            {
                m_Btn_Next[i].Cgp.blocksRaycasts = true;
            }
        });
    }

    private List<NextBtn> CreateNextBtn()
    {
        List<NextBtn> btnList = new List<NextBtn>(10);

        for (int i = 0; i < m_Node_RT.child.Count; i++)
        {
            int tp = i;
            if (m_Node_RT.child[i].limit > 0 || m_Node_RT.child[i].limit == -1)
            {
                Button btn = Instantiate(m_Btn_Pbf, m_Img_Bg.rectTransform);
                ((RectTransform)btn.transform).sizeDelta = new Vector2(380, 50);
                NextBtn nbtn = btn.gameObject.AddComponent<NextBtn>();
                nbtn.Txt.text = m_Node_RT.child[i].val;
                nbtn.Btn.onClick.AddListener(() => { ClickNextBtn(tp); });
                btnList.Add(nbtn);
            }
        }

        return btnList;

        //int btnId = 0;
        //for (int i = 0; i < m_Node_RT.child.Count; i++)
        //{
        //    int tp = i;
        //    if (m_Node_RT.child[i].limit > 0 || m_Node_RT.child[i].limit == -1)
        //    {
        //        if (i >= m_Btn_Next.Count)
        //        {
        //            Button btn = Instantiate(m_Btn_Pbf, m_Img_Bg.rectTransform);
        //            ((RectTransform)btn.transform).sizeDelta = new Vector2(380, 50);
        //            NextBtn nbtn = btn.gameObject.AddComponent<NextBtn>();
        //            nbtn.Txt.text = m_Node_RT.child[i].val;
        //            nbtn.Btn.onClick.AddListener(() => { ClickNextBtn(tp); }); 
        //            m_Btn_Next.Add(nbtn);
        //        }
        //        else
        //        {
        //            m_Btn_Next[btnId].Txt.text = m_Node_RT.child[i].val;
        //            m_Btn_Next[btnId].Btn.onClick.RemoveAllListeners();
        //            m_Btn_Next[btnId].Btn.onClick.AddListener(() => { ClickNextBtn(tp); });
        //        }
        //        btnId++;
        //    }
        //}

        //for(int i = 0; i < m_Btn_Next.Count; i++)
        //{
        //    if (i < btnId)
        //    {
        //        m_Btn_Next[i].gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        Destroy(m_Btn_Next[i].gameObject);
        //    }
        //}

        //m_Btn_Next.RemoveRange(btnId, m_Btn_Next.Count - btnId);
    }

    private void ClickNextBtn(int id)
    {
        Sequence seq = DOTween.Sequence();

        if(m_Node_RT.child[id].limit > 0)
        {
            m_Node_RT.child[id].limit -= 1;
        }

        if (m_IsStart == true)//����ǴӸ��ڵ㿪ʼ�ĵ����ť
        {
            m_IsStart = false;
            seq.Insert(0, m_Txt_StartTxt.DOColor(new Color(1, 1, 1, 0), 1f));
        }
        else//����ǴӸ��ڵ����Ŀ�ʼ�ĵ����ť
        {
            NextBtn oldBtnRT = m_Btn_RT;
            seq.Insert(0, oldBtnRT.Cgp.DOFade(0, 1f).OnComplete(() => { Destroy(oldBtnRT.gameObject); }));
        }

        m_Btn_RT = m_Btn_Next[id];
        m_Btn_RT.transform.SetSiblingIndex(1);
        m_Btn_RT.Cgp.blocksRaycasts = false;
        m_Node_RT = m_Node_RT.child[id];
        m_Btn_Next.Remove(m_Btn_RT);
        List<NextBtn> oldBtn = m_Btn_Next;   

        for (int i = 0; i < oldBtn.Count; i++)
        {
            int tp = i;
            oldBtn[i].Cgp.blocksRaycasts = false;
            seq.Insert(0, oldBtn[i].Cgp.DOFade(0, 1f).OnComplete(() => { Destroy(oldBtn[tp].gameObject); }));
        }
        
        seq.Insert(1f, ((RectTransform)m_Btn_RT.transform).DOAnchorPos(new Vector2(0, 250), 0.7f));
        seq.Insert(1f, ((RectTransform)m_Btn_RT.transform).DOSizeDelta(new Vector2(380, 80), 0.5f));
        seq.Insert(1f, m_Btn_RT.Btn.targetGraphic.DOColor(new Color32(255, 240, 120, 255), 0.5f));

        m_Btn_Next = CreateNextBtn();
        for (int i = 0; i < m_Btn_Next.Count; i++)
        {
            ((RectTransform)m_Btn_Next[i].transform).anchoredPosition = new Vector2(-480, 100 - 80 * i);
            m_Btn_Next[i].Btn.targetGraphic.color = Color.white;
            m_Btn_Next[i].Cgp.alpha = 0;
            m_Btn_Next[i].Cgp.blocksRaycasts = false;
        }

        float baseTime = seq.Duration(false);

        for (int i = 0; i < m_Btn_Next.Count; i++)
        {
            seq.Insert(baseTime + i * 0.15f, ((RectTransform)m_Btn_Next[i].transform).DOAnchorPos(new Vector2(0, 100 - 80 * i), 0.8f));
            seq.Insert(baseTime + i * 0.15f, m_Btn_Next[i].Cgp.DOFade(1, 0.6f));
        }

        seq.AppendCallback(() =>
        {
            for (int i = 0; i < m_Btn_Next.Count; i++)
            {
                m_Btn_Next[i].Cgp.blocksRaycasts = true;
            }
        });
    }
}


