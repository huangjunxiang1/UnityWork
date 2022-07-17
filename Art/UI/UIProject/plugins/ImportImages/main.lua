
local toolMenu = App.menu:GetSubMenu("tool");
toolMenu:AddItem("ImportImages", "test", function(menuItem)
    local pkg = App.project:GetPackageByName("ResPkg");
    local resPath=App.project.basePath.."/../../Texture";

    local DirectoryInfo = CS.System.IO.DirectoryInfo;
    local FileInfo = CS.System.IO.FileInfo;
    local  file = CS.System.IO.File;

    local directoryInfo = DirectoryInfo(resPath);
    local allDirectories = directoryInfo:GetDirectories();
    
    for i = 0, allDirectories.Length-1 do
        local directory = allDirectories[i];
        if(pkg:FindItemByName(directory.Name)==nil) then
           pkg:CreateFolder(directory.Name, nil, false);
        end
    end

    --pkg:CreateFolder("test3", nil, false);
    pkg:Save();
end)