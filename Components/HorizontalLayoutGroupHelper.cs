// Copyright 2014 Tokarev Mikhail (also known as Deepscorn)
// http://www.apache.org/licenses/LICENSE-2.0
using UnityEngine;

// Solves problem: HorizontalLayoutGroup successfully alignes children which present as it's children no matter visible they are or not.
// Visibility is controlled by Canvas Group's alpha value. That script just removes child from children list when alpha is 0. So that visible
// elements are aligned properly without gap
// http://answers.unity3d.com/questions/1092073/horizontal-layout-group-doesnt-omit-hidden-childre.html
public class HorizontalLayoutGroupHelper : MonoBehaviour
{
    private CanvasGroup[] children;
    private int[] siblingIndexes;
    private Transform parentTransform;

    void Awake()
    {
        children = GetComponentsInChildren<CanvasGroup>();
        siblingIndexes = new int[children.Length];
        for (int i = 0; i < siblingIndexes.Length; ++i)
        {
            siblingIndexes[i] = children[i].transform.GetSiblingIndex();
        }
        parentTransform = transform.parent;
    }
    
    void Update()
    {
        for (int i = 0; i < siblingIndexes.Length; ++i)
        {
            var child = children[i];
            if (Mathf.Approximately(child.alpha, 0F))
            {
                if (child.transform.parent != parentTransform)
                {
                    child.transform.SetParent(parentTransform);
                }
            }
            else if (child.transform.parent != transform)
            {
                child.transform.SetParent(transform);
                child.transform.SetSiblingIndex(siblingIndexes[i]);
            }
        }
    }
}
