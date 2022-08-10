/*MIT License

Copyright (c) 2022 Abner Ferreira

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using FastCollections.Poc.Implementation;

namespace FastCollections.Benchmarks;

[MemoryDiagnoser, RankColumn, Orderer(SummaryOrderPolicy.FastestToSlowest), MarkdownExporterAttribute.GitHub]
public class ListOptimizationsBenchmark
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once MemberCanBePrivate.Global
    [Params(32, 256, 1_048_576)]
    public int NumElements { get; set; }

    [Benchmark]
    public void StandardList() => _ = AddInts<List<int>>();

    [Benchmark(Baseline = true)]
    public void StandardListWithInitialCapacity() => _ = AddInts(new List<int>(NumElements));

    [Benchmark]
    public void NaivePoolingList() => AddDisposableInts();

    [Benchmark]
    public void NaivePoolingListWithInitialCapacity() => AddDisposableInts(new ArrayPoolingList<int>(NumElements));

    [Benchmark]
    public void LinkedList() => AddInts<LinkedList<int>>();

    private void AddDisposableInts(ArrayPoolingList<int> list) => AddInts(list).Dispose();

    private void AddDisposableInts() => AddDisposableInts(new ArrayPoolingList<int>());

    private TList AddInts<TList>() where TList : ICollection<int>, new() =>
        AddInts(new TList());

    private TList AddInts<TList>(TList list) where TList : ICollection<int>, new()
    {
        for (var i = 0; i < NumElements; i++)
        {
            list.Add(i);
        }

        return list;
    }
}