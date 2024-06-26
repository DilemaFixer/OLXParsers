using Newtonsoft.Json;

namespace SBot.Save;

public class JsonDataSaver<TargetType>
{
    private JsonSerializerSettings _settings;
    private string _pathToJson;
    private string _fileName;
    private Func<TargetType> _getDefault;
    private FileStream _stream;
    public JsonDataSaver(string path, string fileName ,  Func<TargetType> getDefault)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName) || !IsFileNameValid(fileName))
            throw new NullReferenceException("Input data invalid!");

        _getDefault = getDefault;
        _fileName = fileName;
        _pathToJson = Path.Combine(path, fileName);
        
        CheckIsPathValid(path);
    }
    
    public JsonDataSaver(string path, string fileName , Func<TargetType> getDefault, JsonSerializerSettings settings)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName) || !IsFileNameValid(fileName))
            throw new NullReferenceException("Input data invalid!");

        if (!fileName.Contains(".json"))
            throw new ArgumentException("Json file must end but '.json' , pls add to name and restart");
        
        _getDefault = getDefault;
        _fileName = fileName;
        _settings = settings;
        _pathToJson = Path.Combine(path, fileName);
        CheckIsPathValid(path);
    }
    
    public TargetType Load()
    {
        try
        {
            string json = File.ReadAllText(_pathToJson);
            
            if(_settings == null)
                return JsonConvert.DeserializeObject<TargetType>(json) ?? throw new NullReferenceException("settings is null");
            else 
                return JsonConvert.DeserializeObject<TargetType>(json , _settings) ?? throw new NullReferenceException();
        }
        catch (FileNotFoundException e)
        {
            var defaultObj = _getDefault.Invoke();
            Save(defaultObj);
            return defaultObj;
        }
    }

    public void Save(TargetType data)
    {
        string json = "";
        
        if(_settings == null)
            json = JsonConvert.SerializeObject(data);
        else
            json = JsonConvert.SerializeObject(data , _settings);
        
        File.WriteAllText(_pathToJson , json);
    }
    
    private bool IsFileNameValid(string fileName)
    {
        char[] invalidChars = Path.GetInvalidFileNameChars();

        bool isValid = !fileName.Any(c => invalidChars.Contains(c));

        return isValid;
    }


    private void CreateFile(string path)
    {
       _stream = File.Create(path);
    }

    private void CheckIsPathValid(string path)
    {
        if (!Directory.Exists(path))
            throw new FileNotFoundException("Invalid path to target directory");
    }
}