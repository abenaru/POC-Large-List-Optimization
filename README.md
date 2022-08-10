# POC-Large-List-Optimization

A POC with benchmarks suggesting optimization for dynamically-resizable collections backed by arrays (via pooling of arrays for reducing allocations)

# Benchmarks

``` ini

BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22000
Intel Core i7-8565U CPU 1.80GHz (Whiskey Lake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.400
  [Host]     : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT
  DefaultJob : .NET 6.0.8 (6.0.822.36306), X64 RyuJIT


```
|                              Method | NumElements |             Mean |           Error |          StdDev | Ratio | RatioSD | Rank |     Gen 0 |     Gen 1 |     Gen 2 |    Allocated |
|------------------------------------ |------------ |-----------------:|----------------:|----------------:|------:|--------:|-----:|----------:|----------:|----------:|-------------:|
|     StandardListWithInitialCapacity |          32 |         115.5 ns |         2.30 ns |         3.00 ns |  1.00 |    0.00 |    1 |    0.0440 |         - |         - |        184 B |
| NaivePoolingListWithInitialCapacity |          32 |         167.3 ns |         3.20 ns |         3.14 ns |  1.44 |    0.05 |    2 |    0.0076 |         - |         - |         32 B |
|                        StandardList |          32 |         182.3 ns |         3.59 ns |         5.49 ns |  1.58 |    0.07 |    3 |    0.0880 |         - |         - |        368 B |
|                    NaivePoolingList |          32 |         218.6 ns |         4.00 ns |         3.74 ns |  1.88 |    0.07 |    4 |    0.0076 |         - |         - |         32 B |
|                          LinkedList |          32 |         621.2 ns |        12.18 ns |        15.84 ns |  5.38 |    0.17 |    5 |    0.3767 |         - |         - |      1,576 B |
|                                     |             |                  |                 |                 |       |         |      |           |           |           |              |
| NaivePoolingListWithInitialCapacity |         256 |         696.0 ns |         7.05 ns |         6.59 ns |  0.95 |    0.02 |    1 |    0.0076 |         - |         - |         32 B |
|     StandardListWithInitialCapacity |         256 |         735.2 ns |        14.14 ns |        15.13 ns |  1.00 |    0.00 |    2 |    0.2575 |         - |         - |      1,080 B |
|                    NaivePoolingList |         256 |         963.8 ns |         7.70 ns |         7.20 ns |  1.31 |    0.03 |    3 |    0.0076 |         - |         - |         32 B |
|                        StandardList |         256 |         995.1 ns |        12.43 ns |        11.02 ns |  1.36 |    0.03 |    4 |    0.5331 |         - |         - |      2,232 B |
|                          LinkedList |         256 |       4,495.9 ns |        49.28 ns |        46.10 ns |  6.12 |    0.16 |    5 |    2.9449 |         - |         - |     12,329 B |
|                                     |             |                  |                 |                 |       |         |      |           |           |           |              |
| NaivePoolingListWithInitialCapacity |     1048576 |   2,427,703.0 ns |    47,859.33 ns |    93,345.95 ns |  0.77 |    0.02 |    1 |         - |         - |         - |         35 B |
|                    NaivePoolingList |     1048576 |   2,649,415.8 ns |    20,492.80 ns |    17,112.42 ns |  0.82 |    0.01 |    2 |         - |         - |         - |         35 B |
|     StandardListWithInitialCapacity |     1048576 |   3,223,056.3 ns |    22,859.18 ns |    21,382.49 ns |  1.00 |    0.00 |    3 |  312.5000 |  312.5000 |  312.5000 |  4,194,459 B |
|                        StandardList |     1048576 |   5,424,696.6 ns |    68,473.94 ns |    64,050.56 ns |  1.68 |    0.02 |    4 | 1992.1875 | 1992.1875 | 1992.1875 |  8,389,903 B |
|                          LinkedList |     1048576 | 101,643,822.6 ns | 1,360,463.07 ns | 1,206,015.07 ns | 31.54 |    0.42 |    5 | 8833.3333 | 3500.0000 | 1166.6667 | 50,333,723 B |

# Discussion

https://github.com/dotnet/csharplang/discussions/6362