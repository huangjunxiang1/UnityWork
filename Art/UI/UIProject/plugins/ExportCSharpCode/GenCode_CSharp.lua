local function genCode(handler)
    local settings = handler.project:GetSettings("Publish").codeGeneration
    local codePkgName = handler:ToFilename(handler.pkg.name); --convert chinese to pinyin, remove special chars etc.
    local exportCodePath = handler.exportCodePath
    local namespaceName = codePkgName
    
    if settings.packageName~=nil and settings.packageName~='' then
        namespaceName = settings.packageName..'.'..namespaceName;
    end

    --CollectClasses(stripeMemeber, stripeClass, fguiNamespace)
    local classes = handler:CollectClasses(settings.ignoreNoname, settings.ignoreNoname, nil)
    handler:SetupCodeFolder(exportCodePath, "cs") --check if target folder exists, and delete old files

    local getMemberByName = settings.getMemberByName

    local classCnt = classes.Count
    local writer = CodeWriter.new()

        writer:reset()

        writer:writeln('using FairyGUI;')
        writer:writeln('using FairyGUI.Utils;')

    for i=0,classCnt-1 do
        local classInfo = classes[i]

        if(string.len(classInfo.className)>=4 and string.sub(classInfo.className,1,3)=='FUI') then
       
        local members = classInfo.members

        writer:writeln()
        writer:writeln('partial class %s : FUIBase', classInfo.className)
        writer:startBlock()

        local memberCnt = members.Count
        for j=0,memberCnt-1 do
            local memberInfo = members[j]
           if(string.len(memberInfo.name)>=2 and string.sub(memberInfo.name,1,1)=='_') then
            writer:writeln('public %s %s;', memberInfo.type, memberInfo.varName)
            end
        end
        writer:writeln()
        if handler.project.type==ProjectType.MonoGame then
            writer:writeln("protected override void Binding()")
            writer:startBlock()
        else
            writer:writeln('protected override void Binding()')
            writer:startBlock()
        end
        for j=0,memberCnt-1 do
            local memberInfo = members[j]

              if(string.len(memberInfo.name)>=2 and string.sub(memberInfo.name,1,1)=='_')  then

                  if memberInfo.group==0 then
                      if getMemberByName then
                          writer:writeln('%s = (%s)this.UI.GetChild("%s");', memberInfo.varName, memberInfo.type, memberInfo.name)
                      else
                          writer:writeln('%s = (%s)this.UI.GetChildAt(%s);', memberInfo.varName, memberInfo.type, memberInfo.index)
                      end
                  elseif memberInfo.group==1 then
                      if getMemberByName then
                          writer:writeln('%s = this.UI.GetController("%s");', memberInfo.varName, memberInfo.name)
                      else
                          writer:writeln('%s = this.UI.GetControllerAt(%s);', memberInfo.varName, memberInfo.index)
                      end
                  else
                      if getMemberByName then
                          writer:writeln('%s = this.UI.GetTransition("%s");', memberInfo.varName, memberInfo.name)
                      else
                          writer:writeln('%s = this.UI.GetTransitionAt(%s);', memberInfo.varName, memberInfo.index)
                      end
                  end
              end
        end
        writer:endBlock()

        writer:endBlock() --class


        end
    end
        writer:save(exportCodePath..'/'..'FUI.cs')
end

return genCode