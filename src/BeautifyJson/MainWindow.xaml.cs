using CommunityToolkit.Mvvm.ComponentModel;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using Wpf.Ui.Controls;

namespace BeautifyJson;

[ObservableObject]
public partial class MainWindow : FluentWindow
{
    [ObservableProperty]
    private string input = string.Empty;

    partial void OnInputChanged(string value)
    {
        try
        {
            Output = JsonBeautifier.Beautify(value);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    [ObservableProperty]
    private string output = string.Empty;

    partial void OnOutputChanged(string value)
    {
        try
        {
            AfterBox.Text = Output;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        RegisterHighlighting();

        BeforeBox.TextChanged += (_, _) =>
        {
            Input = BeforeBox.Text;
        };

        PreviewDragOver += (s, e) =>
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true;
        };

        PreviewDrop += (s, e) =>
        {
            try
            {
                string fileName = (e.Data.GetData(DataFormats.FileDrop) as Array)!.GetValue(0)!.ToString()!;

                Title = $"Beautify Json - {fileName}";
                BeforeBox.Text = File.ReadAllText(fileName);
            }
            catch
            {
            }
        };
    }

    protected override void OnSourceInitialized(EventArgs e)
    {
        base.OnSourceInitialized(e);

        if (WindowBackdrop.IsSupported(WindowBackdropType.Mica))
        {
            Background = new SolidColorBrush(Colors.Transparent);
            WindowBackdrop.ApplyBackdrop(this, WindowBackdropType.Mica);
        }
    }

    private void RegisterHighlighting()
    {
        IHighlightingDefinition luaHighlighting;
        using Stream? s = Application.GetResourceStream(new Uri("/Assets/Highlighting/Json.xshd", UriKind.Relative))?.Stream;
        using XmlReader reader = new XmlTextReader(s);
        luaHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);

        HighlightingManager.Instance.RegisterHighlighting("Json", [".json"], luaHighlighting);
        BeforeBox.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Json");
        AfterBox.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("Json");
    }
}
