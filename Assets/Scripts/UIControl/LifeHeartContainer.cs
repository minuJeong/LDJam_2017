using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeHeartContainer : MonoBehaviour
{
    [SerializeField] public GameObject m_ContainerPivot;
    [SerializeField] public Image m_HeartImage;

    private readonly List<Image> _currentHearts = new List<Image>();

    public void SetHeartcount(int count)
    {
        _currentHearts.ConvertAll((img) => img.gameObject).ForEach(Destroy);
        _currentHearts.Clear();
        for (int i = 0; i < count; i++)
        {
            _currentHearts.Add(Instantiate(m_HeartImage, m_ContainerPivot.transform));
        }
    }
}
