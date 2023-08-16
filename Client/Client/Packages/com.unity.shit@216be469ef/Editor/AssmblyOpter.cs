using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class AssmblyOpter
{
    public string name;
    public string rootNamespace;
    public List<string> references;
    public List<string> includePlatforms;
    public List<string> excludePlatforms;
    public bool allowUnsafeCode;
    public bool overrideReferences;
    public List<string> precompiledReferences;
    public bool autoReferenced;
    public List<string> defineConstraints;
    public List<string> versionDefines;
    public bool noEngineReferences;

    public static AssmblyOpter Load(string path)
    {
        return JsonConvert.DeserializeObject<AssmblyOpter>(File.ReadAllText(path));
    }
    public void Save(string path)
    {
        File.WriteAllText(path, this.ToJson());
    }
    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}
