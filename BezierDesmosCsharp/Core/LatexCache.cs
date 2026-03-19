using System.Collections.Concurrent;

namespace BezierCsharp.Core
{
    public static class LatexCache
    {
        private static readonly ConcurrentDictionary<int, List<string>> _latexStringsDict = new();

        public static void AddLatexStrings(int frameId, List<string> latexStrings)
        {
            _latexStringsDict.TryAdd(frameId, latexStrings);
        }

        public static List<string> GetLatexStrings(int frameId)
        {
            if (_latexStringsDict.TryGetValue(frameId, out var latexStrings))
            {
                //Console.WriteLine(string.Join("\n", latexStrings));
                return latexStrings;
            }
            else
            {
                throw new KeyNotFoundException($"Frame ID {frameId} not found in the cache.");
            }
        }

        public static int GetFrameCount()
        {
            foreach (var key in _latexStringsDict.Keys)
            {
                Console.WriteLine($"Frame ID: {key}");
            }


            return _latexStringsDict.Count;
        }
    }
}
