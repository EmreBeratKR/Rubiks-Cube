public class InputChecker : Scenegleton<InputChecker>
{
    public bool isEnabled;
    

    public void IsInputEnabled(bool isValid)
    {
        isEnabled = isValid;
    }
}
