using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using GUI.Views;

namespace GUI
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                var vm = new MainWindowViewModel();
                await vm.Initialize();
                desktop.MainWindow.DataContext = vm;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}