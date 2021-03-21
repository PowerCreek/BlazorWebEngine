using BlazorWebEngine.Components;
using BlazorWebEngine.Management.NodeHandling;
using BlazorWebEngine.Management.OperationHandling;

namespace BlazorWebEngine.Management
{
    public interface IElementServices
    {
        public int id { get; }
        public ComponentMap ComponentMap { get; init; }
        public OperationManager OperationManager { get; init; }
        public NodeManager NodeManager { get; init; }
        public NodeRegistry NodeRegistry { get; init; }
        public NodeInformation NodeInformation { get; init; }
        public ElementContextProvider ElementContextProvider { get; init; }
    }

    public class BackingService : IElementServices
    {
        private static int _id;

        public BackingService(
            IBuilder backBuilder,
            NodeRegistry nodeRegistry,
            NodeInformation nodeInformation,
            OperationManager operationManager,
            ComponentMap componentMap,
            ElementContextProvider elementContextProvider)
        {
            OperationManager = operationManager;
            NodeRegistry = nodeRegistry;
            NodeInformation = nodeInformation;
            ElementContextProvider = elementContextProvider;
            ComponentMap = componentMap;

            backBuilder.Build(this);
        }

        public int id { get; } = _id++;

        public ComponentMap ComponentMap { get; init; }
        public OperationManager OperationManager { get; init; }

        public NodeManager NodeManager { get; init; }
        public NodeRegistry NodeRegistry { get; init; }
        public NodeInformation NodeInformation { get; init; }
        public ElementContextProvider ElementContextProvider { get; init; }
    }
}