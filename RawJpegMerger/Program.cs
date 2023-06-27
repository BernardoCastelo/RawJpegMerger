// See https://aka.ms/new-console-template for more information
using RawJpegMerger;
using static RawJpegMerger.Constants;

string rawFolder = "F:\\Photos\\01.RAW";
string photoFolder = "F:\\Photos\\00.LIB";

var frw = new FileReadWritter(rawFolder);

var files = frw.GetAllFileNames(photoFolder);

var results = new List<ReplaceWithRawIfFoundResults>();

var total = files.Count;
var count = 0;

foreach (var file in files)
{
    results.Add(frw.ReplaceWithRawIfFound(file));
    
    count++;
    if (count % 10 == 0)
    {
        Console.WriteLine(100.0 * count / total);
    }
}
