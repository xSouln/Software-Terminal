using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI.TemplateSelectors
{
    class OptionsTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is IPortOptionsViewModel viewModel && viewModel.OptionsTemplate != null)
            {
                return viewModel.OptionsTemplate;
            }

            return new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(typeof(ContentControl))
            };
        }
    }
}
