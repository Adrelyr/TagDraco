using System;
using System.Diagnostics;
using System.Drawing;

namespace TagDraco.Core
{
    public class Tags : IDisposable
    {
        public string   Album               { get; set; } = string.Empty;
        public string   Title               { get; set; } = string.Empty;
        public string[] AlbumArtists        { get; set; } = new string[1];
        public string[] TrackArtists        { get; set; } = new string[1];
        public uint     Year                { get; set; } = 1970;
        public uint     Track               { get; set; } = 0;
        public string[] Genres              { get; set; } = new string[1];
        public Image    AlbumCover          { get; set; } = null;
        public string   FilePath            { get; set; } = string.Empty;

        public Tags()
        {

        }

        ~Tags()
        {
            AlbumCover.Dispose();
            Debug.WriteLine("Tag has been yeeted");
        }

        public Tags(string Album, string Title, string[] Artists, string[] ContributingArtists, uint Year, uint Track, string[] Genres, Image AlbumCover, string FilePath)
        {
            try
            {
                this.Album = Album;
                this.Title = Title;
                this.Genres = Genres;
                this.Year = Year;
                this.AlbumArtists = Artists;
                this.TrackArtists = ContributingArtists;
                this.Track = Track;
                this.AlbumCover = AlbumCover;
                this.FilePath = FilePath;
            }catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string GetJoinedArtists()
        {
             return Join(AlbumArtists);
        }

        public string GetJoinedContributingArtists()
        {
            return Join(TrackArtists);
        }

        public string GetJoinedGenres()
        {
            return Join(Genres);
        }

        public string Join(string[] toJoin)
        {
            string joined = string.Empty;
            for (int i = 0; i < toJoin.Length; i++)
            {
                if (i == toJoin.Length - 1)
                {
                    joined += toJoin[i];
                    continue;
                }
                joined += toJoin[i] + ",";
            }
            return joined;
        }

        public void Dispose()
        {
            if(AlbumCover!=null) AlbumCover.Dispose();
        }
    }
}
