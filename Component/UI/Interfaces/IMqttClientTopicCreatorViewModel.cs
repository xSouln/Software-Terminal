namespace Terminal.UI
{
    public interface IMqttClientTopicCreatorViewModel
    {
        object SelectedTopic { get; set; }
        string PortName { get; set; }
        string TopicName { get; set; }
        void AddTopic();
        void RemoveTopic();
    }
}
