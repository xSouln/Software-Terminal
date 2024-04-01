using xLibV100.Controls;

namespace Terminal.UI
{
    public interface IMqttBrokerAddTopicAction
    {
        object SelectedTopic { get; set; }
        Results AddTopics();
        Results AddTopic();
        void RemoveTopic();
    }
}
