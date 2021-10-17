using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class CMDL
{
	public const ushort C2R_Login = 20018;
	public const ushort R2C_Login = 20019;
	public const ushort C2G_LoginGate = 20020;
	public const ushort G2C_LoginGate = 20021;
}

enum EventIDL
{
	InScene,//进入场景
	OutScene,//退出场景
}