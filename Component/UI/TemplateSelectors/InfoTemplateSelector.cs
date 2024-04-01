using System.Windows;
using System.Windows.Controls;

namespace Terminal.UI.TemplateSelectors
{
    class InfoTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null && item is IPortInfoViewModel viewModel && viewModel.InfoTemplate != null)
            {
                return viewModel.InfoTemplate;
            }

            return new DataTemplate
            {
                VisualTree = new FrameworkElementFactory(typeof(ContentControl))
            };
        }
    }
}
