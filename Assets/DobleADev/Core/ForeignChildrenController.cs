using System.Collections.Generic;
using UnityEngine;

namespace DobleADev.Core
{
    public class ForeignChildrenController : MonoBehaviour
    {
        [SerializeField] private List<Transform> _children;

        public bool Contains(Transform children)
        {
            return _children.Contains(children);
        }
        
        public void InstatiateChildren(GameObject prefab)
        {
            _children.Add(Instantiate(prefab).transform);
        }

        public void InstatiateChildrenInside(GameObject prefab)
        {
            _children.Add(Instantiate(prefab, transform).transform);
        }

        public void AddChildren(Transform children)
        {
            _children.Add(children);
        }

        public void DestroyChildren(Transform children)
        {
            if (!Contains(children)) return;
            _children.Remove(children);
            Destroy(children);
        }

        public void DestroyAllChildren()
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                Destroy(children);
            }
            _children.Clear();
            
        }

        public void EnableChildren()
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                children.gameObject.SetActive(true);
            }
                
        }

        public void DisableChildren()
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                children.gameObject.SetActive(false);
            }
        }

        public void SetChildrenPosition(Vector3 worldPosition)
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                children.position = worldPosition;
            }
        }

        public void SetChildrenLocalPosition(Vector3 localPosition)
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                children.localPosition = localPosition;
            }
        }

        public void TranslateChildren(Vector3 translation)
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                children.Translate(translation);
            }
        }

        public void SendMessageToChildren(string methodName)
        {
            foreach (var children in _children)
            {
                if (children == null) continue;
                children.SendMessage(methodName);
            }
        }

        

        // public void SendMessageToChildren(string methodName, params object[] args)
        // {
            
        // }
    }
}
