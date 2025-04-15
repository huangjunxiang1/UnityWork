using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;

//begin#1
//internal class ComponentFilter<?[T]+, ?> : ComponentFilter ?where [T] : SComponent+ ?
//{
//?    public [T] [t];+\r?
//    public override void _addTo_HandlesList()
//    {
//?        [t]._Handles.Add(this);+\r?
//    }
//    public override void _addTo_kvHandlesList() 
//    {
//        if (!kv._Handles.Contains(this))
//            kv._Handles.Add(this);
//    }
//    public override void _handle_waitRemove(ICollection<SComponent> hash)
//    {
//?        hash.Add([t]);+\r?
//    }
//    public override SComponent GetFirstComponent() => t;
//}
//end
