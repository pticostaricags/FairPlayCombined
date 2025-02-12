namespace FairPlayCombined.Models.InstagramApi
{

    public class UserMediaModel
    {
        public Datum[]? data { get; set; }
        public Paging? paging { get; set; }
    }

    public class Paging
    {
        public Cursors? cursors { get; set; }
    }

    public class Cursors
    {
        public string? before { get; set; }
        public string? after { get; set; }
    }

    public class Datum
    {
        public string? id { get; set; }
    }

}
