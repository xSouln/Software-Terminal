using xLibV100.Controls;

namespace Terminal.UI
{
    public interface IMqttBrokerTopicCreatorViewModel
    {
        string TopicName { get; set; }
        string RxTopic { get; set; }
        string TxTopic { get; set; }
        object SelectedTopic { get; set; }
        Results AddTopic();
        void RemoveTopic();
        Results AddTopics();
    }
}
