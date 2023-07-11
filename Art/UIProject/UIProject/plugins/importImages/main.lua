--FYI: https://github.com/Tencent/xLua/blob/master/Assets/XLua/Doc/XLua_Tutorial_EN.md

function Split(szFullString, szSeparator)  
 local nFindStartIndex = 1  
 local nSplitIndex = 1  
 local nSplitArray = {}  
 while true do  
    local nFindLastIndex = string.find(szFullString, szSeparator, nFindStartIndex)  
    if not nFindLastIndex then  
     nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, string.len(szFullString))  
     break  
    end  
    nSplitArray[nSplitIndex] = string.sub(szFullString, nFindStartIndex, nFindLastIndex - 1)  
    nFindStartIndex = nFindLastIndex + string.len(szSeparator)  
    nSplitIndex = nSplitIndex + 1  
 end  
 return nSplitArray  
 end 

function SyncDir(source,target,path)
    local pkg = CS.FairyEditor.App.project:GetPackageByName("ResPkg");
    local fs = CS.System.IO.Directory.GetFiles(source);

    for i=0,fs.Length-1 do
        local s = string.gsub(fs[i],"\\","/");
        local names = Split(s,"/");
        local name = names[#names];
        local ss = string.gsub(name,".jpg","");
        ss = string.gsub(name,".png","");
        
        if(CS.System.IO.File.Exists(target..name)==false) then
           pkg:ImportResource(fs[i],path,name);
        else
           pkg:UpdateResource(pkg:GetItemByPath(path..ss),fs[i]);
        end
    end

    local ds = CS.System.IO.Directory.GetDirectories(source);
    for i=0,ds.Length-1 do
        local  names = Split(ds[i],"/");
        local  name = names[#names];

        if(CS.System.IO.Directory.Exists(target..name)==false) then
            pkg:CreateFolder(name,path,false);
        end
        SyncDir(source..name.."/",target..name.."/",path..name.."/");
    end
end


function DeleteFile(source,target,path)
    local pkg = CS.FairyEditor.App.project:GetPackageByName("ResPkg");
    local fs = CS.System.IO.Directory.GetFiles(target);
    for i=0,fs.Length-1 do
    
        local s = string.gsub(fs[i],"\\","/");
        local  names = Split(s,"/");
        local  name = names[#names];
        local ss = string.gsub(name,".jpg","");
        ss = string.gsub(name,".png","");
        
        if(CS.System.IO.File.Exists(fs[i])==true) then
           if(CS.System.IO.File.Exists(source..name)==false) then
              item=pkg:GetItemByPath(path..ss);
              if(item~=nil) then
                  pkg:DeleteItem(item);
              end
           end
        end
    end

    local ds = CS.System.IO.Directory.GetDirectories(target);
    for i=0,ds.Length-1 do
        local  names = Split(ds[i],"/");
        local  name = names[#names];

        local forIt = true;

         if(CS.System.IO.Directory.Exists(ds[i])==true) then
           if(CS.System.IO.Directory.Exists(source..name)==false) then
              item=pkg:GetItemByPath(path..name);
              if(item~=nil) then
                  forIt=false;
                  pkg:DeleteItem(item);
              end
           end
        end

        if(forIt) then
            DeleteFile(source..name.."/",target..name.."/",path..name.."/");
        end
    end
end


local toolMenu = App.menu:GetSubMenu("tool");
toolMenu:AddItem("导入美术资源", "importImages", function(menuItem)
    local source = CS.FairyEditor.App.project.basePath.."/../../UI/Sprite/";
    local target = CS.FairyEditor.App.project.basePath.."/assets/ResPkg/";
    local pkg = CS.FairyEditor.App.project:GetPackageByName("ResPkg");

    SyncDir(source,target,"/");
    DeleteFile(source,target,"/");
    
    local items = pkg.items;
    for j=0,items.Count-1 do
        if(items[j].type~="folder") then
            items[j].exported =true;
        end
    end
    
    pkg:SetChanged();
    pkg:Save();
end)


-------do cleanup here-------

function onDestroy()
    toolMenu:RemoveItem("importImages")
end