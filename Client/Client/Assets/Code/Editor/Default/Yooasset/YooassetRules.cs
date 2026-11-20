using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YooAsset.Editor;

internal class AddressGroupDirectoryFile: IAddressRule
{
    public string GetAssetAddress(AddressRuleData data)
    {
        var fi = new FileInfo(data.AssetPath);
        return $"{data.GroupName}_{fi.Directory.Name}_{Path.GetFileNameWithoutExtension(data.AssetPath)}";
    }
}
