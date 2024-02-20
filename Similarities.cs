using F23.StringSimilarity;

namespace shopping_app;

public static class Similarities
{
    /// <summary>
    /// Evaluates the similarity of each entry in each list against each other
    /// and adds it to the list to be returned if it is above the similarity threshold
    /// </summary>
    /// <param name="listA"></param>
    /// <param name="listB"></param>
    /// <returns>
    /// A list of tuples of score, with each word being compared
    /// </returns>
    public static List<(double score, string? wordA, string? wordB)> Evaluate(List<string?> listA, List<string?> listB)
    {
        const double similarityThreshold = 0.8;
        
        var similarities = new List<(double, string?, string?)>();
        var similarity = new NormalizedLevenshtein();
        foreach (var itemAName in listA)
        {
            foreach (var itemBName in listB)
            {
                if (string.IsNullOrWhiteSpace(itemAName) || string.IsNullOrWhiteSpace(itemBName)) continue;
                var score = similarity.Similarity(itemAName, itemBName);
                if (!(score > similarityThreshold)) continue;
                
                Console.WriteLine($"[{itemAName}]\n" +
                                  $"[{itemBName}]\n" +
                                  $"Similarity: {score}");
                similarities.Add(new ValueTuple<double, string, string>(score, itemAName, itemBName));
            }
        }

        return similarities;
    }
}