namespace CameraView.Models
{
    public class ExcelDataModels
    {
        public List<List<object>> Data { get; set; } = new List<List<object>>();
        public string? SwitchName { get; internal set; }
    }
}
