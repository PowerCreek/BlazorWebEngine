using System.Collections.Generic;

namespace BlazorWebEngine.Management.NodeHandling
{
    public class NodeRegistry : IRegisterElement
    {
        public int Indexor = 0;
        public readonly HashSet<int> ElementMap = new HashSet<int>();
        
        public virtual int AddNode()
        {
            int id = Indexor++;
            ElementMap.Add(id);
            return id;
        }

        public virtual void RemoveElement(int id)
        {
            ElementMap.Remove(id);
        }
    }
}