using F23.StringSimilarity;

namespace shopping_app;

public static class Similarities
{
    public static List<(double, string?, string?)> Evaluate(List<string?> shopA, List<string?> shopB)
    {
        var similarities = new List<(double, string?, string?)>();
        var similarity = new NormalizedLevenshtein();
        foreach (var itemAName in shopA)
        {
            foreach (var itemBName in shopB)
            {
                if (string.IsNullOrWhiteSpace(itemAName) || string.IsNullOrWhiteSpace(itemBName)) continue;
                var score = similarity.Similarity(itemAName, itemBName);
                if (!(score > 0.8)) continue;
                
                Console.WriteLine($"[{itemAName}]\n" +
                                  $"[{itemBName}]\n" +
                                  $"Similarity: {score}");
                similarities.Add(new ValueTuple<double, string, string>(score, itemAName, itemBName));
            }
        }

        return similarities;
    }
}