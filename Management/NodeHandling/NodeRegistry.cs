using System.Collections.Generic;

namespace BlazorWebEngine.Management.NodeHandling
{
    public class NodeRegistry : IRegisterElement
    {
        public readonly HashSet<int> ElementMap = new();
        public int Indexor;

        public virtual int AddNode()
        {
            var id = Indexor++;
            ElementMap.Add(id);
            return id;
        }

        public virtual void RemoveElement(int id)
        {
            ElementMap.Remove(id);
        }
    }
}