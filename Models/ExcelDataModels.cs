namespace CameraView.Models
{
    public class ExcelDataModels
    {
        public int STT { get; set; }
        public string SwitchName { get; set; } = string.Empty;
        public string Port { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public int VLAN { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;
        public List<List<object>> Data { get; set; } = new List<List<object>>();
    }
}
