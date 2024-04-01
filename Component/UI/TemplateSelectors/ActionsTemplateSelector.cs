using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI.TemplateSelectors
{
    public class ActionsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is IPortViewModel viewModel && viewModel.ActionTemplate != null)
            {
                ContentPresenter contentPresenter = container as ContentPresenter;
                if (contentPresenter != null)
                {
                    contentPresenter.VerticalAlignment = VerticalAlignment.Stretch;
                }
                return viewModel.ActionTemplate;
            }

            return new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(typeof(ContentControl))
            };
        }
    }
}
