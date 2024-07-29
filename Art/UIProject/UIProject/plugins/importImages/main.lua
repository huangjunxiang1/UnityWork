--FYI: https://github.com/Tencent/xLua/blob/master/Assets/XLua/Doc/XLua_Tutorial_EN.md


function SyncDir(source,target,path,pkgName)
    local pkg = CS.FairyEditor.App.project:GetPackageByName(pkgName);
    local fs = CS.System.IO.Directory.GetFiles(source);

    for i=0,fs.Length-1 do
    
        local name =CS.System.IO.FileInfo(fs[i]).Name;
        local ss = string.gsub(name,".jpg","");
        ss = string.gsub(ss,".png","");
        
        if(CS.System.IO.File.Exists(target..name)==false) then
           pkg:ImportResource(fs[i],path,name);
        else
           pkg:UpdateResource(pkg:GetItemByPath(path..ss),fs[i]);
        end
    end

    local ds = CS.System.IO.Directory.GetDirectories(source);
    for i=0,ds.Length-1 do
        local  name =CS.System.IO.DirectoryInfo(ds[i]).Name;
        
        if(CS.System.IO.Directory.Exists(target..name)==false) then
            pkg:CreateFolder(name,path,false);
        end
        SyncDir(source..name.."/",target..name.."/",path..name.."/",pkgName);
    end
end

function string.ends(String, End)
    return End == '' or string.sub(String, -string.len(End)) == End
end
function DeleteFile(source,target,path,pkgName)
    local pkg = CS.FairyEditor.App.project:GetPackageByName(pkgName);
    local fs = CS.System.IO.Directory.GetFiles(target);

    for i=0,fs.Length-1 do
         local name =CS.System.IO.FileInfo(fs[i]).Name;
         local igoner = string.ends(name, ".jta") or string.ends(name, ".fnt")  or string.ends(name, ".xml");
         if(igoner==false) then
             local ss = string.gsub(name,".jpg","");
             ss = string.gsub(ss,".png","");
             
             if(CS.System.IO.File.Exists(fs[i])==true) then
                if(CS.System.IO.File.Exists(source..name)==false) then
                   item=pkg:GetItemByPath(path..ss);
                   if(item~=nil) then
                       pkg:DeleteItem(item);
                   end
                end
             end
         end
    end

    local ds = CS.System.IO.Directory.GetDirectories(target);
    for i=0,ds.Length-1 do
        local  name =CS.System.IO.DirectoryInfo(ds[i]).Name;

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
            DeleteFile(source..name.."/",target..name.."/",path..name.."/",pkgName);
        end
    end
end


local toolMenu = App.menu:GetSubMenu("tool");
local source = CS.FairyEditor.App.project.basePath.."/../../UI/Sprite/";
local target = CS.FairyEditor.App.project.basePath.."/assets/";

local ds=CS.System.IO.Directory.GetDirectories(source);
for i=0,ds.Length-1 do
      local name = CS.System.IO.DirectoryInfo(ds[i]).Name;
      toolMenu:AddItem("导入美术资源包="..name,"importImages", function(menuItem)
         local pkg = CS.FairyEditor.App.project:GetPackageByName(name);
         if(pkg==nil) then
		 	pkg = CS.FairyEditor.App.project:CreatePackage(name);
		 end
         
         SyncDir(source..name.."/",target..name.."/","/",name);
         DeleteFile(source..name.."/",target..name.."/","/",name);
         
         local items = pkg.items;
         for j=0,items.Count-1 do
             if(items[j].type~="folder"and string.sub(items[j].name,1,1)~='@') then
                 items[j].exported =true;
             else
                 items[j].exported =false;
             end
         end

         pkg:SetChanged();
         pkg:Save();

         local items = pkg.items;
         local xml=CS.FairyEditor.XMLExtension.Load(target..name.."/package.xml");
         local elements = xml.elements[0].elements;
         for j=0,items.Count-1 do
             if(items[j].type=="image") then
                local xmlItem = nil;
                for k=0,elements.Count-1 do
                   if(elements[k]:GetAttribute("id","")==items[j].id) then
                      xmlItem=elements[k];
                   end
                end
                
                if (xmlItem~=nil) then
                   if(items[j].width*items[j].height >= 256*256) then
                       xmlItem:SetAttribute("atlas","alone_npot");
                    else
                       xmlItem:RemoveAttribute("atlas");
                    end
                end
            end
        end

        CS.System.IO.File.WriteAllText(target..name.."/package.xml",xml:ToXMLString(true));

        CS.FairyEditor.App.RefreshProject();

      end);
end

toolMenu:AddItem("导入美术资源=所有包", "importImages", function(menuItem)

    local ds=CS.System.IO.Directory.GetDirectories(source);
    for i=0,ds.Length-1 do

        local name = CS.System.IO.DirectoryInfo(ds[i]).Name;
        local pkg = CS.FairyEditor.App.project:GetPackageByName(name);
        if(pkg==nil) then
			pkg = CS.FairyEditor.App.project:CreatePackage(name);
		end
        
        SyncDir(source..name.."/",target..name.."/","/",name);
        DeleteFile(source..name.."/",target..name.."/","/",name);
        
        local items = pkg.items;
        for j=0,items.Count-1 do
            if(items[j].type~="folder" and string.sub(items[j].name,1,1)~='@') then
                items[j].exported =true;
            else
                items[j].exported =false;
            end
            
            if(items[j].type=="image") then
               if(items[j].width*items[j].height >= 256*256) then
                  items[j].folderAtlas="alone_npot";
               else
                  items[j].folderAtlas=nil;
               end
            end
        end
        
        pkg:SetChanged();
        pkg:Save();

        local items = pkg.items;
         local xml=CS.FairyEditor.XMLExtension.Load(target..name.."/package.xml");
         local elements = xml.elements[0].elements;
         for j=0,items.Count-1 do
             if(items[j].type=="image") then
                local xmlItem = nil;
                for k=0,elements.Count-1 do
                   if(elements[k]:GetAttribute("id","")==items[j].id) then
                      xmlItem=elements[k];
                   end
                end
                
                if (xmlItem~=nil) then
                   if(items[j].width*items[j].height >= 256*256) then
                       xmlItem:SetAttribute("atlas","alone_npot");
                    else
                       xmlItem:RemoveAttribute("atlas");
                    end
                end
            end
        end

        CS.System.IO.File.WriteAllText(target..name.."/package.xml",xml:ToXMLString(true));

        CS.FairyEditor.App.RefreshProject();

    end

end)


-------do cleanup here-------

function onDestroy()
    toolMenu:RemoveItem("importImages")
end