using System.Collections.Generic;

namespace BlazorWebEngine.Classes
{
    public class ElementBuilder
    {
        public static ElementBuilder Build()
        {
            return new ElementBuilder();
        }

        public List<(string, string)> AttributeList { get; } = new();

        private ElementBuilder(){}
        public ElementBuilder MakeKeyAble(out List<(string,string)> attributeList)
        {
            attributeList = MakeKeyable().AttributeList;
            return this;
        }

        public ElementBuilder MakeKeyable()
        {
            AttributeList.Add(("tabindex","0"));
            return this;
        }
    }
}