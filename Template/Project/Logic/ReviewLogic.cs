using System.Text.Json;
using System.IO;
using System.Linq;

public class ReviewLogic
{
    private ReviewAcces _reviewAcces = new ReviewAcces();

    public List<ReviewModel> GetReviewsForMovie(long movieId)
    {
        try
        {
            var reviewsFile = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "reviews.json"));
            if (!File.Exists(reviewsFile)) return new List<ReviewModel>();

            var json = File.ReadAllText(reviewsFile);

            try
            {
                var typed = JsonSerializer.Deserialize<List<ReviewModel>>(json);
                if (typed != null)
                {
                    return typed.Where(r =>
                    {
                        try
                        {
                            var p = r.GetType().GetProperty("MovieId") ?? r.GetType().GetProperty("MovieID");
                            if (p == null) return false;
                            var val = p.GetValue(r);
                            return val != null && Convert.ToInt64(val) == movieId;
                        }
                        catch { return false; }
                    }).ToList();
                }
            }
            catch { }

            
            var dtoList = JsonSerializer.Deserialize<List<ReviewDto>>(json) ?? new List<ReviewDto>();
            var result = new List<ReviewModel>();
            foreach (var dto in dtoList)
            {
                if (dto.MovieId != movieId) continue;
                var rm = Activator.CreateInstance<ReviewModel>();
                void SetIfPossible(string name, object? value)
                {
                    var prop = rm.GetType().GetProperty(name);
                    if (prop != null && prop.CanWrite && value != null)
                    {
                        try { prop.SetValue(rm, Convert.ChangeType(value, prop.PropertyType)); } catch { }
                    }
                }
                SetIfPossible("MovieId", dto.MovieId);
                SetIfPossible("Title", dto.Title);
                SetIfPossible("Rating", dto.Rating);
                SetIfPossible("Comment", dto.Comment);
                result.Add(rm);
            }

            return result;
        }
        catch
        {
            return new List<ReviewModel>();
        }
    }

    private class ReviewDto
    {
        public long MovieId { get; set; }
        public string Title { get; set; } = "";
        public int Rating { get; set; }
        public string Comment { get; set; } = "";
    }
}