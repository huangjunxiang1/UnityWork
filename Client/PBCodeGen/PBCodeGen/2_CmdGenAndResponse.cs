using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class CmdGenAndResponse
{
    public static void Gen(PBParserResult ret)
    {
        for (int i = 0; i < ret.pbs.Count; i++)
        {
            for (int j = 0; j < ret.pbs[i].classes.Count; j++)
            {
                var pb = ret.pbs[i].classes[j];
                int cmd = 0;
                for (int k = 0; k < pb.name.Length; k++)
                    cmd ^= (int)pb.name[k] << (k % 4 * 8);
                pb.cmd = cmd.ToString();
                if (pb.name.StartsWith("C2S"))
                {
                    var response = pb.name.Replace("C2S", "S2C");
                    ret.classMap.TryGetValue(response, out pb.Response);
                }
            }
        }
    }
}
