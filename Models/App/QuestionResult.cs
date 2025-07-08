using System.Windows.Media;
using TestMaster;

public class QuestionResult
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public string IsCorrectIcon => IsCorrect ? "✔" : "✖";

    public Brush IconColor => IsCorrect
        ? (Brush)App.Current.Resources["AccentMint"]
        : (Brush)App.Current.Resources["AccentRed"];
}