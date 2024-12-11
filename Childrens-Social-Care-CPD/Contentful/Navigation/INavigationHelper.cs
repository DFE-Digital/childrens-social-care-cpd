namespace Childrens_Social_Care_CPD.Contentful.Navigation;

public interface INavigationHelper
{
    public NavigationLocation Next { get; }
    public NavigationLocation Previous { get; }
}