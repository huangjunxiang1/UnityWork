// Test1.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include "RecastDll.h"
#include <fstream>

struct NavMeshSetHeader
{
    int magic;
    int version;
    int numTiles;
};


static const int NAVMESHSET_MAGIC = 'M' << 24 | 'S' << 16 | 'E' << 8 | 'T'; //'MSET';
static const int NAVMESHSET_VERSION = 1;

int main()
{
    const char* path = "D:/UnityWork/Client/MapObj/Main2.bin";
    char* p = (char*)path;


    std::fstream ofile;
    ofile.open(path);
    if (ofile.fail())
    {
        std::cout << "失败";
        return 0;
    }
    
    {
        long l, m;
        ifstream in(filename, ios::in | ios::binary);
        l = in.tellg();
        in.seekg(0, end);
        m = in.tellg();
        in.close();
        std::cout << "size of " << filename;
        std::cout << " is " << (m - l) << " bytes.\n";
    }

    char* p1;
    int idx = 0;
    if (!ofile.eof())
    {
        ofile >> p1[idx++];
    }
    ofile.close();
    std::cout << "长度";
    std::cout << *p1;

    bool b1 = recast_init();
    bool b2 = recast_loadmapOnData(10001, p1);

    std::cout << "\n";
    std::cout << b1 ;
    std::cout << "\n";
    std::cout << b2 ;
    std::cout << "\n";

    system("pause");

    return 0;
}

// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门使用技巧: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
