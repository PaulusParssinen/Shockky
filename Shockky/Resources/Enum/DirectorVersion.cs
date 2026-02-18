namespace Shockky.Resources;

public enum DirectorVersion
{
    Unknown,

    //D1 & 2
    V1023 = 0x3FF,
    V1024 = 0x400,

    //D3
    V1025,
    V1028 = 0x404,
    V1029,

    //D4
    V1113 = 0x459,
    V1114,
    V1115,
    V1116,
    V1117,

    //D5
    V1201 = 0x4B1,
    V1210 = 0x4BA,
    V1214 = 0x4BE,
    V1215,
    V1217 = 0x4C1,

    //D6
    V1222 = 0x4C6,
    V1223,

    //D7
    V1406 = 0x57E,

    //D8
    V1600 = 0x640,

    //D8.5
    V1800 = 0x708,

    //D9
    V1850 = 0x73A,

    //D10
    V1858 = 0x742,
    V1860 = 0x744,
    V1921 = 0x781,

    //D11
    V1922,
    V1923,

    //D12
    V1951 = 0x79F,

    Protected = 0x163C
}